using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.BindingModels
{
    public class CashManagmentModel
    {
        public int RowId { get; set; }
        public string StoreId { get; set; }
        public DateTime TradingDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool isClosed { get; set; }
        public bool isSparq { get; set; }
        public bool? isLocked { get; set; }
    }
}