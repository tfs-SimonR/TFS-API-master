using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class PICountVMDTO
    {
        /// <summary>
        /// ID of the List
        /// </summary>
        public int list_id { get; set; }
        /// <summary>
        /// Count List name
        /// </summary>
        public string list_name { get; set; }
        /// <summary>
        /// Date of performed Scan
        /// </summary>
        public DateTime expected_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string user_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string store_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reason_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<PICountItems> counted_items { get; set; }

        public class PICountItems
        {
            /// <summary>
            /// SKU
            /// </summary>
            public string sku { get; set; }

            public int sku_counted_qty { get; set; }

            /// <summary>
            /// SKU Expected Count
            /// </summary>
            public int sku_expected_qty { get; set; }

            public DateTime scan_date { get; set; }

        }
    }
}