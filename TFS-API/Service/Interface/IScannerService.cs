using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFS_API.Dto;
using tfs_api.query.data.Models;

namespace TFS_API.Service.Interface
{
    public interface IScannerService
    {
        List<StoresDto> GetStores();
        List<RTWReasonCodeDto> GetRTWCodes();
        List<GdoDto> GetGdo();
        List<WarehouseDto> GetWarehouseList(int id);

        List<WarehouseDto> GetPalletByID(string id);
        List<ProductDto> GetProductObject(string variant);

        List<WarehouseDto> GetInterStoreList(int id);
        List<WarehouseDto> GetInterStoreListStoresCentral(int id);
        List<WarehouseDto> GetInterStoreDetail(string id);

        List<StoresDto> GetStoresById(string id);
    }
}
