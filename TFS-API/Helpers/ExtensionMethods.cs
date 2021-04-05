using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mi9DataAccessLayer;

namespace TFS_API.Helpers
{
    public static class ExtensionMethods
    {
        public static decimal PriceHeaderCheck(this rpricehdr obj, decimal defaltValue)
        {
            if (obj != null && obj.retailprice != null)
                return obj.retailprice;
            return defaltValue;
        }

        public static decimal PriceHistoryCheck(this rpricehist obj, decimal defaltValue)
        {
            if (obj != null && obj.retailprice != null)
                return obj.retailprice;
            return defaltValue;
        }

        public static decimal StockCheck(this brnstock obj, decimal defaltValue)
        {
            if (obj != null && obj.branchcode != null)
                return obj.retail;
            return defaltValue;
        }

        public static decimal? CheckMrp(this supproduct obj, decimal defaltValue)
        {
            if (obj != null && obj.varint != null && obj.retailprice !=null)
                return obj.retailprice;
            return defaltValue;
        }

        public static decimal Normalize(this decimal value)
        {
            return value / 1.000000000000000000000000000000000m;
        }
    }
}