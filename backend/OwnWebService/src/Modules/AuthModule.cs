/*
 * Created by SharpDevelop.
 * User: Frenix
 * Date: 3/27/2015
 * Time: 8:44 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
//using System.Configuration;
using Nancy;
using Nancy.ModelBinding;
using JWT;
using OHWebService.Authentication;
using OHWebService.Models;

namespace OHWebService.Modules
{
	/// <summary>
	/// Description of AuthModule.
	/// </summary>
	public class AuthModule	: Nancy.NancyModule
	{
		private readonly string secretKey;
       // private readonly IUserService userService;
               
        
        public AuthModule ()  : base ("/login")
        {

            Post ["/"] = _ => LoginHandler(this.Bind<LoginRequest>());

            secretKey = System.Configuration.ConfigurationManager.AppSettings ["SecretKey"];
        }

        public dynamic LoginHandler(LoginRequest loginRequest)
        {
            if (IsValidUser (loginRequest.email, loginRequest.password)) {
				//{ "userId", 101 }
                var payload = new Dictionary<string, object> {
                    { "email", loginRequest.email },
                    { "userId", GuidCreator.New() }
                };

                var token = JsonWebToken.Encode (payload, secretKey, JwtHashAlgorithm.HS256);

                return new JwtToken { Token = token };
            } else {
                return HttpStatusCode.Unauthorized;
            }
        }
        
        private bool IsValidUser(string email, string pswd) 
		{
            //check expiry
            //https://github.com/jchannon/Owin.StatelessAuth/blob/master/src/Owin.StatelessAuthExample/MySecureTokenValidator.cs
            // create a connection to the PetaPoco orm and try to fetch and object with the given Id
			AgentContext ctx = new AgentContext();
			AgentModel res = ctx.GetByEmailAddAndPwd(email, pswd);
			// a null return means no object found
			if (res == null) 
			{
				return false;
			}
				
			return true;
		}
    }

	
    public class JwtToken
    {
        public string Token { get; set; }
    }

    public class LoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }
	
}
