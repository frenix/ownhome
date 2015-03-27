/*
 * Created by SharpDevelop.
 * User: Frenix
 * Date: 3/26/2015
 * Time: 9:47 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace OHWebService.Modules
{
	/// <summary>
	/// Description of GuidCreator.
	/// </summary>
	public class GuidCreator
	{
		public GuidCreator()
		{
		}
		
		public static Guid Value(string input)
		{
			Guid uuid;
			
			uuid = Guid.NewGuid();
			return uuid;
		}
	}
}
