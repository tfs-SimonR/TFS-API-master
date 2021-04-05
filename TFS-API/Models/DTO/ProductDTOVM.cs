﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class ProductDTOVM
    {
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
        public int? avlead { get; set; }
        public string vatcode { get; set; }
        public double? cubefactor { get; set; }
        public double? weight { get; set; }
        public double? facing { get; set; }
        public double? height { get; set; }
        public double? width { get; set; }
        public double? depth { get; set; }
        public double? length { get; set; }
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
        public DateTime? lastmovedate { get; set; }
        public DateTime? amenddate { get; set; }
        public string amenduser { get; set; }
        public DateTime createdate { get; set; }
        public DateTime? deletedate { get; set; }
        public DateTime? introdate { get; set; }
        public DateTime? discondate { get; set; }
        public DateTime? suspdate { get; set; }
        public DateTime? lastcheckdte { get; set; }
        public DateTime? nextcheckdte { get; set; }
        public double? stdcost { get; set; }
        public double? nxtstdcost { get; set; }
        public DateTime? nxtstddate { get; set; }
        public double? oldstdcost { get; set; }
        public double? avcost { get; set; }
        public double? repcost { get; set; }
        public double? valueprice { get; set; }
        public double? ordercost { get; set; }
        public double? storagecost { get; set; }
        public double? demandytd { get; set; }
        public double? lostsalesytd { get; set; }
        public double? mingmpc { get; set; }
        public double? maxgmpc { get; set; }
        public double? basegmpc { get; set; }
        public int? reportseq { get; set; }
        public int? accesscount { get; set; }
        public int? abcrankno { get; set; }
        public int? abclastrank { get; set; }
        public DateTime? abclastdate { get; set; }
        public int prodint { get; set; }
        public short? life { get; set; }
        public int? idealset { get; set; }
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
        public double? shelldep { get; set; }
        public double? bottledep { get; set; }
        public string paytable { get; set; }
        public string competeprice { get; set; }
        public string knownvalue { get; set; }
        public string manufacturer { get; set; }
        public string buyer { get; set; }
        public string replenmethod { get; set; }
        public short? backsalhist { get; set; }
        public double? wksstockheld { get; set; }
        public DateTime? replenstdte { get; set; }
        public DateTime? replenenddte { get; set; }
        public DateTime? statchgdate { get; set; }
        public string range { get; set; }
        public int? CommodityID { get; set; }
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
        public int? DefaultVendorProductID { get; set; }
        public string level4 { get; set; }
        public string level5 { get; set; }
        public int? AltMerchHierarchy1ID { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int InnerPack { get; set; }
        public int MasterPack { get; set; }
        public DateTime? MarkdownDate { get; set; }
        public bool PreTicket { get; set; }
        public bool Assorted { get; set; }
        public string AllocUnits { get; set; }
        public int? SetQty { get; set; }
        public int CartonQty { get; set; }
        public int PalletQty { get; set; }
        public DateTime? WebDate { get; set; }
        public DateTime? CopyDate { get; set; }
        public DateTime? PhotoDate { get; set; }
        public string PhotoLocation { get; set; }
        public string SetType { get; set; }
        public bool Replenished { get; set; }
        public byte ReplenishmentSourceType { get; set; }
        public string POSMarkdown { get; set; }
        public bool VMI { get; set; }
        public bool Scaled { get; set; }
        public string PriceCompareUnits { get; set; }
        public decimal? PriceCompareFactor { get; set; }
        public int? SalesUnitsID { get; set; }
        public int? AltSalesUnitsID { get; set; }
        public int? AllocUnitsID { get; set; }
        public int? PDQUnitsID { get; set; }
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
        public int? AssociatedProdint { get; set; }
        public string POSScriptLine1 { get; set; }
        public string POSScriptLine2 { get; set; }
        public bool AllowPriceOverrideInPOS { get; set; }
        public bool ForceEnteringQtyInPOS { get; set; }
        public DateTime? WebPublishedDate { get; set; }
        public byte NonMerchType { get; set; }
        public string HTSCode { get; set; }
        public string ProPricingGroup { get; set; }
        public string ProPricingType { get; set; }
        public bool ShipQtyMayExceedAvailable { get; set; }
        public DateTime? WarehouseReplenishmentStartDate { get; set; }
        public DateTime? WarehouseReplenishmentEndDate { get; set; }
    }
}