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
    
    public partial class purchcost
    {
        public int purchlineint { get; set; }
        public string costcode { get; set; }
        public short homecurrency { get; set; }
        public int ID { get; set; }
        public byte SourceType { get; set; }
        public Nullable<int> SourceID { get; set; }
        public bool CreatedOffline { get; set; }
        public decimal NetCost { get; set; }
        public decimal LandedCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal DutiableCost { get; set; }
        public string Reason { get; set; }
        public bool FromHeader { get; set; }
        public decimal costvalue { get; set; }
    }
}
