using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using NLog;

namespace TFS_API.Models.BindingModels
{
    public class StoreToStoreBM
    {
        public string gdo_number { get; set; }

        public List<ShipmentItems> shipment_items { get; set; }
       

        public class ShipmentItems
        {
            public string store_origin { get; set; }
            public string store_destination { get; set; }
            public string description { get; set; }

            public string sku { get; set; }
            public int sku_qty { get; set; }

            public int varint { get; set; }

            public bool priority_pick { get; set; }

        }

    }
}