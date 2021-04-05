using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class ReservationAddedDTO
    {
        public long reservationId { get; set; }
        public string store_name { get; set; }
        public string order_item_description { get; set; }
        public string customer_name { get; set; }
        public string store_address { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
    }
}