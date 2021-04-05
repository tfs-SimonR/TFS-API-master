using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.BindingModels
{
    public class StockStoreBM
    {
        /// <summary>
        /// Stock Quantity at Store
        /// </summary>
        public decimal? stock_qty { get; set; }
        /// <summary>
        /// TOFS Branch ID
        /// </summary>
        public string branch_id { get; set; }

        public string sku { get; set; }

        public string description { get; set; }

        public string pallet_identifier { get; set; }
    }
}