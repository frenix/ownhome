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
using System.Linq;
using System.Text;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace OHWebService
{
	class Program
	{
		// Our url.
		const string DOMAIN = "http://localhost:8088";
		
		public static void Main(string[] args)
		{
			var nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri(DOMAIN));
			
			//start
			nancyHost.Start();
			
			Console.WriteLine("ProperyFinder service listening on " + DOMAIN);
			// stop with an <Enter key press>
			Console.ReadLine();
			nancyHost.Stop();
		}
	}
	
	// utterly basic configuration of the Nancy server. Other configuration not researched.
	public class Bootstrapper : Nancy.DefaultNancyBootstrapper
	{
		protected virtual Nancy.Bootstrapper.NancyInternalConfiguration InternalConfiguration
		{
			get
			{
				return Nancy.Bootstrapper.NancyInternalConfiguration.Default;
			}
		}
		
		protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
			    {
			
			       //CORS Enable
			        pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) =>
			        {
			            ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
			                            .WithHeader("Access-Control-Allow-Methods", "POST,GET")
			                            .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");
			
			        });
				}
	}
		
}