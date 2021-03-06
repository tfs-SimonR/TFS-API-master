using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class ClickAndCollectPanelDataDTO
    {
        public bool isAvailable { get; set; }
        public string sku { get; set; }
        public string default_variant_sku { get; set; }
        public decimal current_price { get; set; }
        public decimal was_price { get; set; }
        public decimal retail_price { get; set; }
        public decimal saving { get; set; }
        public string product_name { get; set; }

        public List<PanelData> variant_list { get; set; }
        public class PanelData
        {
            public bool isAvailable { get; set; }
            public string sku_variant { get; set; }
            public string product_name { get; set; }
            public string size { get; set; }
            public string colour { get; set; }
            public decimal current_price { get; set; }
            public decimal was_price { get; set; }
            public decimal retail_price { get; set; }
            public decimal saving { get; set; }

        }

    }

    
}