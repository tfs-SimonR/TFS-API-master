using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mi9TESTDAL;

namespace TFS_API.Models.BindingModels
{
    public class GoodInWarehouseBM
    {
     
        /// <summary>
        /// Pallet Number
        /// </summary>
        public string pallet_identifier { get; set; }

        

        public List<ShipmentItems> shipment_items { get; set; }
        public class ShipmentItems
        {
            /// <summary>
            /// SKU Code
            /// </summary>
            public string sku { get; set; }

            /// <summary>
            /// Products on Pallet
            /// </summary>
            public decimal stock_qty { get; set; }
            /// <summary>
            /// Description of Item
            /// </summary>
            public string description { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int varint { get; set; }

            public bool? priority_pick { get; set; }

        }
       
    }
}