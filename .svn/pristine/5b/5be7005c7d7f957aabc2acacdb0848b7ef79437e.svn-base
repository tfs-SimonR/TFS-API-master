using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.PostViewModels
{
    public class StoreToStoreDTO
    {
        /// <summary>
        /// GDO Number (IST Number)
        /// </summary>
        public string gdo_number { get; set; }
        /// <summary>
        /// Stored ID as a string
        /// </summary>
        public string store_id { get; set; }
        /// <summary>
        /// username of the user logged into the scanner
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// Collection of the ShipmentItems Data Transfer Object
        /// </summary>
        public List<ShipmentItemsDTO> shipment_items { get; set; }

        public class ShipmentItemsDTO
        {
            /// <summary>
            /// Item SKU
            /// </summary>
            public string sku { get; set; }

            /// <summary>
            /// SKU Counted QTY by scanner
            /// </summary>
            public int sku_counted_qty { get; set; }
            /// <summary>
            /// The SKU QTY that was expected on the delivery
            /// </summary>
            public int sku_expected_qty { get; set; }

        }
        
    }
}