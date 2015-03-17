/*
 * Created by SharpDevelop.
 * User: durane
 * Date: 3/13/2015
 * Time: 9:29 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Nancy;
using Nancy.Responses;

namespace OHWebService.Modules
{
	/// <summary>
	/// Description of ErrorBuilder.
	/// </summary>
	public class ErrorBuilder
	{
		public static Nancy.Response ErrorResponse(string url, string verb, HttpStatusCode code, string errorMessage)
		{
			ErrorBody e = new ErrorBody
			{
				Url = url, 
				Operation = verb,
				Message = errorMessage
			};
			// Build and return an object that the Nancy server knows about.
			Nancy.Response response = new Nancy.Responses.JsonResponse<ErrorBody>(e, new DefaultJsonSerializer());
			response.StatusCode = code;
			return response;
		}

	}
		// useful info to return in an error
    public class ErrorBody
    {
        public string Url {get; set; }
        public string Operation { get; set; }
        public string Message { get; set; }
    }
}
