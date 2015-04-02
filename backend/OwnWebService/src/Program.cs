/*
 * Created by SharpDevelop.
 * User: durane
 * Date: 2/17/2015
 * Time: 3:26 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 * Note: Special thanks from resource in http://www.codeproject.com/Articles/680119/Creating-a-REST-Server-for-a-CRUD-Web-Application
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.Owin;

using Owin;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Cors;

using OHWebService.Authentication;

namespace OHWebService
{
    using AppFunc =  Func<IDictionary<string, object>, Task>;
    class Program
    {
        static void Main(string[] args)
		{
			 var url = "http://+:8088";

                using (WebApp.Start<Startup>(url))
                {
                    Console.WriteLine("Running on {0}", url);
                    Console.WriteLine("Press enter to exit");
                    Console.ReadLine();
                }
		}
        
        //Startup
        public class Startup
        {
      
            public void Configuration(IAppBuilder app)
            {
               
//               .UseNancy(options => options.PassThroughWhenStatusCodesAre(
//				              HttpStatusCode.NotFound,
//				              HttpStatusCode.InternalServerError))
				app		
                    .UseCors(CorsOptions.AllowAll)				    
					.Use(typeof(JwtOwinAuth))
				    .UseNancy();
					
            }
        }
    }
}