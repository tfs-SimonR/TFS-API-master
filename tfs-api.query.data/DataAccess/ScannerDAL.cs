using Serilog;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using tfs_api.query.data.DataAccess.Base;
using tfs_api.query.data.Models;

namespace tfs_api.query.data.DataAccess
{
    public class ScannerDAL: IDisposable
    {
        #region Disposal 

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing)
            {
            }
        }

        #endregion

        private readonly DataHelper _dataHelper;

        public delegate void ReturnDalErrorHandler(string methodName, Exception exception);

        public event ReturnDalErrorHandler ReturnDalError = delegate { };

        public ScannerDAL(string connectionString) {
            _dataHelper = new DataHelper(connectionString);
        }


        public DataTable GetProductInfo(string id)
        {
            var sql = $"EXEC sp_GetProductDetails {id}";
            return _dataHelper.GetDataTable(sql);
        }


        /// <summary>
        /// Gets Warehousedeliveries by store ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetWareHouseDeliveriesByStore(int id)
        {
            var sql = $"EXEC sp_WareHouseDeliveries {id}";
            return _dataHelper.GetDataTable(sql);
        }

        public DataTable GetInterStoreList(int id)
        {
            var sql = $"EXEC sp_InterStoreDeliveries {id}";
            return _dataHelper.GetDataTable(sql);
        }

        public DataTable GetInterStoreListStoresCentral(int id)
        {
            var sql = $"EXEC [sp_InterStoreDeliveriesStoresCentral] {id}";
            return _dataHelper.GetDataTable(sql);
        }

        public DataTable GetInterStoreDetail(string id)
        {
            var sql = $"EXEC [sp_InterStoreDetail] {id}";
            return _dataHelper.GetDataTable(sql);
        }


        public DataTable GetPalletById(string id)
        {
            var sql = $"EXEC [sp_GetPallet] {id}";
            return _dataHelper.GetDataTable(sql);
        }

        public DataTable GetStores()
        {
            var sql = $"EXEC [sp_GetStoreDetails]";
            return _dataHelper.GetDataTable(sql);
        }

        public DataTable GetStoresById(string id)
        {
            var sql = $"EXEC [sp_GetStoreById] {id}";
            return _dataHelper.GetDataTable(sql);
        }

        public DataTable GetRTWCodes()
        {
            var sql = $"EXEC [sp_GetRTW_Codes]";
            return _dataHelper.GetDataTable(sql);
        }

        public DataTable GetNextGDO()
        {
            var sql = $"EXEC [sp_GetGDO]";
            return _dataHelper.GetDataTable(sql);
        }

        public bool InsertShipment(WarehouseDto goodsoutDetail)
        {
            var build = new StringBuilder();
            build.Append($"'{goodsoutDetail.Pallet_Number}', ");
            build.Append($"'{goodsoutDetail.Destination_Id}', ");
            build.Append($"'{goodsoutDetail.Destination_Name}', ");
            build.Append($"'{goodsoutDetail.Source_Id}', ");
            build.Append($"'{goodsoutDetail.SourceName}', ");
            build.Append($"'{goodsoutDetail.SKU}', ");
            build.Append($"{goodsoutDetail.Qty}, ");
            build.Append($"'{goodsoutDetail.Description}' ");
            var sql = $"EXEC [sp_InsertGoodsOut] {build}";
            try
            {
                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (SqlException sex)
            {
                Log.Error(sex, "SQL Error, ReturnDal.InsertShipment");
                return false;
            }
            catch (Exception exception)
            {
                Log.Error(exception, "ReturnDal.InsertShipment");
                return false;
            }
        }

        public long InsertGdo(GdoDto returnDetail)
        {

            StringBuilder build = new StringBuilder();
            build.Append($"'{returnDetail.GeneratedGDO}', ");
            build.Append($"'{returnDetail.StoreDestination}', ");
            build.Append($"'{returnDetail.StoreSource}', ");
            build.Append($"{returnDetail.isUSed}, ");
            var sql = $"EXEC [sp_UpdateGdo] {build}";

            try
            {
                return (long)_dataHelper.ExecuteScalar<decimal>(sql);
            }
            catch (SqlException sex)
            {
                Log.Error(sex, "SQL Error, ReturnDal.InsertGdo");
                return -1;
            }
            catch (Exception exception)
            {
                Log.Error(exception, "ReturnDal.InsertGdo");
                return -1;
            }
        }


        public bool UpdateGdo(GdoDto returnDetail)
        {
            var sql = string.Empty;
            try
            {
                StringBuilder build = new StringBuilder();
                build.Append($"'{returnDetail.RandomValue}', ");
                build.Append($"'{returnDetail.GeneratedGDO}', ");
                build.Append($"{returnDetail.StoreDestination}, ");
                build.Append($"{returnDetail.StoreSource} ");
                sql = $"EXEC sp_UpdateGdo {build}";
                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (SqlException sex)
            {
                Log.Error(sex, "SQL Error, ReturnDal.UpdateReturn", sql);
                return false;
            }
            catch (Exception exception)
            {
                Log.Error(exception, "ReturnDal.UpdateReturn", sql);
                return false;
            }
        }


        //This updates the PALLET to say its been delivered so it will no longer show on the scanner
        public bool UpdatePallet(WarehouseDto palletDetail)
        {
            var sql = string.Empty;
            try
            {
                StringBuilder build = new StringBuilder();
                build.Append($"{palletDetail.Pallet_Number}, ");
                build.Append($"'{palletDetail.IsDelivered}' ");
                sql = $"EXEC sp_UpdatePallet {build}";
                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (SqlException sex)
            {
                Log.Error(sex, "SQL Error, ScannerDal.UpdatePallet", sql);
                return false;
            }
            catch (Exception exception)
            {
                Log.Error(exception, "ScannerDal.UpdatePallet", sql);
                return false;
            }
        }


    }
}
