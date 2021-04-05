using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.ViewModels
{
    public class StockLocationVM
    {
        public StockLocationVM()
        {
            StockAtStores = new List<AbstractSubStockLocationVM>();

        }

        public string productId { get; set; }
        public List<AbstractSubStockLocationVM> StockAtStores { get; set; }

        public class AbstractSubStockLocationVM
        {
            public string store_id { get; set; }
            public string store_name { get; set; }

            public string description { get;set; }
            public double lat { get; set; }
            public double longi { get; set; }
            public decimal? stock_qty { get; set; }
            public decimal? stock_in_transit_to_remote_store { get; set; }

        }
        
    }
}