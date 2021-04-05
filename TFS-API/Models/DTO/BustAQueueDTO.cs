using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class BustAQueueDTO
    {
        public string cardNumber { get; set; }

        public List<BustAQueItems> QueueItems { get; set; }

        public class BustAQueItems
        {
            public string sku { get; set; }
            public int qty { get; set; }
        }
    }
}