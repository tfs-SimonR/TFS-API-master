using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.PostViewModels
{
    public class SupplierToStoreDTO

    {
        public string po_number { get; set; }
        public string store_id { get; set; }
        public string user_name { get; set; }
        public List<ShipmentItems> shipment_items { get; set; }
    }
    public class ShipmentItems
    {
        
        public string sku { get; set; }
        public int sku_counted_qty { get; set; }
        public int sku_expected_qty { get; set; }
        
    }
}