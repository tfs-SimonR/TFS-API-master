using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.Tables
{
    public class AdHocDTO
    {

        /// <summary>
        /// User ID
        /// </summary>
        public string user_id { get; set; }
        /// <summary>
        /// Stored Id
        /// </summary>
        public string store_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reason_code { get; set; }
        
        public List<AdHocList> counted_items { get; set; }

        public class AdHocList
        {
            /// <summary>
            /// SKU
            /// </summary>
            public string sku { get; set; }
            /// <summary>
            ///  Sku counted quantiuty
            /// </summary>
            public int sku_counted_qty { get; set; }
          
            /// <summary>
            /// Date of performed Scan
            /// </summary>
            public DateTime scan_date { get; set; }

        }
      
    }
}