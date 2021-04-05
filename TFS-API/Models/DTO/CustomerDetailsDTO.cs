using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class CustomerDetailsDTO
    {
        public string customerId { get; set; }
        public string forename { get; set; }
        public string surname { get; set; }
        public string postcode { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string activeCardNumber { get; set; }
        public int customerType { get; set; }
    }
}