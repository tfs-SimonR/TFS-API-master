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
    
    public partial class rpricehist
    {
        public int plint { get; set; }
        public int varint { get; set; }
        public decimal retailprice { get; set; }
        public System.DateTime enddate { get; set; }
        public System.DateTime startdate { get; set; }
        public string reasoncode { get; set; }
        public Nullable<int> joincode { get; set; }
        public System.DateTime createdate { get; set; }
        public string createuser { get; set; }
        public Nullable<System.DateTime> amenddate { get; set; }
        public string amenduser { get; set; }
        public double startcost { get; set; }
        public double endcost { get; set; }
        public string PriceType { get; set; }
        public Nullable<int> PriceChangeLineID { get; set; }
        public int ID { get; set; }
        public Nullable<int> PriceChangeID { get; set; }
        public Nullable<System.DateTime> DateStatusChanged { get; set; }
        public Nullable<decimal> RetailPriceAltUOM { get; set; }
    
        public virtual productcode productcode { get; set; }
    }
}