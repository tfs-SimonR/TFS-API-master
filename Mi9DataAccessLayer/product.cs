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
    
    public partial class product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public product()
        {
            this.productcodes = new HashSet<productcode>();
            this.productcodes1 = new HashSet<productcode>();
            this.products1 = new HashSet<product>();
            this.supproducts = new HashSet<supproduct>();
            this.purchordlines = new HashSet<purchordline>();
        }
    
        public string prodcode { get; set; }
        public string velcode { get; set; }
        public string altkey { get; set; }
        public string barcode { get; set; }
        public string proddesc { get; set; }
        public string longdescind { get; set; }
        public string level1 { get; set; }
        public string level2 { get; set; }
        public string level3 { get; set; }
        public string prodquality { get; set; }
        public string prodstatus { get; set; }
        public string prodtype { get; set; }
        public string prodhistlev { get; set; }
        public string prodorderind { get; set; }
        public string prodallocind { get; set; }
        public string kitind { get; set; }
        public string packcode { get; set; }
        public string supplier { get; set; }
        public string supptable { get; set; }
        public Nullable<int> avlead { get; set; }
        public string vatcode { get; set; }
        public Nullable<double> cubefactor { get; set; }
        public Nullable<double> weight { get; set; }
        public Nullable<double> facing { get; set; }
        public Nullable<double> height { get; set; }
        public Nullable<double> width { get; set; }
        public Nullable<double> depth { get; set; }
        public Nullable<double> length { get; set; }
        public string convcode { get; set; }
        public string purchunits { get; set; }
        public string stockunits { get; set; }
        public string salesunits { get; set; }
        public string abcclass { get; set; }
        public string analcode1 { get; set; }
        public string analcode2 { get; set; }
        public string analcode3 { get; set; }
        public string analcode4 { get; set; }
        public string analcode5 { get; set; }
        public string prodclass { get; set; }
        public string disccat { get; set; }
        public string commcat { get; set; }
        public string resprodcode { get; set; }
        public string seasoncode { get; set; }
        public string misccharge { get; set; }
        public string merchmodel { get; set; }
        public string carrmethod { get; set; }
        public string carrcharge { get; set; }
        public string handcharge { get; set; }
        public string surcharge { get; set; }
        public string salesnomcode { get; set; }
        public string purchnomcode { get; set; }
        public string stocknomcode { get; set; }
        public Nullable<System.DateTime> lastmovedate { get; set; }
        public Nullable<System.DateTime> amenddate { get; set; }
        public string amenduser { get; set; }
        public System.DateTime createdate { get; set; }
        public Nullable<System.DateTime> deletedate { get; set; }
        public Nullable<System.DateTime> introdate { get; set; }
        public Nullable<System.DateTime> discondate { get; set; }
        public Nullable<System.DateTime> suspdate { get; set; }
        public Nullable<System.DateTime> lastcheckdte { get; set; }
        public Nullable<System.DateTime> nextcheckdte { get; set; }
        public Nullable<double> stdcost { get; set; }
        public Nullable<double> nxtstdcost { get; set; }
        public Nullable<System.DateTime> nxtstddate { get; set; }
        public Nullable<double> oldstdcost { get; set; }
        public Nullable<double> avcost { get; set; }
        public Nullable<double> repcost { get; set; }
        public Nullable<double> valueprice { get; set; }
        public Nullable<double> ordercost { get; set; }
        public Nullable<double> storagecost { get; set; }
        public Nullable<double> demandytd { get; set; }
        public Nullable<double> lostsalesytd { get; set; }
        public Nullable<double> mingmpc { get; set; }
        public Nullable<double> maxgmpc { get; set; }
        public Nullable<double> basegmpc { get; set; }
        public Nullable<int> reportseq { get; set; }
        public Nullable<int> accesscount { get; set; }
        public Nullable<int> abcrankno { get; set; }
        public Nullable<int> abclastrank { get; set; }
        public Nullable<System.DateTime> abclastdate { get; set; }
        public int prodint { get; set; }
        public Nullable<short> life { get; set; }
        public Nullable<int> idealset { get; set; }
        public string brand { get; set; }
        public string contind { get; set; }
        public string introseason { get; set; }
        public string reordmethod { get; set; }
        public string customsdesc { get; set; }
        public string prodstruct { get; set; }
        public string labeltype { get; set; }
        public string sourcecode { get; set; }
        public string licencecode { get; set; }
        public string foodstamp { get; set; }
        public Nullable<double> shelldep { get; set; }
        public Nullable<double> bottledep { get; set; }
        public string paytable { get; set; }
        public string competeprice { get; set; }
        public string knownvalue { get; set; }
        public string manufacturer { get; set; }
        public string buyer { get; set; }
        public string replenmethod { get; set; }
        public Nullable<short> backsalhist { get; set; }
        public Nullable<double> wksstockheld { get; set; }
        public Nullable<System.DateTime> replenstdte { get; set; }
        public Nullable<System.DateTime> replenenddte { get; set; }
        public Nullable<System.DateTime> statchgdate { get; set; }
        public string range { get; set; }
        public Nullable<int> CommodityID { get; set; }
        public string VarTableCode { get; set; }
        public string CoordinateGroup { get; set; }
        public string Silhouette { get; set; }
        public bool CreatedOffline { get; set; }
        public string CreateUser { get; set; }
        public string LongDesc { get; set; }
        public string StatChgUser { get; set; }
        public string PrimaryMaterial { get; set; }
        public byte KitDisassemblyAction { get; set; }
        public bool Stockable { get; set; }
        public bool Consignment { get; set; }
        public bool SpecialOrder { get; set; }
        public string NonMarkdownReason { get; set; }
        public decimal AvReceivedCost { get; set; }
        public int ReceivedQty { get; set; }
        public Nullable<int> DefaultVendorProductID { get; set; }
        public string level4 { get; set; }
        public string level5 { get; set; }
        public Nullable<int> AltMerchHierarchy1ID { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public int InnerPack { get; set; }
        public int MasterPack { get; set; }
        public Nullable<System.DateTime> MarkdownDate { get; set; }
        public bool PreTicket { get; set; }
        public bool Assorted { get; set; }
        public string AllocUnits { get; set; }
        public Nullable<int> SetQty { get; set; }
        public int CartonQty { get; set; }
        public int PalletQty { get; set; }
        public Nullable<System.DateTime> WebDate { get; set; }
        public Nullable<System.DateTime> CopyDate { get; set; }
        public Nullable<System.DateTime> PhotoDate { get; set; }
        public string PhotoLocation { get; set; }
        public string SetType { get; set; }
        public bool Replenished { get; set; }
        public byte ReplenishmentSourceType { get; set; }
        public string POSMarkdown { get; set; }
        public bool VMI { get; set; }
        public bool Scaled { get; set; }
        public string PriceCompareUnits { get; set; }
        public Nullable<decimal> PriceCompareFactor { get; set; }
        public Nullable<int> SalesUnitsID { get; set; }
        public Nullable<int> AltSalesUnitsID { get; set; }
        public Nullable<int> AllocUnitsID { get; set; }
        public Nullable<int> PDQUnitsID { get; set; }
        public string AltSalesUnitsSKU { get; set; }
        public string HazMatClass { get; set; }
        public string SecurityTagType { get; set; }
        public string SellingRestriction { get; set; }
        public string POSKeyword { get; set; }
        public decimal CommissionPercent { get; set; }
        public string Fee { get; set; }
        public bool NeverOutOfStock { get; set; }
        public bool FarmerTaxExempt { get; set; }
        public bool SerialNumRequired { get; set; }
        public bool WarrantyAvailable { get; set; }
        public bool PreventEnteringQtyForRepeatItemInPOS { get; set; }
        public bool FoodStampCanBeUsed { get; set; }
        public Nullable<int> AssociatedProdint { get; set; }
        public string POSScriptLine1 { get; set; }
        public string POSScriptLine2 { get; set; }
        public bool AllowPriceOverrideInPOS { get; set; }
        public bool ForceEnteringQtyInPOS { get; set; }
        public Nullable<System.DateTime> WebPublishedDate { get; set; }
        public byte NonMerchType { get; set; }
        public string HTSCode { get; set; }
        public string ProPricingGroup { get; set; }
        public string ProPricingType { get; set; }
        public bool ShipQtyMayExceedAvailable { get; set; }
        public Nullable<System.DateTime> WarehouseReplenishmentStartDate { get; set; }
        public Nullable<System.DateTime> WarehouseReplenishmentEndDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<productcode> productcodes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<productcode> productcodes1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<product> products1 { get; set; }
        public virtual product product1 { get; set; }
        public virtual supplier supplier1 { get; set; }
        public virtual supplier supplier2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<supproduct> supproducts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<purchordline> purchordlines { get; set; }
    }
}
