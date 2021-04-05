using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class StoreDistanceDTO
    {

        public string storename { get; set; }
        /// <summary>
        /// Stock Item Description
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// Product SKU Code
        /// </summary>
        public string sku { get; set; }
        /// <summary>
        /// Product Bar Code
        /// </summary>
        public string barcode { get; set; }

        /// <summary>
        /// RRP For product
        /// </summary>
        public decimal? sellingprice { get; set; }
        /// <summary>
        /// Stock Quantity at Store
        /// </summary>
        public decimal? stock_qty { get; set; }
        /// <summary>
        /// Stock enroute to store
        /// </summary>
        public int? stk_enroute { get; set; }
      
    }
}