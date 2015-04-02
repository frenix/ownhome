/*
 * Created by Fuego, Inc. 
 * File  :   PropertyContext.cs
 * Author:    Efren Duran
 * Date: 3/20/2015
 * Time: 9:26 AM
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
    /// Class to handle manipulating Property details to db
    /// </summary>
    public class PropertyContext
    {
        public PropertyContext()
        {
        }
        
        internal IList<PropertyModel> Get(int top, int from, string filter)
		{
			// TODO: acknowledge parameter values.
			String sql = "select * from listing where SoldFlag = 0 order by ListingId ";
			return CommonModule.GetDatabase().Query<PropertyModel>(sql).ToList();
		}
        
        public void Add(PropertyModel listing)
		{
			CommonModule.GetDatabase().Insert(listing);
		}
        
        public PropertyModel GetById(int id)
		{
		    String sql = "select * from listing where ListingId =" + id.ToString();
			return CommonModule.GetDatabase().FirstOrDefault<PropertyModel>(sql);
		}
        
        internal void update(PropertyModel listing)
		{
			CommonModule.GetDatabase().Update(listing);
		}
        
        internal void delete(PropertyModel listing)
		{
			CommonModule.GetDatabase().Delete(listing);
		}
        
    }
}
