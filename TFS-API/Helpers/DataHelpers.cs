using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Mi9DataAccessLayer;
using TFS_API.Models;

namespace TFS_API.Helpers
{
    internal class DataHelpers
    {
        private static readonly Mi9DBEntities _mi9Db = new Mi9DBEntities();
        private static readonly Random random = new Random();

        public static string GetIpAddress()
        {
            string ipAddressString = HttpContext.Current.Request.UserHostAddress;

            if (ipAddressString == null)
                return null;

            IPAddress ipAddress;
            IPAddress.TryParse(ipAddressString, out ipAddress);

            // If we got an IPV6 address, then we need to ask the network for the IPV4 address 
            // This usually only happens when the browser is on the same machine as the server.
            if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                ipAddress = System.Net.Dns.GetHostEntry(ipAddress).AddressList
                    .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            }

            return ipAddress.ToString();
        }


        public static IEnumerable<DateTime> Range(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d));
        }
        public static decimal ConvertdecimaltoInt(decimal rrp)
        {
            var str = rrp.ToString().Replace(".", string.Empty);
            var dec = decimal.Parse(str);
            return dec;
        }

        public static decimal Normalize(decimal value)
        {
            return value / 1.000000000000000000000000000000000m;
        }

        public static decimal TruncateDecimal(decimal? value, int precision)
        {
            var step = (decimal) Math.Pow(10, precision);
            var tmp = Math.Truncate((decimal) (step * value));
            return tmp;
        }

        public static string FormatNumberSpecialNumber(decimal inputString)
        {
            var number = inputString;
            var formattedString = number / 100;
            return formattedString.ToString(CultureInfo.InvariantCulture);
        }


        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomNumber(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static int getEpochTime()
        {
            var epochTicks = new DateTime(1970, 1, 1).Ticks;
            var unixTime = (DateTime.UtcNow.Ticks - epochTicks) / TimeSpan.TicksPerSecond;

            return (int) unixTime;
        }

        public static string setQuantity(string number)
        {
            var numberLength = number.Length;

            var theString = "00000000";
            var aStringBuilder = new StringBuilder(theString);

            if (numberLength < 2)
            {
                aStringBuilder.Remove(5, numberLength);
                aStringBuilder.Insert(5, number);
            }
            else if (numberLength == 2)
            {
                aStringBuilder.Remove(4, numberLength);
                aStringBuilder.Insert(4, number);
            }
            else if (numberLength == 3)
            {
                aStringBuilder.Remove(3, numberLength);
                aStringBuilder.Insert(3, number);
            }
            else if (numberLength == 4)
            {
                aStringBuilder.Remove(2, numberLength);
                aStringBuilder.Insert(2, number);
            }
            else if (numberLength == 5)
            {
                aStringBuilder.Remove(1, numberLength);
                aStringBuilder.Insert(1, number);
            }

            theString = aStringBuilder.ToString();

            return theString;
        }

        public static string replaceLastFour(string number)
        {
            var lengthofnumber = number.Length;

            var thenumber = "00000000";

            var replace = thenumber.Substring(0, 8 - lengthofnumber) + number;

            return replace;
        }

        public static double CalculateDistance(double sLatitude, double sLongitude, double eLatitude,
            double eLongitude)
        {
            var radiansOverDegrees = Math.PI / 180.0;

            var sLatitudeRadians = sLatitude * radiansOverDegrees;
            var sLongitudeRadians = sLongitude * radiansOverDegrees;
            var eLatitudeRadians = eLatitude * radiansOverDegrees;
            var eLongitudeRadians = eLongitude * radiansOverDegrees;

            var dLongitude = eLongitudeRadians - sLongitudeRadians;
            var dLatitude = eLatitudeRadians - sLatitudeRadians;

            var result1 = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                          Math.Cos(sLatitudeRadians) * Math.Cos(eLatitudeRadians) *
                          Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Using 3956 as the number of miles around the earth
            var result2 = 3956.0 * 2.0 * Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

            return result2;
        }

        public static double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
        {
            var d1 = latitude * (Math.PI / 180.0);
            var num1 = longitude * (Math.PI / 180.0);
            var d2 = otherLatitude * (Math.PI / 180.0);
            var num2 = otherLongitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        public static string ToFinancialYear(DateTime dateTime)
        {
            return "Financial Year " + (dateTime.Month >= 4 ? dateTime.Year + 1 : dateTime.Year);
        }

        public static string ToFinancialYearShort(DateTime dateTime)
        {
            return "FY" + (dateTime.Month >= 4 ? dateTime.AddYears(1).ToString("yy") : dateTime.ToString("yy"));
        }

        public static string ToFYString(DateTime dateTime, bool longFlag = false, int monthLimit = 3)
        {
            var format = longFlag ? "yyyy" : "yy";
            return dateTime.Month > monthLimit ? dateTime.AddYears(1).ToString(format) : dateTime.ToString(format);
        }

        public static string GetProductDescription(int prodint)
        {
            var _description = "";
            var getDescription = (from products in _mi9Db.products
                join productcodes in _mi9Db.productcodes on products.prodint equals productcodes
                    .prodint
                where products.prodint == prodint
                select products).FirstOrDefault();
            if (getDescription != null)
                _description = getDescription.proddesc;
            else
                _description = "Not Found";

            return _description;
        }

        public static int GetProdInt(string sku)
        {
            var _prodInt = 0;
            var getProdint = (from prodcodes in _mi9Db.productcodes
                where prodcodes.variantcode == sku
                select prodcodes).FirstOrDefault();
            if (getProdint != null)
                _prodInt = getProdint.prodint;
            else
                _prodInt = 0;

            return _prodInt;
        }

        public static double? GetCostPrice(string sku)
        {
            var _prodInt = GetProdInt(sku);
            double? _costPrice = 0;
            var getcostPrice = _mi9Db.TOFS_CostPrice.FirstOrDefault(x => x.prodcode == sku);
            if (getcostPrice != null)
                _costPrice = getcostPrice.Cost;
            else
                _costPrice = _mi9Db.TOFS_CostPrice.FirstOrDefault(x => x.prodint == _prodInt)?.Cost;

            return _costPrice;
        }

        public static string GetStoreName(string storeId)
        {
            var _storename = "";
            var storeName = _mi9Db.tfs_store_details.FirstOrDefault(x => x.storeId == storeId);
            if (storeName != null)
                _storename = storeName.store_name;
            else
                _storename = "No Store Name Found";

            return _storename;
        }

        public static decimal? GetRetailPrice(int prodint)
        {
            decimal? _getRetailPrice = 0;
            var getRetailPrice = (from products in _mi9Db.products
                join productcodes in _mi9Db.productcodes on products.prodint equals productcodes
                    .prodint
                join retailprice in _mi9Db.TOFS_RetailPrice on productcodes.varint equals
                    retailprice.Varint
                where products.prodint == prodint
                select retailprice).FirstOrDefault();
            if (getRetailPrice != null)
                _getRetailPrice = getRetailPrice.Retail_Price;
            else
                _getRetailPrice = 0;

            return _getRetailPrice;
        }

        public static decimal FindDifference(decimal nr1, decimal nr2)
        {
            return Math.Abs(nr1 - nr2);
        }

        public static string GetReasonCodeDescription(string reasonCode)
        {
            string _reasonDes = null;
            var resonList = new List<PPReasonCodes>
            {
                new PPReasonCodes {id = 1, reason_code = "SA", description = "Write Off - Damaged"},
                new PPReasonCodes {id = 2, reason_code = "SB", description = "Write Off - Theft"},
                new PPReasonCodes {id = 3, reason_code = "SC", description = "Write Off - Damaged on Delivery"},
                new PPReasonCodes {id = 4, reason_code = "SD", description = "Write Off - Out Of Date"},
                new PPReasonCodes {id = 5, reason_code = "SF", description = "Write Off - Head Office"},
                new PPReasonCodes {id = 6, reason_code = "SI", description = "Write Off - Own Store Use"},
                new PPReasonCodes {id = 7, reason_code = "SJ", description = "Write Off - Odd Shoes"},
                new PPReasonCodes {id = 8, reason_code = "SL", description = "Write Off - Stock Correction"},
                new PPReasonCodes {id = 9, reason_code = "SM", description = "Write Off - Colleague Uniform"},
                new PPReasonCodes {id = 10, reason_code = "SN", description = "Write Off - Delivery Discrepancy"},
                new PPReasonCodes {id = 11, reason_code = "SY", description = "Write On - Delivery Discrepancy"},
                new PPReasonCodes {id = 12, reason_code = "SZ", description = "Write On - Stock Correction"}
            };

            var reasonDescriptoion = resonList.FirstOrDefault(x => x.reason_code == reasonCode);
            if (reasonDescriptoion != null)
                _reasonDes = reasonDescriptoion.description;

            return _reasonDes;
        }

        public static double kmphTOmph(double kmph)
        {
            return 0.6214 * kmph;
        }
    }
}