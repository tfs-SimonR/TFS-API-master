using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TFS_API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AuditTransactionVM
    {
        /// <summary>
        /// This is for recording transactions for PI
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int StoreId { get; set; }
        public string SKU { get; set; }
        public DateTime EventDateTime { get; set; }
        public int StockAdjustmentLevel { get; set; } //This is a positive or negative value
        public decimal RetailValue { get; set; }
        public decimal CostValue { get; set; }
        public int PriorReasonCode { get; set; }
        public string PriorStatusCode { get; set; }
        public int EventType { get; set; }

        public string Area { get; set; }

    }
}