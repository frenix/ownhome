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
	    	    
		public AgentModule() : base("/agents")
		{
		    // /agent
		    Get["/"] = parameter => { return GetAll(); };
				
			// /agent/99
			Get["/{id}"] = parameter => { return GetById(parameter.id); };			
							
			// /agent       POST: Agent JSON in body
			Post["/"] = parameter => { return this.AddAgent(); };
			
			// /agent        DELETE: {AgentId}
			Delete["/{id}"] = parameter => { return this.DeleteAgent(parameter.id); };
			
			// /agent/		PUT: Token JSON in body
			Put["/"] = parameter => { return this.UpdateAgentByToken(); };
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
					return ErrorBuilder.ErrorResponse(this.Request.Url.ToString(), "GET", HttpStatusCode.NotFound, "NG", String.Format("Agent with Id = {0} does not exist", id));
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
				return CommonModule.HandleException(e, String.Format("AgentModule.GetById({0})", id), "NG", this.Request);
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
				return CommonModule.HandleException(e, String.Format("AgentModule.GetAll()"), "NG", this.Request);
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
			
			//setup GUID for this user
			Guid uuid = GuidCreator.New();
			string fullName;
			
			AgentModel profile = null;
			try
			{
				// bind the request body to the object via a Nancy module.
				profile = this.Bind<AgentModel>();

				// check exists. Return 409 if it does
				if ((profile.EmailAddress.Length == 0) && (profile.Password.Length == 0))
				{
					return ErrorBuilder.ErrorResponse(this.Request.Url.ToString(), "POST", HttpStatusCode.NotAcceptable, "NG", String.Format("Please update your email address-> {0}", profile.EmailAddress));
				}
				
				//update AuthKey with GUID
				profile.AuthKey = uuid.ToString();
				//create fullname of the agent
				fullName = profile.FirstName + " " + profile.LastName;
				// Connect to the database
				AgentContext ctx = new AgentContext();
				ctx.Add(profile);
				
				// 201 - created
				Nancy.Response response = new Nancy.Responses.JsonResponse<AgentModel>(profile, new DefaultJsonSerializer());
				response.StatusCode = HttpStatusCode.Created;
				// uri
				string uri = this.Request.Url.SiteBase + this.Request.Path + "/" + profile.EmailAddress;
				response.Headers["Location"] = uri;
				
				
				//send email for confirmation
				// this is to update confirmedFlag in db
				SendMail.Send(fullName , profile.EmailAddress, uuid.ToString());
				return response;
			}
			catch (Exception e)
			{
				Console.WriteLine(rawBody);
				String operation = String.Format("AgentModule.AddAgent({0})", (profile == null) ? "No Model Data" : profile.EmailAddress);
				return CommonModule.HandleException(e, operation, "NG", this.Request);
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
					return ErrorBuilder.ErrorResponse(this.Request.Url.ToString(), "DELETE", HttpStatusCode.NotFound, "NG", String.Format("Agent with Id = {0} does not exist", id));
				}
				AgentModel ci = new AgentModel();
				ci.AgentId = id;
				ctx.delete(ci);
				return 204;
			}
			catch (Exception e)
			{
				return CommonModule.HandleException(e, String.Format("\nAgentModule.Delete({0})", id), "NG", this.Request);
			}
		}
		
		//PUT /agents/token
		// used to update ConfirmFlg when user confirms email 
		Nancy.Response UpdateAgentByToken()
		{
			// debug code only
			// capture actual string posted in case the bind fails (as it will if the JSON is bad)
			// need to do it now as the bind operation will remove the data
			//String rawBody = this.GetBodyRaw(); 
			String rawBody = CommonModule.GetBodyRaw(this.Request);
			
			AgentModelToken agentauthkey = null;
			try
			{
				// bind the request body to the object
				agentauthkey= this.Bind<AgentModelToken>();

				AgentContext ctx = new AgentContext();

				AgentModel agent = ctx.GetByToken(agentauthkey.token);
				
				if (agent == null)
				{
					return 404;
				}
				// if agent is found by authkey, update it
				agent.ConfirmFlg = "1"; //set flag to 1
				ctx.update(agent);
				return 204; // no content response
			}
			catch (Exception e)
			{
				String operation = String.Format("AgentModule.UpdateAgentByToken({0})", (agentauthkey == null) ? "No Model Data" : agentauthkey.token);
				return CommonModule.HandleException(e, operation,"NG",this.Request);
			}
		}
		
		
	} //end Class: Agent
}
