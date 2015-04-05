/*
 * Created by Fuego, Inc. 
 * File  :   AgentContext.cs
 * Author:    Efren Duran
 * Date: 3/17/2015
 * Time: 5:11 PM
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OHWebService.Models;

namespace OHWebService.Modules
{

    /// <summary>
    /// Class to handle manipulating Agent details to db
    /// </summary>
    public class AgentContext
    {
       // CommonModule cmn = new CommonModule();
        
        public AgentContext()
        {
        }
        
        internal IList<AgentModel> Get(int top, int from, string filter)
		{
			// TODO: acknowledge parameter values.
			String sql = "select * from property_agent order by AgentId";
			return CommonModule.GetDatabase().Query<AgentModel>(sql).ToList();
		}
		
        //maybe not use
        public AgentModel GetById(int id)
		{
        	String sql = "select * from property_agent where AgentId =" + id.ToString();
			return CommonModule.GetDatabase().FirstOrDefault<AgentModel>(sql);
		}
        
		public AgentModel GetByToken(string token)
		{
		    String sql = "select * from property_agent where AuthKey = @0";
			return CommonModule.GetDatabase().FirstOrDefault<AgentModel>(sql, token);
		}
        
		public AgentModel GetByEmailAddAndPwd(string  emailadd, string pswd)
		{
		    //String sql = "select * from property_agent where EmailAddress ='" + emailadd + "' and Password='" + pswd +"'";
		    String sql = "select * from property_agent where EmailAddress = @0 and Password=@1";
		    
		    return CommonModule.GetDatabase().FirstOrDefault<AgentModel>(sql,emailadd,pswd);
		}
		
		public void Add(AgentModel agent)
		{
			CommonModule.GetDatabase().Insert(agent);
		}

		internal void update(AgentModel agent)
		{
			CommonModule.GetDatabase().Update(agent);
		}
		internal void delete(AgentModel agent)
		{
			CommonModule.GetDatabase().Delete(agent);
		}
        
    } //end of AgentContext
}
