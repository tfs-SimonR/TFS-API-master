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
    
    public partial class StoresModel
    {
        public int RowId { get; set; }
        public string StoreId { get; set; }
        public string numberofTills { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public bool isDeleted { get; set; }
        public string DeletedBy { get; set; }
        public System.DateTime DateDeleted { get; set; }
    }
}
