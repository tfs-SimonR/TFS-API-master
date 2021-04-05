using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class CreateSupReturnDTO
    {
        /// <summary>
        /// GDO Number
        /// </summary>
        public string gdo_number { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        public string user_name { get; set; }
        /// <summary>
        /// Store Id
        /// </summary>
        public string store_id { get; set; }
        /// <summary>
        /// Destination store Id for the shipment
        /// </summary>
        public string supplier_id { get; set; }
        /// <summary>
        /// PONumber of return item
        /// </summary>

        public string ponumber { get; set; }
        public int reason_id { get; set; }

        public List<SupplierShipmentItems> return_items { get; set; }

       

        public class SupplierShipmentItems
        {
            /// <summary>
            /// sku counted qty
            /// </summary>
            public string sku_counted_qty { get; set; }

            /// <summary>
            /// sku
            /// </summary>
            public string sku { get; set; }
        }
    }
}