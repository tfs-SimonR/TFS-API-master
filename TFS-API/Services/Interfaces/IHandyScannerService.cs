using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TFS_API.Models.BindingModels;
using TFS_API.Models.DTO;
using TFS_API.Models.ViewModels;

namespace TFS_API.Services.Interfaces
{
    public interface IHandyScannerService
    {
        List<PreviousWarhouseDeliveriesDTO> GetPreviousWarehouseGoodsOut(string storeId);
        List<SupplierListBM> GetSupplierList();
        NearestStoresVM GetNearestStoresTest(string productCode, string storeid);
        List<GoodInWarehouseBM> GetWarehouseToStoreList(string storeId);
        Task<List<GoodsInInterStoresBM>> GetSupplierToStoresList(string storeId);
        List<StoresListBM> GetStoresList();
        HandyScannerBM GetStockAtBranch(string productCode, string branchId);
        HandyScannerBM GetStockAtBranchBarcode(string barcode, string branchId);
        List<StoreToStoreBM> GetStoreToStoreList(string storeId);
        List<PreviousGoodsOutDTO> GetPreviousGoodsOut(string storeId);
        List<AllStockEnRouteDTO> GetAllStockEnRoute(string storeid);
        NearestStoresVM GetNearestStoresProdcode(string productCode, string storeid);
        StoreDistanceDTO GetStockatLocal(string productCode, string branchId);
        List<HHTActionDTO> GetTaskLists(string storeid);
        StoreWebPanelDTO GetStoreStockWebPanel(string productCode, double longitude, double latitude);
        StoreWebPanelDTOv2 GetStoreStockWebPanelv2(string productCode, double longitude, double latitude);
    }
}