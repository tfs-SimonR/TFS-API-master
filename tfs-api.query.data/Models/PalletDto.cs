using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tfs_api.query.data.Models
{
    public class PalletDto
    {
        public DateTime BookedIn_Date { get; set; }

        public bool IsDelivered { get; set; }
        public bool IsHeld { get; set; }
    }
}
