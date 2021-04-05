using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class GETPanelDTO
    {
        

        public List<productcodeList> productcodes { get; set; }

        public int qty { get; set; }

        public class productcodeList
        {
            public string productcode { get; set; }
            public bool isAvailable { get; set; }

            public string product_sizes { get; set; }

        }
    }
}