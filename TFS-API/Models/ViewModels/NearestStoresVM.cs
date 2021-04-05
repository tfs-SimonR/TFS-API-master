using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mi9DataAccessLayer;

namespace TFS_API.Models.ViewModels
{
    public class NearestStoresVM
    {

        public string current_store { get; set; }
        public string description { get; set; }
        public decimal? stock_qty { get; set; }
        public int whs_stock_qty { get; set; }
        public decimal retail_price { get; set; }
        public string barcode { get; set; }
        public decimal in_transit_stock_qty { get; set; }
        public decimal? current_lat { get; set; }
        public decimal? current_long { get; set; }
        public int? seven_day { get; set; }
        public int? thirty_day { get; set; }
        public bool top_two_hundred { get; set; }
        public List<ListOfStores> list_stores { get; set; }
        public List<AreaStores> area_stores { get; set; }

        

        public class ListOfStores
        {
            public string store_id { get; set; }
            public string store_name { get; set; }
            public double lat { get; set; }
            public double longi { get; set; }
            public decimal? stock_qty { get; set; }
            public decimal? stock_in_transit_to_remote_store { get; set; }
            public double distance_to_store { get; set; }

        }

        public class AreaStores
        {
            public string store_id { get; set; }
            public string store_name { get; set; }
            public double lat { get; set; }
            public double longi { get; set; }
            public decimal? stock_qty { get; set; }
            public decimal? stock_in_transit_to_remote_store { get; set; }

        }
    }
}