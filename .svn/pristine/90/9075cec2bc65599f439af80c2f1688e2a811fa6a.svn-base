using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class StoreWebPanelDTO
    {
        public string current_store { get; set; }
        public string description { get; set; }
        public decimal? stock_qty { get; set; }
        public int whs_stock_qty { get; set; }
      
        public string barcode { get; set; }
        public decimal in_transit_stock_qty { get; set; }
        public decimal? current_lat { get; set; }
        public decimal? current_long { get; set; }
       
        public List<StoresList> list_stores { get; set; }
        public class StoresList
        {
            public string store_id { get; set; }
            public string store_name { get; set; }
            public string store_address { get; set; }
            public double lat { get; set; }
            public double longi { get; set; }
            public decimal? stock_qty { get; set; }
            public double distance_to_store { get; set; }
            public string url { get; set; }
        }
 
    }
}