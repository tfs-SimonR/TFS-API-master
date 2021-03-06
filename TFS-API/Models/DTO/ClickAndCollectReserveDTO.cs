using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class ClickAndCollectReserveDTO
    {
        public string storeid { get; set; }
        public string guid { get; set; }
        public string sku_variant { get;set; }
        public string name { get; set; }
        public string postcode { get; set; }
        public int quantity { get; set; }
        public string email { get; set; }
        public string tofscardnumber { get; set; }
        public string mobilenumber { get; set; }
        public bool marketing_consent { get; set; }
        public DateTime marketingconsentdate { get; set; }
        public DateTime tandcconsentdate { get; set; }

    }
}