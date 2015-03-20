/*
 * Created by SharpDevelop.
 * User: durane
 * Date: 2/20/2015
 * Time: 9:03 AM
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
	/// The root for this service is http://<ip></ip>:<port></port>/Properties
	/// </summary>
	public class PropertyModule : Nancy.NancyModule
	{
	    //temp string to return
		const String PropertyPage = @"
                        <html><body>
                        <h1>Properties Page </h1>
                        </body></html>
                        ";
		
		CommonModule cmn = new CommonModule();
		public PropertyModule() : base ("/Properties")
		{
				// /Properties          GET: Get All Available Properties (public) 
			Get["/"] = parameter => { return GetAll(); };
		}
		
		// -- IMPLEMENTATION PART --
		
		// Get all data
		private object GetAll()
		{
			try
			{
				// create a connection to the PetaPoco orm and try to fetch and object with the given Id
				PropertyContext ctx = new PropertyContext();
				// Get all (or rather the first 999) objects
                IList<PropertyModel> res = ctx.Get(999, 0, ""); // future development parameters are: top, from, filter
				// Nancy will convert this into an array of JSON objects.
                return res;
			}
			catch (Exception e)
			{
				return CommonModule.HandleException(e, String.Format("PropertyModule.GetAll()"), this.Request);
			}
		}
		
	} //end class PropertyModule
}
