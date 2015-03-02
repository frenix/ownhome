/*
 * Created by SharpDevelop.
 * User: durane
 * Date: 2/20/2015
 * Time: 9:03 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace OHWebService
{
	/// <summary>
	/// Description of PropertyModule.
	/// </summary>
	public class PropertyModule : Nancy.NancyModule
	{
		const String PropertyPage = @"
<html><body>
<h1>Properties Page </h1>
</body></html>
";
		public PropertyModule() : base ("/Properties")
		{
				// http://localhost:<port>/Properties
			Get["/"] = parameter => { return PropertyPage; };
		}
	}
}
