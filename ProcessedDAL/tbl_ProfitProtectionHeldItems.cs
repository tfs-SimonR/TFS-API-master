//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProcessedDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_ProfitProtectionHeldItems
    {
        public int RowId { get; set; }
        public System.DateTime DateWrittenOff { get; set; }
        public string userid { get; set; }
        public string storeid { get; set; }
        public string sku { get; set; }
        public int varint { get; set; }
        public int quantity { get; set; }
        public decimal costvalueperitem { get; set; }
        public decimal costtotal { get; set; }
        public System.DateTime eventdate { get; set; }
        public string reasoncode { get; set; }
        public Nullable<bool> isHeld { get; set; }
        public string uniquefilename { get; set; }
        public string storename { get; set; }
        public string description { get; set; }
        public Nullable<decimal> triggeramount { get; set; }
        public Nullable<decimal> retailpriceperitem { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public string DeletedBy { get; set; }
    }
}
