//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AuditDataAccessLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_SupplierToStore_Audit
    {
        public int RowId { get; set; }
        public Nullable<System.DateTime> DateCounted { get; set; }
        public string UserId { get; set; }
        public string SKU { get; set; }
        public string StoreId { get; set; }
        public Nullable<int> VarInt { get; set; }
        public Nullable<decimal> StockLevelDifference { get; set; }
        public Nullable<decimal> PriceValue { get; set; }
        public Nullable<decimal> CostValue { get; set; }
        public Nullable<System.DateTime> EventDate { get; set; }
        public string ponumber { get; set; }
    }
}
