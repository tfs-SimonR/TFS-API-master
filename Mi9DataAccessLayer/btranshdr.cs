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
    
    public partial class btranshdr
    {
        public string transnumber { get; set; }
        public string typecode { get; set; }
        public string reasoncode { get; set; }
        public System.DateTime transdate { get; set; }
        public string branchcode { get; set; }
        public string branchother { get; set; }
        public string nominal { get; set; }
        public string status { get; set; }
        public string download { get; set; }
        public string usercode { get; set; }
        public Nullable<System.DateTime> processdate { get; set; }
        public int transint { get; set; }
        public Nullable<int> postransint { get; set; }
        public string source { get; set; }
        public Nullable<int> KitAssemblyOrderID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public bool SalesAudit { get; set; }
        public string Reference { get; set; }
        public Nullable<int> WhDeliveryID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string ShipCode { get; set; }
        public string TargetCode { get; set; }
        public string AdjOtherTranCode { get; set; }
        public string AdjOtherLocCode { get; set; }
        public string Reference2 { get; set; }
        public Nullable<int> KitDisassemblyID { get; set; }
        public Nullable<int> KitAssemblyID { get; set; }
        public Nullable<System.DateTime> DateExtractedToBI { get; set; }
        public Nullable<int> CustomerOrderID { get; set; }
        public Nullable<int> CustomerShipmentID { get; set; }
        public string UserAmended { get; set; }
        public Nullable<System.DateTime> DateAmended { get; set; }
        public string UserApproved { get; set; }
        public Nullable<System.DateTime> DateApproved { get; set; }
        public string UserRejected { get; set; }
        public Nullable<System.DateTime> DateRejected { get; set; }
        public string UserConfirmed { get; set; }
        public Nullable<System.DateTime> DateConfirmed { get; set; }
        public Nullable<decimal> TotalAbsoluteCost { get; set; }
        public Nullable<decimal> TotalAbsoluteRetail { get; set; }
        public Nullable<int> PIHeaderID { get; set; }
    
        public virtual branch branch { get; set; }
        public virtual branch branch1 { get; set; }
    }
}