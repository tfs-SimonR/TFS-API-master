//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CashingUpDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_StoreDetails
    {
        public int RowId { get; set; }
        public string StoreId { get; set; }
        public Nullable<int> numberofTills { get; set; }
        public Nullable<int> numberofSafes { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public Nullable<bool> isSparq { get; set; }
        public Nullable<int> tillIdStart { get; set; }
        public Nullable<int> tillIdEnd { get; set; }
        public Nullable<decimal> floatAmount { get; set; }
    }
}
