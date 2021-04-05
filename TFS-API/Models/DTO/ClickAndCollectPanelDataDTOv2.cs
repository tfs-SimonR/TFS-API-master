using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class ClickAndCollectPanelDataDTOv2
    {
        
        public string variant_sku { get; set; }
        public decimal current_price { get; set; }
        public decimal was_price { get; set; }
        public decimal retail_price { get; set; }
        public decimal saving { get; set; }
    }
}