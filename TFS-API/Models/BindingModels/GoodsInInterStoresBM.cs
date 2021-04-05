using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.BindingModels
{
    public class GoodsInInterStoresBM
    {
        public string delivery_number { get; set; }
        
        public List<ShipmentItems> shipment_items { get; set; }
        public DateTime? expected_date { get; set; }

        public class ShipmentItems
        {
            public string sku { get; set; }

            public decimal sku_qty { get; set; }
            public int varint { get; set; }

            public bool priority_pick { get; set; }
        }
    }
}