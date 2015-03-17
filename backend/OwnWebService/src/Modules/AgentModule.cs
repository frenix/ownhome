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
	    
		public AgentModule() : base("/Agent")
		{
		    // /Agent
			Get["/"] = parameter => { return AgentPage; };
			
			// /Agent/99
			Get["/{Username}/{Password}"] = parameter => 
			{ 
				var profile = this.Bind<AgentModel>();
				
				return IsValidUser(profile);
			};
			
			// /Agent       POST: Agent JSON in body
			Post["/"] = parameter => { return this.AddAgent(); };
		}
		
		// GET /Agent/root/pass
		private object IsValidUser(AgentModel agent) 
		{
			if ( agent.UserName == "root" && agent.Password == "pass") 
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
			String rawBody = this.GetBodyRaw(); 

			AgentModel profile = null;
			try
			{
				// bind the request body to the object via a Nancy module.
				profile = this.Bind<AgentModel>();

				// check exists. Return 409 if it does
				if (!(profile.UserName == "root" && profile.Password == "rootpass"))
				{
					return ErrorBuilder.ErrorResponse(this.Request.Url.ToString(), "POST", HttpStatusCode.Conflict, String.Format("Use PUT to update an existing User with Id = {0}", profile.UserName));
				}

				//just return OK
				// 201 - created
				Nancy.Response response = new Nancy.Responses.JsonResponse<AgentModel>(profile, new DefaultJsonSerializer());
				response.StatusCode = HttpStatusCode.Created;
				// uri
				string uri = this.Request.Url.SiteBase + this.Request.Path + "/" + profile.UserName;
				response.Headers["Location"] = uri;

				return response;
			}
			catch (Exception e)
			{
				Console.WriteLine(rawBody);
				String operation = String.Format("BadgesModule.AddBadge({0})", (profile == null) ? "No Model Data" : profile.UserName);
				return HandleException(e, operation);
			}	
			
		}
		
		
		//TO DO: Check if this can be common
		
		Nancy.Response HandleException(Exception e, String operation)
		{
			// we were trying this operation
			String errorContext = String.Format("{1}:{2}: {3} Exception caught in: {0}", operation, DateTime.UtcNow.ToShortDateString(), DateTime.UtcNow.ToShortTimeString(),e.GetType()); 
			// write detail to the server log. 
			Console.WriteLine("----------------------\n{0}\n{1}\n--------------------", errorContext, e.Message);
			if (e.InnerException != null)
				Console.WriteLine("{0}\n--------------------", e.InnerException.Message);
			// but don't be tempted to return detail to the caller as it is a breach of security.
			return ErrorBuilder.ErrorResponse(this.Request.Url.ToString(), "GET", HttpStatusCode.InternalServerError, "Operational difficulties");
		}
		private String GetBodyRaw()
		{
			// discover the body as a raw string
			byte[] b = new byte[this.Request.Body.Length];
			this.Request.Body.Read(b, 0, Convert.ToInt32(this.Request.Body.Length));
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			String bodyData = encoding.GetString(b);
			return bodyData;
		}
		
		
		
	} //end Class: Agent
}
