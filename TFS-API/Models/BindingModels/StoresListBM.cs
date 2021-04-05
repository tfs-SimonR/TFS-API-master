using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.BindingModels
{
    public class StoresListBM
    {
        public string store_id { get; set; }
        public string store_name { get; set; }
        public decimal? lat { get; set; }
        public decimal? longitude {get; set; }
    }
}