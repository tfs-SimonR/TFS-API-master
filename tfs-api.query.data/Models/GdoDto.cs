using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tfs_api.query.data.Models
{
    public class GdoDto
    {
        public string RandomValue { get; set; }

        public string StoreDestination { get; set; }
        public string StoreSource { get; set; }

        public string GeneratedGDO { get; set; }
        public DateTime? DateCreated { get; set; }

        public bool? isUSed { get; set; }
    }
}
