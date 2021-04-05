using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tfs_api.query.data.Models;

namespace TFS_API.Service.Interface
{
    public interface IARTSService
    {
        string WarehouseReceipt(List<WarehouseDto> dto);
        string PurchaseOrderReceipt(List<WarehouseDto> dto);
        string InterStoreReceipt(List<WarehouseDto> dto);
        string MovementOutOfStore(List<WarehouseDto> dto);
    }
}