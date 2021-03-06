using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class TillDataTransferDTO
    {
        public string storeId { get; set; }
        public int tillId { get; set; }
        public decimal CashAmountExpected { get; set; }
        public decimal CreditCard { get; set; }
        public decimal ExpensesTotal { get; set; }
        public decimal LSVExpected { get; set; }
        public decimal IncomeExpected { get; set; }
        public decimal netSales { get; set; }
        public decimal delivery_sales { get; set; }
        public decimal gift_card { get; set; }
    }
}