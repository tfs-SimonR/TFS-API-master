using Serilog;
using System;
using System.Data;
using System.Linq;
using tfs_api.query.data.DataAccess;
using tfs_api.query.data.Models;
using TFS_API.Dto.Mappers;

namespace TFS_API.BusinessObjects
{
    /// <summary>
    /// Handy Scanner Business Objects
    /// </summary>
    public class ScannerBo
    {
        private ScannerDAL dal = new ScannerDAL(Globals.ConnectionString);

        public delegate void ReturnErrorHandler(string methodName, Exception exception);

        public event ReturnErrorHandler ReturnError = delegate { };

        private void Dal_ReturnDalError(string methodName, Exception exception)
        {
            Log.Error(exception, methodName);
            ReturnError(methodName, exception);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gdoDetail"></param>
        /// <returns></returns>
        public bool InsertGoodsOut(WarehouseDto gdoDetail)
        {
            dal.ReturnDalError += Dal_ReturnDalError;
            try {
                dal.InsertShipment(gdoDetail);
                return true;
            }catch (Exception exception) {
                ReturnError("InsertGoodsOut", exception);
                return false;
            }
        }

        public bool UpdateGDOTable(GdoDto gdoObject)
        {
            dal.ReturnDalError += Dal_ReturnDalError;
            try {
                dal.UpdateGdo(gdoObject);
                return true;
            }catch (Exception exception) {
                ReturnError("UpdateGDOTable", exception);
                return false;
            }
        }

    }
}