using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.DTO
{
    public class WriteOffDTO
    {
        public string user_id { get; set; }
        public string store_id { get; set; }

        public string reason_code { get; set; }

        public List<WriteOffList> write_off_items { get; set; }

        public class WriteOffList
        {
            public string sku { get; set; }

            public int sku_counted_qty { get; set; }
        }

    }
}