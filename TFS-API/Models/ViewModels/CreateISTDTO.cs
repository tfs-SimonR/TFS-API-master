using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.ViewModels
{
    public class CreateISTDTO
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
        public string destination_store_id { get; set; }

        public List<ISTShipmentItems> shipment_items { get; set; }

        public int reason_id { get; set; }

        public class ISTShipmentItems
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