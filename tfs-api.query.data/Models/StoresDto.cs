using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tfs_api.query.data.Models
{
    public class StoresDto
    {
        public string store_id { get; set; }
        public string store_name { get; set; }
        public decimal? lat { get; set; }
        public decimal? longitude { get; set; }
    }
}
