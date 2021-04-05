using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class SKUBranchDTOv3
    {
        public string sku_variant { get; set; }
        public string storeId { get; set; }
        public string location { get; set; }
        public long lat { get; set; }
        public long lon { get; set; }
        public string latlng { get; set; }
    }
}