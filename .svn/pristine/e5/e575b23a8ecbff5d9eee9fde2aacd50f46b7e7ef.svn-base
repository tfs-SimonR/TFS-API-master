using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class AllStockEnRouteDTO
    {

        public List<PalletItems> stock_items { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public class PalletItems
        {
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
            public decimal sellingprice { get; set; }
            /// <summary>
            /// Stock Quantity at Store
            /// </summary>
            public decimal qty_onway { get; set; }

        }
        
    }
}