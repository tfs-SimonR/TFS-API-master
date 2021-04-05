using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class ProductOptionsDTO
    {
        public bool isAvailable { get; set; }
        public string sku { get; set; }
        public string default_variant_sku { get; set; }
        public decimal current_price { get; set; }
        public decimal was_price { get; set; }
        public decimal retail_price { get; set; }
        public decimal saving { get; set; }
        public Options product_options { get; set; }
        public class Options
        {
            public List<colors> product_colours { get; set; }
            public List<sizes> product_sizes { get; set; }
        }
        
        public class colors
        {
            public int colourId { get; set; }
            public string colourText { get; set; }

        }
        public class sizes
        {
            public int sizeId { get; set; }
            public string sizeText { get; set; }
        }
    }
}