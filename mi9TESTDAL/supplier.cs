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
    
    public partial class supplier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public supplier()
        {
            this.products = new HashSet<product>();
            this.products1 = new HashSet<product>();
            this.suppliers1 = new HashSet<supplier>();
            this.suppliers11 = new HashSet<supplier>();
            this.structlevels = new HashSet<structlevel>();
        }
    
        public string supplier1 { get; set; }
        public string name { get; set; }
        public string shortname { get; set; }
        public string commsline { get; set; }
        public string hoaccountno { get; set; }
        public string grpaccountno { get; set; }
        public string category { get; set; }
        public string typecode { get; set; }
        public string @class { get; set; }
        public string accounttype { get; set; }
        public string buyer { get; set; }
        public string ordermethod { get; set; }
        public string merchplan { get; set; }
        public string invmethod { get; set; }
        public string paymethod { get; set; }
        public string currency { get; set; }
        public string retmethod { get; set; }
        public string creditproc { get; set; }
        public Nullable<int> minlead { get; set; }
        public Nullable<int> maxlead { get; set; }
        public short leadtimetype { get; set; }
        public Nullable<int> orderbranch { get; set; }
        public string historylevel { get; set; }
        public string suppprod { get; set; }
        public Nullable<System.DateTime> lastmovedate { get; set; }
        public System.DateTime createdate { get; set; }
        public Nullable<System.DateTime> amenddate { get; set; }
        public Nullable<System.DateTime> deletedate { get; set; }
        public int suppint { get; set; }
        public string suspreason { get; set; }
        public string suspreasonra { get; set; }
        public string suspreasonap { get; set; }
        public Nullable<System.DateTime> suspdate { get; set; }
        public string suspuser { get; set; }
        public Nullable<System.DateTime> reinstdate { get; set; }
        public string reinstuser { get; set; }
        public string status { get; set; }
        public Nullable<int> minorder { get; set; }
        public Nullable<int> maxorder { get; set; }
        public string anacode { get; set; }
        public Nullable<double> discount { get; set; }
        public string languageid { get; set; }
        public string settletype { get; set; }
        public string taxregno { get; set; }
        public string printprice { get; set; }
        public Nullable<short> supptype { get; set; }
        public string buymethod { get; set; }
        public string stdindclass { get; set; }
        public string dbrating { get; set; }
        public string reportcat { get; set; }
        public string carrier { get; set; }
        public string multilocords { get; set; }
        public Nullable<int> maxdelval { get; set; }
        public Nullable<int> mindelval { get; set; }
        public string allowbo { get; set; }
        public Nullable<int> compafter { get; set; }
        public string sa01 { get; set; }
        public string sa02 { get; set; }
        public string sa03 { get; set; }
        public string sa04 { get; set; }
        public string sa05 { get; set; }
        public string sa06 { get; set; }
        public string sa07 { get; set; }
        public string sa08 { get; set; }
        public string sa09 { get; set; }
        public string sa10 { get; set; }
        public string sa11 { get; set; }
        public string sa12 { get; set; }
        public string sa13 { get; set; }
        public string sa14 { get; set; }
        public string sa15 { get; set; }
        public string sa16 { get; set; }
        public string sa17 { get; set; }
        public string sa18 { get; set; }
        public string sa19 { get; set; }
        public string sa20 { get; set; }
        public string sa21 { get; set; }
        public string sa22 { get; set; }
        public string sa23 { get; set; }
        public string sa24 { get; set; }
        public string sa25 { get; set; }
        public string EdiTestCodeQualifier { get; set; }
        public string EdiTestCode { get; set; }
        public string EdiLiveCodeQualifier { get; set; }
        public string EdiLiveCode { get; set; }
        public byte EdiStatus { get; set; }
        public byte EdiFormat { get; set; }
        public string EdiVersion { get; set; }
        public Nullable<System.DateTime> EdiTestStartDate { get; set; }
        public Nullable<System.DateTime> EdiLiveStartDate { get; set; }
        public Nullable<System.DateTime> EdiSuspendDate { get; set; }
        public byte EdiPo { get; set; }
        public bool EdiPoAck { get; set; }
        public bool EdiPoAmend { get; set; }
        public bool EdiInvoice { get; set; }
        public bool EdiAsn { get; set; }
        public bool EdiPayment { get; set; }
        public bool EdiCatalog { get; set; }
        public bool EdiFuncAck { get; set; }
        public byte AutoAuthorizeOrders { get; set; }
        public string TransportMethod { get; set; }
        public bool Import { get; set; }
        public string Brand { get; set; }
        public bool Active { get; set; }
        public bool Fulfillment { get; set; }
        public string FFShipMode { get; set; }
        public string FFShipModeInt { get; set; }
        public Nullable<decimal> FFHandlingCharge { get; set; }
        public Nullable<decimal> FFHandlingChargeInt { get; set; }
        public Nullable<decimal> FFItemHandlingCharge { get; set; }
        public Nullable<decimal> FFItemHandlingChargeInt { get; set; }
        public byte FFShipChargeType { get; set; }
        public byte FFShipChargeCountType { get; set; }
        public bool FFUsePackaging { get; set; }
        public byte FFPackagingOptimizationType { get; set; }
        public bool CreatedOffline { get; set; }
        public bool Interfaced { get; set; }
        public Nullable<System.DateTime> DateInterfaced { get; set; }
        public bool PreTicket { get; set; }
        public int DUNS { get; set; }
        public byte RequireVendorProduct { get; set; }
        public byte RequireVendorUPC { get; set; }
        public int Edi850ControlNumber { get; set; }
        public bool VMI { get; set; }
        public byte AutoReceiveASN { get; set; }
        public byte CancelAfterDays { get; set; }
        public bool Edi850DTM001InUse { get; set; }
        public bool Edi850DTM010InUse { get; set; }
        public bool Edi850DTM037InUse { get; set; }
        public bool Edi850DTM038InUse { get; set; }
        public bool Edi850DTM074InUse { get; set; }
        public string EdiLastInvoiceCode { get; set; }
        public string EdiLastASNCode { get; set; }
        public bool VendorLocationsInUse { get; set; }
        public byte EdiUseTildeAsSegmentTerminator { get; set; }
        public byte EnforceVendorCompanyLocationRules { get; set; }
        public bool UseSystemTolerance { get; set; }
        public string ImportingCompany { get; set; }
        public string ImportingCompanyGLU { get; set; }
        public bool SbaContractInUse { get; set; }
        public Nullable<System.DateTime> SbaContractStartDate { get; set; }
        public decimal CommercialDiscountPercent { get; set; }
        public Nullable<System.DateTime> CommercialDiscountPercentStartDate { get; set; }
        public decimal DefectiveMerchandiseDiscountPercent { get; set; }
        public Nullable<System.DateTime> DefectiveMerchandiseDiscountPercentStartDate { get; set; }
        public string EdiProvider { get; set; }
        public string EdiProviderGLU { get; set; }
        public string EdiMessagingProtocol { get; set; }
        public string EdiMessagingProtocolGLU { get; set; }
        public bool OrderPlanRequired { get; set; }
        public bool AllowBrokenPacks { get; set; }
        public byte NewStoreDiscountDays { get; set; }
        public bool EdiProductActivity { get; set; }
        public bool UpdateCostOnASN { get; set; }
        public string RebateVendor { get; set; }
        public string RebateVendorGLU { get; set; }
        public bool VerticallyIntegratedRetailer { get; set; }
        public bool SpecialOrders { get; set; }
        public bool SpecialOrdersSpecRequired { get; set; }
        public bool Edi850DTM001DeliveryDateInUse { get; set; }
        public bool Edi850SDQInUse { get; set; }
        public bool Edi850TD5InUse { get; set; }
        public bool Edi850ITDInUse { get; set; }
        public bool EdiFuncAckForInvoice { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<product> products { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<product> products1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<supplier> suppliers1 { get; set; }
        public virtual supplier supplier2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<supplier> suppliers11 { get; set; }
        public virtual supplier supplier3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<structlevel> structlevels { get; set; }
    }
}
