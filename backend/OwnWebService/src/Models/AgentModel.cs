/*
 * Created by Fuego, Inc. 
 * File  :   AgentModel.cs
 * Author:    Efren Duran
 * Date: 3/17/2015
 * Time: 12:50 PM
 * 
 */
using System;

namespace OHWebService.Models
{
    /// <summary>
    /// Description of AgentModel.
    /// </summary>
    public class AgentModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAdd { get; set; }
		public string Password { get; set; }
		public string AuthKey { get; set; }
    }
}
