using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.TestDTO
{
    public class WarehouseResetTESTDTO
    {
        public List<Pallets> pallet_list { get; set; }

        public class Pallets
        {
            public string palletbarcode { get; set; }
        }

    }
}