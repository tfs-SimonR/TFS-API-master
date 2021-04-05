using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class ProductStockDTO
    {
        public string storeId { get; set; }
        public string Key { get; set; }
        public string storeName { get; set; }
        public string variantcode { get; set; }
        public long qty { get; set; }
    }
}