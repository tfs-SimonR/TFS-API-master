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
    
    public partial class hs_processed_consighdrs
    {
        public int rowid { get; set; }
        public string consignment { get; set; }
        public Nullable<System.DateTime> date_bookedin { get; set; }
        public bool isBookedin { get; set; }
        public string storeid { get; set; }
    }
}
