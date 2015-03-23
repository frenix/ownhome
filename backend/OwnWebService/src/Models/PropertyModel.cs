/*
 * Created by Fuego, Inc. 
 * File  :   ${FILE}
 * Author:    Efren Duran
 * Date: 3/20/2015
 * Time: 9:26 AM
 * 
 */
using System;

namespace OHWebService.Models
{
    /// <summary>
    /// Class that defines Property and map it to db
    /// </summary>

    [PetaPoco.TableName("listing")]
	[PetaPoco.PrimaryKey("ListingId")]
    public partial class PropertyModel
    {
        [PetaPoco.Column("ListingId")]
		public Int64 ListingId { get; set; }
		[PetaPoco.Column("Title")]
		public String Title { get; set; }
		[PetaPoco.Column("HouseType")]
		public String HouseType { get; set; }
		[PetaPoco.Column("PriceInThousand")]
		public Decimal PriceInThousand { get; set; }
		[PetaPoco.Column("Description")]
		public String Description { get; set; }
		[PetaPoco.Column("Address")]
		public String Address { get; set; }
		[PetaPoco.Column("Latitude")]
		public String Latitude { get; set; }
		[PetaPoco.Column("Longitude")]
		public String Longitude { get; set; }
		[PetaPoco.Column("SoldFlag")]
		public Int32 SoldFlag { get; set; }
		[PetaPoco.Column("AgentId")]
		public Int64 AgentId { get; set; }  
    }
}
