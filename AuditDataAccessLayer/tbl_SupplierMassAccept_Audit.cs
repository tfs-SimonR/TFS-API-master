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
    
    public partial class tbl_SupplierMassAccept_Audit
    {
        public int RowId { get; set; }
        public Nullable<System.DateTime> DateAccepted { get; set; }
        public string userId { get; set; }
        public string storeid { get; set; }
        public Nullable<int> varint { get; set; }
        public Nullable<System.DateTime> EventDate { get; set; }
        public string ponumber { get; set; }
        public string reason { get; set; }
    }
}
