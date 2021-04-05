using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Serilog;
using TFS_API.Dto.Mappers;
using tfs_api.query.data.DataAccess;
using tfs_api.query.data.Mappers;
using tfs_api.query.data.Models;

namespace TFS_API.BusinessObjects
{
    /// <summary>
    /// Warehouse to Store Business Objects
    /// </summary>
    public class WarehouseBo
    {
        private ScannerDAL dal = new ScannerDAL(Globals.WarehouseConnectionString);

        public delegate void ReturnErrorHandler(string methodName, Exception exception);

        public event ScannerBo.ReturnErrorHandler ReturnError = delegate { };

        private void Dal_ReturnDalError(string methodName, Exception exception)
        {
            Log.Error(exception, methodName);
            ReturnError(methodName, exception);
        }

        public ProductDto GetProductInfo(string id)
        {
            dal.ReturnDalError += Dal_ReturnDalError;
            DataTable data = dal.GetProductInfo(id);
            var result = new ProductMapper().Map(data);
            return result.FirstOrDefault();
        }

        public WarehouseDto GetWarehouseDeliveriesByStore(int id)
        {
            dal.ReturnDalError += Dal_ReturnDalError;
            DataTable data = dal.GetWareHouseDeliveriesByStore(id);
            var result = new WarehouseMapper().Map(data);
            return result.FirstOrDefault();
        }

        public WarehouseDto GetPallet(string id)
        {
            dal.ReturnDalError += Dal_ReturnDalError;
            DataTable data = dal.GetPalletById(id);
            var result = new WarehouseMapper().Map(data);
            return result.FirstOrDefault();
        }

        public bool UpdatePallet(WarehouseDto gdoDetail)
        {
            dal.ReturnDalError += Dal_ReturnDalError;
            return dal.UpdatePallet(gdoDetail);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gdoDetail"></param>
        /// <returns></returns>
        public bool InsertGoodsOutPallet(WarehouseDto gdoDetail)
        {
            dal.ReturnDalError += Dal_ReturnDalError;
            try
            {
                dal.InsertShipment(gdoDetail);
                return true;
            }
            catch (Exception exception)
            {
                ReturnError("InsertGoodsOut", exception);
                return false;
            }
        }

    }
}