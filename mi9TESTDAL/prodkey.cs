//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mi9TESTDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class prodkey
    {
        public string prodaltkey { get; set; }
        public int prodint { get; set; }
        public string keytype { get; set; }
        public bool Primary { get; set; }
        public Nullable<int> ProductUnitID { get; set; }
        public bool PrimaryForUnit { get; set; }
        public string prodaltkeyNoZeros { get; set; }
    
        public virtual productcode productcode { get; set; }
    }
}
