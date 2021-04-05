using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class SKUBranchDTO
    {
        public string guid { get; set; }
        public string storeId { get; set; }
        public string postcode { get; set; }
        public long lat { get; set; }
        public long lon { get; set; }
        public string latlng { get; set; }

    }
}