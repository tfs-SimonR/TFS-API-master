using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFS_API.Dto;
using TFS_API.Dto.Mappers;
using tfs_api.query.data.DataAccess;
using tfs_api.query.data.Mappers;
using tfs_api.query.data.Models;
using TFS_API.Service.Interface;

namespace TFS_API.Service
{
    public class ScannerService: IScannerService, IDisposable {

        #region Disposal 

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_dal?.Dispose();
            }
        }

        #endregion

        public delegate void ScannerServiceExceptionHandler(Exception exception, string methodName);

        public delegate void ScannerServiceEventHandler(string methodName, string eventText);

        public delegate void ScannerServiceResultHandler(bool result, string methodName);

        public event ScannerServiceEventHandler ClickCollectServiceEvent = delegate { };
        public event ScannerServiceExceptionHandler ClickCollectServiceException = delegate { };
        public event ScannerServiceResultHandler ClickCollectServiceResult = delegate { };

        public List<StoresDto> GetStores() {
            using (var dal = new ScannerDAL(Globals.ConnectionString)) {
                QueryAnalysis.Start();
                var data = dal.GetStores();
                QueryAnalysis.Stop();
                var map = new StoreMapper().Map(data);
                return map;
            }
        }

        public List<StoresDto> GetStoresById(string id)
        {
            using (var dal = new ScannerDAL(Globals.ConnectionString))
            {
                QueryAnalysis.Start();
                var data = dal.GetStoresById(id);
                QueryAnalysis.Stop();
                var map = new StoreMapper().Map(data);
                return map;
            }
        }

        public List<RTWReasonCodeDto> GetRTWCodes()
        {
            using (var dal = new ScannerDAL(Globals.ConnectionString))
            {
                QueryAnalysis.Start();
                var data = dal.GetRTWCodes();
                QueryAnalysis.Stop();
                var map = new RTWReasonCodeMapper().Map(data);
                return map;
            }
        }

        public List<ProductDto> GetProductObject(string variant)
        {
            using (var dal = new ScannerDAL(Globals.ConnectionString))
            {
                QueryAnalysis.Start();
                var data = dal.GetProductInfo(variant);
                QueryAnalysis.Stop();
                var map = new ProductMapper().Map(data);
                return map;
            }
        } 

        public List<GdoDto> GetGdo()
        {
            using (var dal = new ScannerDAL(Globals.ConnectionString))
            {
                QueryAnalysis.Start();
                var data = dal.GetNextGDO();
                QueryAnalysis.Stop();
                var map = new GDOMapper().Map(data);
                return map;
            }
        }

        public List<WarehouseDto> GetWarehouseList(int id)
        {
            using (var dal = new ScannerDAL(Globals.WarehouseConnectionString))
            {
                QueryAnalysis.Start();
                var data = dal.GetWareHouseDeliveriesByStore(id);
                QueryAnalysis.Stop();
                var map = new WarehouseMapper().Map(data);
                return map;
            }
        }

        public List<WarehouseDto> GetInterStoreList(int id)
        {
            using (var dal = new ScannerDAL(Globals.WarehouseConnectionString))
            {
                QueryAnalysis.Start();
                var data = dal.GetInterStoreList(id);
                QueryAnalysis.Stop();
                var map = new WarehouseMapper().Map(data);
                return map;
            }
        }
        public List<WarehouseDto> GetInterStoreListStoresCentral(int id)
        {
            using (var dal = new ScannerDAL(Globals.WarehouseConnectionString))
            {
                QueryAnalysis.Start();
                var data = dal.GetInterStoreListStoresCentral(id);
                QueryAnalysis.Stop();
                var map = new WarehouseMapper().Map(data);
                return map;
            }
        }

        public List<WarehouseDto> GetInterStoreDetail(string id)
        {
            using (var dal = new ScannerDAL(Globals.WarehouseConnectionString))
            {
                QueryAnalysis.Start();
                var data = dal.GetInterStoreDetail(id);
                QueryAnalysis.Stop();
                var map = new WarehouseMapper().Map(data);
                return map;
            }
        }

        public List<WarehouseDto> GetPalletByID(string id)
        {
            using (var dal = new ScannerDAL(Globals.WarehouseConnectionString))
            {
                QueryAnalysis.Start();
                var data = dal.GetPalletById(id);
                QueryAnalysis.Stop();
                var map = new WarehouseMapper().Map(data);
                return map;
            }
        }
    }
}