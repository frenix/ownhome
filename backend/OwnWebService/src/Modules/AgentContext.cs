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
using System.Data.Common;
using System.IO;
using OHWebService.Models;

namespace OHWebService.Modules
{

    /// <summary>
    /// Class to handle persisting Agent details to db
    /// </summary>
    public class AgentContext
    {
        public AgentContext()
        {
        }
        internal IList<AgentModel> Get(int top, int from, string filter)
		{
			// TODO: acknowledge parameter values.
			String sql = "select * from property_agent order by AgentId";
			return AgentContext.GetDatabase().Query<AgentModel>(sql).ToList();
		}
		
		public AgentModel GetById(int  id)
		{
		    String sql = "select * from property_agent where AgentId =" + id.ToString();
			return AgentContext.GetDatabase().FirstOrDefault<AgentModel>(sql);
		}

		public void Add(AgentModel agent)
		{
			AgentContext.GetDatabase().Insert(agent);
		}

		internal void update(AgentModel agent)
		{
			AgentContext.GetDatabase().Update(agent);
		}
		internal void delete(AgentModel agent)
		{
			AgentContext.GetDatabase().Delete(agent);
		}

		private static PetaPoco.Database GetDatabase()
		{
			// A sqlite database is just a file.
			String fileName = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "OHDB.db");
			String connectionString = "Data Source=" + fileName;
			DbProviderFactory sqlFactory = new System.Data.SQLite.SQLiteFactory();
			PetaPoco.Database db = new PetaPoco.Database(connectionString, sqlFactory);
			return db;
		}

        
    } //end of AgendContext
}
