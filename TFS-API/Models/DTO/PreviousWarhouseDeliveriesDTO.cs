using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class PreviousWarhouseDeliveriesDTO
    {
        /// <summary>
        /// Pallet Number
        /// </summary>
        public string pallet_identifier { get; set; }
        /// <summary>
        /// destination
        /// </summary>
        public string destination { get; set; }

        public DateTime? booked_in_date { get; set; }

        public List<WarehouseItems> shipment_items { get; set; }

        public class WarehouseItems
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

            public int varint { get; set; }

            public bool priority_pick { get; set; }

        }
    }
}