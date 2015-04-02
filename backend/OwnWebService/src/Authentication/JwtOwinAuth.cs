/*
 * Created by SharpDevelop.
 * User: Frenix
 * Date: 3/27/2015
 * Time: 8:59 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JWT;

namespace OHWebService.Authentication
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    /// <summary>
    /// OWIN add-in module for JWT authorization.
    /// referred to http://mikehadlow.blogspot.com/2014/04/json-web-tokens-owin-and-angularjs.html
    /// </summary>
    public class JwtOwinAuth
    {
        private readonly AppFunc next;
        private readonly string secretKey;
        private readonly HashSet<string> exceptions = new HashSet<string>{ 
            "/",
            "/login",
            "/login/"
        };

        public JwtOwinAuth (AppFunc next)
        {
            this.next = next;
            secretKey = System.Configuration.ConfigurationManager.AppSettings ["SecretKey"];
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            var path = environment ["owin.RequestPath"] as string;
            if (path == null) {
                throw new ApplicationException ("Invalid OWIN request. Expected owin.RequestPath, but not present.");
            }
            if (!exceptions.Contains(path)) {
                var headers = environment ["owin.RequestHeaders"] as IDictionary<string, string[]>;
                if (headers == null) {
                    throw new ApplicationException ("Invalid OWIN request. Expected owin.RequestHeaders to be an IDictionary<string, string[]>.");
                }
                if (headers.ContainsKey ("Authorization")) {
                    var token = GetTokenFromAuthorizationHeader (headers ["Authorization"]);
                    try {
                        var payload = JsonWebToken.DecodeToObject (token, secretKey) as Dictionary<string, object>;
                        //environment.Add("myapp.userId", (int)payload["userId"]);// => change from int to guid
                        environment.Add("myapp.userId", payload["userId"].ToString());
                        environment.Add("myapp.email", payload["email"].ToString());
                    } catch (SignatureVerificationException) {
                        return UnauthorizedResponse (environment);
                    }
                } else {
                    return UnauthorizedResponse (environment);
                }
            }
            return next (environment);
        }

        public string GetTokenFromAuthorizationHeader(string[] authorizationHeader)
        {
            if (authorizationHeader.Length == 0) {
                throw new ApplicationException ("Invalid authorization header. It must have at least one element");
            }
            var token = authorizationHeader [0].Split (' ') [1];
            return token;
        }

        public Task UnauthorizedResponse(IDictionary<string, object> environment)
        {
            environment ["owin.ResponseStatusCode"] = 401;
            return Task.FromResult (0);
        }
    }
}