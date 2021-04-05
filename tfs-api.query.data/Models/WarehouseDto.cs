using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tfs_api.query.data.Models
{
    public class WarehouseDto
    {
        public string Pallet_Number { get; set; }
        public string Destination_Id { get; set; }
        public string Destination_Name { get; set; }
        public string Source_Id { get; set; }
        public string SourceName { get; set; }
        [DisplayName("sku")]
        public string SKU { get; set; }
        [DisplayName("stock_qty")]
        public int? Qty { get; set; }
        [DisplayName("description")]
        public string Description { get; set; }
        public DateTime? Shipment_Date { get; set; }
        public DateTime? BookedIn_Date { get; set; }
        public bool? IsDelivered { get; set; }
        public bool? IsHeld { get; set; }
        public int? varint { get; set; }
    }
}
