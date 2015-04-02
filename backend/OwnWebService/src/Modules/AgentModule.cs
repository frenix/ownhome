/*
 * Created by Fuego, Inc. 
 * File  :   Agent.cs
 * Author:    Efren Duran
 * Date: 3/17/2015
 * Time: 10:56 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.IO;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using OHWebService.Models;
using OHWebService.Authentication;

namespace OHWebService.Modules
{
	/// <summary>
	/// The root for this service is http://<ip></ip>:<port></port>/Agent
	/// </summary>
	public class AgentModule : Nancy.NancyModule
	{
	    const String AgentPage = @"
                                <html><body>
                                <h1>Agent Page </h1>
                                </body></html>
                                ";
	    	    
		public AgentModule() : base("/Agents")
		{
		    // /Agent
		    Get["/"] = parameter => { return GetAll(); };
			
			// /Agent/99/pass [temp]
			Get["/{Username}/{Password}"] = parameter => 
			{ 
				var profile = this.Bind<AgentModel>();
				
				Guid g = GuidCreator.New();
				Console.WriteLine(g);
				
				int s = SendMail.Send();
				
				return IsValidUser(profile);
			};
			
			// /Agent/99
			Get["/{id}"] = parameter => { return GetById(parameter.id); };			
			
			// /Agent/root@ownhome.com
			Get["/{emailadd}"] = parameter => { return GetByEmailAdd(parameter.emailadd); };	
			
			// /Agent       POST: Agent JSON in body
			Post["/"] = parameter => { return this.AddAgent(); };
			
			// /Agent        DELETE: {AgentId}
			Delete["/{id}"] = parameter => { return this.DeleteAgent(parameter.id); };
		}
		
		// -- IMPLEMENTATION PART --
		
		// GET /Agents/99
		private object GetById(int id)
		{
			try
			{
				// create a connection to the PetaPoco orm and try to fetch and object with the given Id
				AgentContext ctx = new AgentContext();
				AgentModel res = ctx.GetById(id);
				if (res == null)   // a null return means no object found
				{
					// return a reponse conforming to REST conventions: a 404 error
					return ErrorBuilder.ErrorResponse(this.Request.Url.ToString(), "GET", HttpStatusCode.NotFound, String.Format("Agent with Id = {0} does not exist", id));
				}
				else
				{
					// success. The Nancy server will automatically serialise this to JSON
					return res;
				}
			}
			// Please, please handle exceptions in a way that provides information about the context of the error.
			catch (Exception e)
			{
				return CommonModule.HandleException(e, String.Format("AgentModule.GetById({0})", id), this.Request);
			}
		}
		
		// GET /Agents/root@ownhome.com
		// DOESNT WORK!!!! email as param
		private object GetByEmailAdd(string emailadd)
		{
			try
			{
				// create a connection to the PetaPoco orm and try to fetch and object with the given Id
				AgentContext ctx = new AgentContext();
				AgentModel res = ctx.GetByEmailAdd(emailadd);
				if (res == null)   // a null return means no object found
				{
					// return a reponse conforming to REST conventions: a 404 error
					return ErrorBuilder.ErrorResponse(this.Request.Url.ToString(), "GET", HttpStatusCode.NotFound, String.Format("Agent with email = {0} does not exist", emailadd));
				}
				else
				{
					// success. The Nancy server will automatically serialise this to JSON
					return res;
				}
			}
			// Please, please handle exceptions in a way that provides information about the context of the error.
			catch (Exception e)
			{
				return CommonModule.HandleException(e, String.Format("AgentModule.GetByEmailAdd({0})", emailadd), this.Request);
			}
		}
		
		// Get all data
		private object GetAll()
		{
			try
			{
				// create a connection to the PetaPoco orm and try to fetch and object with the given Id
				AgentContext ctx = new AgentContext();
				// Get all (or rather the first 999) objects
                IList<AgentModel> res = ctx.Get(999, 0, ""); // future development parameters are: top, from, filter
				// Nancy will convert this into an array of JSON objects.
                return res;
			}
			catch (Exception e)
			{
				return CommonModule.HandleException(e, String.Format("AgentModule.GetAll()"), this.Request);
			}
		}
		
		// GET /Agent/root/pass
		private object IsValidUser(AgentModel agent) 
		{
			if ( agent.EmailAddress == "root@ownhome.com" && agent.Password == "pass") 
			{
			    return "true";
			} 
			else 
			{
				return "false";		    	
			}
		}
		
		// POST /Agent
		Nancy.Response AddAgent() 
		{
			// debug code only
			// capture actual string posted in case the bind fails (as it will if the JSON is bad)
			// need to do it now as the bind operation will remove the data
			//String rawBody = this.GetBodyRaw(); 
			String rawBody = CommonModule.GetBodyRaw(this.Request);
			
			AgentModel profile = null;
			try
			{
				// bind the request body to the object via a Nancy module.
				profile = this.Bind<AgentModel>();

				// check exists. Return 409 if it does
				if ((profile.EmailAddress.Length == 0) && (profile.Password.Length == 0))
				{
					return ErrorBuilder.ErrorResponse(this.Request.Url.ToString(), "POST", HttpStatusCode.NotAcceptable, String.Format("Please update your email address-> {0}", profile.EmailAddress));
				}

				// Connect to the database
				AgentContext ctx = new AgentContext();
				ctx.Add(profile);
				
				// 201 - created
				Nancy.Response response = new Nancy.Responses.JsonResponse<AgentModel>(profile, new DefaultJsonSerializer());
				response.StatusCode = HttpStatusCode.Created;
				// uri
				string uri = this.Request.Url.SiteBase + this.Request.Path + "/" + profile.EmailAddress;
				response.Headers["Location"] = uri;

				return response;
			}
			catch (Exception e)
			{
				Console.WriteLine(rawBody);
				String operation = String.Format("AgentModule.AddAgent({0})", (profile == null) ? "No Model Data" : profile.EmailAddress);
				return CommonModule.HandleException(e, operation, this.Request);
			}	
			
		}
		
		// DELETE /Agent/99
		Nancy.Response DeleteAgent(int id)
		{
			try
			{
				AgentContext ctx = new AgentContext();
				AgentModel res = ctx.GetById(id);

				if (res == null)
				{
					return ErrorBuilder.ErrorResponse(this.Request.Url.ToString(), "DELETE", HttpStatusCode.NotFound, String.Format("Agent with Id = {0} does not exist", id));
				}
				AgentModel ci = new AgentModel();
				ci.AgentId = id;
				ctx.delete(ci);
				return 204;
			}
			catch (Exception e)
			{
				return CommonModule.HandleException(e, String.Format("\nAgentModule.Delete({0})", id), this.Request);
			}
		}
		
		
	} //end Class: Agent
}
