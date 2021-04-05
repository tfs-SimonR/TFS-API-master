using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.ViewModels
{
    public class StockDTO
    {
        public string branchcode { get; set; }
        public int varint { get; set; }
        public int? lastmovedate { get; set; }
        public int onorder { get; set; }
        public double avcost { get; set; }
        public decimal retail { get; set; }
        public decimal showroom { get; set; }
        public decimal damaged { get; set; }
        public decimal allocated { get; set; }
        public decimal quarantine { get; set; }
        public string stockcheck { get; set; }
        public int? lastcheckdte { get; set; }
        public short discrepancy { get; set; }
        public double? stdcost { get; set; }
        public double? lastcost { get; set; }
        public double? carryoverqty { get; set; }
        public double? carryovercst { get; set; }
        public DateTime? firstdeldate { get; set; }
        public DateTime? lastdeldate { get; set; }
        public DateTime? FirstSoldDate { get; set; }
        public DateTime? LastSoldDate { get; set; }
        public decimal CrossDockQty { get; set; }
        public decimal TransferHeldQty { get; set; }
    }
}