using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.ViewModels
{
    public class AcceptWarehouseVM
    {
        public string palletbarcode { get; set; }
        public string storeId { get; set; }
        public string user_name { get; set; }
    }
}