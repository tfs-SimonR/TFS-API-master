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
    
    public partial class varcode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public varcode()
        {
            this.productcodes = new HashSet<productcode>();
        }
    
        public string vartype { get; set; }
        public string varcode1 { get; set; }
        public string description { get; set; }
        public string vargroup { get; set; }
        public int varcodeint { get; set; }
        public Nullable<int> Sequence { get; set; }
        public string Code1 { get; set; }
        public string Code2 { get; set; }
        public Nullable<int> NRFCode { get; set; }
        public string ColourARGB { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<productcode> productcodes { get; set; }
    }
}