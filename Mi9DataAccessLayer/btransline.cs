//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mi9DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class btransline
    {
        public int transint { get; set; }
        public int transline { get; set; }
        public int prodint { get; set; }
        public decimal quantity { get; set; }
        public decimal costvalue { get; set; }
        public decimal retailvalue { get; set; }
        public Nullable<decimal> retailtax { get; set; }
        public string category { get; set; }
        public string status { get; set; }
        public string download { get; set; }
        public string costcode { get; set; }
        public Nullable<short> weeknumber { get; set; }
        public string subcategory { get; set; }
        public Nullable<int> stklevel { get; set; }
        public Nullable<System.DateTime> transdate { get; set; }
        public Nullable<decimal> pluvalue { get; set; }
        public Nullable<decimal> plutax { get; set; }
        public string barcode { get; set; }
        public string reasoncode { get; set; }
        public int translineint { get; set; }
        public Nullable<int> kitlineint { get; set; }
        public Nullable<double> stklretail { get; set; }
        public Nullable<double> stklcost { get; set; }
        public Nullable<double> stklcm { get; set; }
        public Nullable<int> fiscyrpercode { get; set; }
        public Nullable<int> kitvarint { get; set; }
        public Nullable<decimal> OldQty { get; set; }
        public bool DbProcessStatus { get; set; }
        public short DbProcessedCount { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<bool> Replenished { get; set; }
        public string PromoCode { get; set; }
        public Nullable<int> CustomerOrderLineID { get; set; }
        public Nullable<int> CustomerShipmentLineID { get; set; }
        public Nullable<int> StoreSubcategoryID { get; set; }
    
        public virtual productcode productcode { get; set; }
    }
}