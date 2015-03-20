/*
 * Created by Fuego, Inc. 
 * File  :   ${FILE}
 * Author:    Efren Duran
 * Date: 3/20/2015
 * Time: 9:48 AM
 * 
 */
using System;
using System.Data.Common;
using System.IO;
using System.Configuration;
using Nancy;

namespace OHWebService.Modules
{
    /// <summary>
    /// Common functions 
    /// </summary>
    public class CommonModule
    {
        public CommonModule()
        {
        }
        
        		
		// Process errors
		public static Nancy.Response HandleException(Exception e, String operation, Nancy.Request req)
		{
			// we were trying this operation
			String errorContext = String.Format("{1}:{2}: {3} Exception caught in: {0}", operation, DateTime.UtcNow.ToShortDateString(), DateTime.UtcNow.ToShortTimeString(),e.GetType()); 
			// write detail to the server log. 
			Console.WriteLine("----------------------\n{0}\n{1}\n--------------------", errorContext, e.Message);
			if (e.InnerException != null)
				Console.WriteLine("{0}\n--------------------", e.InnerException.Message);
			// but don't be tempted to return detail to the caller as it is a breach of security.
			return ErrorBuilder.ErrorResponse(req.Url.ToString(), "GET", HttpStatusCode.InternalServerError, "Operational difficulties");
		}
		
		// Get Raw Data (JSON) from Nancy.Request
		public static String GetBodyRaw(Nancy.Request req)
		{
			// discover the body as a raw string
			byte[] b = new byte[req.Body.Length];
			req.Body.Read(b, 0, Convert.ToInt32(req.Body.Length));
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			String bodyData = encoding.GetString(b);
			return bodyData;
		}
		
		// TODO: check if this should be public static or just public
		public static PetaPoco.Database GetDatabase()
		{
		    string conStr = ConfigurationSettings.AppSettings["conStr"];
		    string dbName = ConfigurationSettings.AppSettings["dbName"];
		    
			// A sqlite database is just a file.
			String fileName = Path.Combine(conStr, dbName);
			String connectionString = "Data Source=" + fileName;
			DbProviderFactory sqlFactory = new System.Data.SQLite.SQLiteFactory();
			PetaPoco.Database db = new PetaPoco.Database(connectionString, sqlFactory);
			return db;
		}
    }
}
