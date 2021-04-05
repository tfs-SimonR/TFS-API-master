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
    
    public partial class consighdr
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public consighdr()
        {
            this.consigdests = new HashSet<consigdest>();
            this.consigdocs = new HashSet<consigdoc>();
        }
    
        public string warehouse { get; set; }
        public string consignment { get; set; }
        public string description { get; set; }
        public string collect { get; set; }
        public string carrier { get; set; }
        public int notepad { get; set; }
        public System.DateTime createdate { get; set; }
        public Nullable<System.DateTime> amenddate { get; set; }
        public Nullable<System.DateTime> despatchdate { get; set; }
        public Nullable<System.DateTime> issuedate { get; set; }
        public Nullable<System.DateTime> confirmdate { get; set; }
        public string consigdoc { get; set; }
        public short exceptions { get; set; }
        public string status { get; set; }
        public string authorityno { get; set; }
        public string CreateUser { get; set; }
        public string AmendUser { get; set; }
        public string AuthorizeUser { get; set; }
        public Nullable<System.DateTime> AuthorizeDate { get; set; }
        public bool ManyToOne { get; set; }
        public string ChannelType { get; set; }
        public bool TransferRequest { get; set; }
        public bool TransferAll { get; set; }
        public string BuyerCode { get; set; }
        public string ReasonCode { get; set; }
        public bool RTV { get; set; }
        public Nullable<int> TransferRequestID { get; set; }
        public bool StoreInitiated { get; set; }
        public string RTVAuthRequestedUser { get; set; }
        public Nullable<System.DateTime> RTVAuthRequestedDate { get; set; }
        public string RTVRejectNotes { get; set; }
        public string RTVRejectedUser { get; set; }
        public Nullable<System.DateTime> RTVRejectedDate { get; set; }
        public string RTVCancelReasonCode { get; set; }
        public string RTVCancelReasonGLU { get; set; }
        public Nullable<System.DateTime> DateExtractedToAP { get; set; }
        public bool AllowAddProductsDuringPack { get; set; }
        public Nullable<System.DateTime> DateCreatedInStore { get; set; }
        public string UserCreatedInStore { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<consigdest> consigdests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<consigdoc> consigdocs { get; set; }
    }
}
