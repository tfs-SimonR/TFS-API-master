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
    
    public partial class tbl_cancel_allocations_audit
    {
        public int rowid { get; set; }
        public string userid { get; set; }
        public string sku { get; set; }
        public Nullable<System.DateTime> dateupdated { get; set; }
        public string storedprocedurecalled { get; set; }
    }
}