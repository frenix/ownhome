/*
 * Created by SharpDevelop.
 * User: durane
 * Date: 2/18/2015
 * Time: 11:36 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace OHWebService
{
	/// <summary>
	/// The root for this service is http://localhost:<port></port>/Users
	/// </summary>
	public class UserModule : Nancy.NancyModule
	{
				const String UserPage = @"
<html><body>
<h1>User Page </h1>
</body></html>
";
		public UserModule() : base("/Users")
		{
			// http://localhost:<port>/Users
			Get["/"] = parameter => { return UserPage; };

//			// http://localhost:8000/Badges/99
//			Get["/{id}"] = parameter => { return GetById(parameter.id); };
//
//			// http://localhost:8008/Badges       POST: Badge JSON in body
//			Post["/"] = parameter => { return this.AddBadge(); };
//
//			// http://localhost:8088/Badges/99    PUT: Badge JSON in body
//			Put["/{id}"] = parameter => { return this.UpdateBadge(parameter.id); };
//
//			// http://localhost:8088/Badges/99    DELETE:  
//			Delete["/{id}"] = parameter => { return this.DeleteBadge(parameter.id); };
		}
	}
}
