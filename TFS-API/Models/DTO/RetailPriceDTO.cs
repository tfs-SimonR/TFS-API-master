using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class RetailPriceDTO
    {
        public int plint { get; set; }
        public int varint { get; set; }
        public decimal retailprice { get; set; }
        public Nullable<System.DateTime> startdate { get; set; }
        public Nullable<int> joincode { get; set; }
        public short status { get; set; }
        public string reasoncode { get; set; }
        public Nullable<System.DateTime> enddate { get; set; }
        public System.DateTime createdate { get; set; }
        public string createuser { get; set; }
        public Nullable<System.DateTime> amenddate { get; set; }
        public string amenduser { get; set; }
        public double startcost { get; set; }
        public bool CreatedOffline { get; set; }
        public int ID { get; set; }
        public string PriceType { get; set; }
        public Nullable<int> PriceChangeLineID { get; set; }
        public Nullable<int> PriceChangeID { get; set; }
        public Nullable<System.DateTime> DateStatusChanged { get; set; }
        public bool WasStatusResetToNew { get; set; }
        public Nullable<decimal> RetailPriceAltUOM { get; set; }
    }
}