using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class PreviousGoodsOutDTO
    {
        /// <summary>
        /// Pallet Number
        /// </summary>
        public string pallet_identifier { get; set; }
        /// <summary>
        /// destination
        /// </summary>
        public string destination { get; set; }

        public DateTime expected_date { get; set; }

        public List<GoodsOutPreviousItems> shipment_items { get; set; }
        public class GoodsOutPreviousItems
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