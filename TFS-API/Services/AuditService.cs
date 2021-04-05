using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuditDataAccessLayer;
using Mi9DataAccessLayer;
using TFS_API.Helpers;
using TFS_API.Models;
using TFS_API.Models.PostViewModels;
using TFS_API.Models.Tables;
using TFS_API.Services.Interfaces;

namespace TFS_API.Services
{
    /// <inheritdoc />
    public class AuditService : IAuditService
    {
        private readonly IExceptionService _errorService;

        /// <inheritdoc />
        public AuditService(IExceptionService errorService)
        {
            _errorService = errorService;

        }

        public tbl_writeoff_audit WriteoffAudit(string userid, string storeid, string sku,
            int varint, int writeofftotal, decimal cost, string reasonCode, string description,
            decimal? retailPrice, string reasoncodeDescription)
        {
            try
            {
                var audit = new tbl_writeoff_audit
                {
                    DateofWriteOff = DateTime.Now,
                    userid = userid,
                    storeid = storeid,
                    sku = sku,
                    stockleveldifference = 0,
                    writeofftotalqty = writeofftotal,
                    costvalue = cost,
                    retailperitem = retailPrice,
                    retailvaluetotal = retailPrice * writeofftotal,
                    writeofftotalcost = (cost * writeofftotal),
                    eventdate = DateTime.Now,
                    reasonCode = reasonCode,
                    descriptions = description,
                    reasoncodeDescription = reasoncodeDescription
                };

                return audit;

            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }

        public tbl_SupplierMassAccept_Audit MassAcceptAudit(purchorder purchorder, string storeid, string userid,
            int varint)
        {
            try
            {
                var audit = new tbl_SupplierMassAccept_Audit
                {
                    ponumber = purchorder.ordernumber,
                    storeid = storeid,
                    userId = userid,
                    reason = "Accepted as mass delivery",
                    DateAccepted = DateTime.Now,
                    EventDate = DateTime.Now,
                    varint = varint
                };
                return audit;
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }
     
        /// <summary>
        /// Warehouse to Store Audit
        /// </summary>
        /// <param name="consighdr"></param>
        /// <param name="pm"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public tbl_WarehouseToStore_Audit HandyScannerWarehouseToStoreAudit(consighdr consighdr, WarehouseAcceptPM pm)
        {
            try
            {
                var audit = new tbl_WarehouseToStore_Audit
                {
                    consignment_number = consighdr.consignment,
                    UserId = pm.user_name,
                    StoreId = pm.storeId,
                    EventDate = DateTime.Now,
                    PriorStatusCode = consighdr.status,

                };
                return audit;

            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }

        /// <summary>
        /// STOCK QTY AUDIT 
        /// </summary>
        /// <param name="consighdr"></param>
        /// <param name="pm"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public tbl_SupplierToStore_Audit HandyScannerSupplierToStoreAudit(purchorder purchorder, string userId, string storeId, string sku, int sku_qty, int sku_exp_qty)
        {
            try
            {
                var stockadjustlevel = 0;
                if (sku_exp_qty != sku_qty)
                {
                    if (sku_exp_qty > sku_qty)
                    {
                        stockadjustlevel = Math.Abs(sku_qty - sku_exp_qty) * -1;
                    }
                    else
                    {
                        stockadjustlevel = Math.Abs(sku_qty - sku_exp_qty);
                    }
                }

                var audit = new tbl_SupplierToStore_Audit()
                {
                    DateCounted = DateTime.Now,
                    UserId = userId,
                    StoreId = storeId,
                    EventDate = DateTime.Now,
                    SKU = sku,
                    StockLevelDifference = stockadjustlevel,
                    ponumber = purchorder.ordernumber

                };
                return audit;
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }

        /// <summary>
        /// IST Audit
        /// </summary>
        /// <param name="consighdr"></param>
        /// <param name="sku"></param>
        /// <param name="sku_qty"></param>
        /// <param name="sku_exp_qty"></param>
        /// <param name="userId"></param>
        /// <param name="storeId"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public tbl_InterStore_Audit HandyScannerISTStockPutAudit(consighdr consighdr, string sku, int sku_qty, int sku_exp_qty, string userId, string sourcestoreId, string deststoreid)
        {
            try
            {
                //Get difference as a negative or positive from the difference between the expected and the actual counts.
                var stockadjustlevel = 0;
                if (sku_exp_qty != sku_qty)
                {
                    if (sku_exp_qty > sku_qty)
                    {
                        stockadjustlevel = Math.Abs(sku_qty - sku_exp_qty) * -1;
                    }
                    else
                    {
                        stockadjustlevel = Math.Abs(sku_qty - sku_exp_qty);
                    }
                }
                var audit = new tbl_InterStore_Audit
                {
                    consignment_number = consighdr.consignment,
                    UserId = userId,
                    source_storeid = sourcestoreId,
                    destination_StoreId = deststoreid,
                    EventDate = DateTime.Now,
                    PriorStatusCode = consighdr.status,
                    SKU = sku,
                    StockLevelDifference = stockadjustlevel,
                    InsertRecordId = consighdr.consignment
                };
                return audit;
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }

       /// <summary>
       /// ADHOC Counting Audit
       /// </summary>
       /// <param name="model"></param>
       /// <param name="sku"></param>
       /// <param name="sku_counted"></param>
       /// <param name="userId"></param>
       /// <param name="storeId"></param>
       /// <param name="dateTime"></param>
       /// <returns></returns>
       public tbl_ADHOCCount_Audit AuditADHOCCount(brnstock model, string sku, int sku_counted, string userId, string storeId,
           DateTime dateTime, int varint, decimal costprice)
       {
           try
           {
               var stockadjustlevel = 0;

               if (model.retail > sku_counted)
                   stockadjustlevel = (int) (Math.Abs(sku_counted -(decimal)model.retail) * -1);
               else
                   stockadjustlevel = (int) Math.Abs(sku_counted - (decimal)model.retail);

               var audit = new tbl_ADHOCCount_Audit()
               {
                   DateCounted = dateTime,
                   UserId = userId,
                   StoreId = storeId,
                   EventDate = DateTime.Now,
                   SKU = sku,
                   VarInt = varint,
                   StockLevelDifference = stockadjustlevel,
                   CostValue = costprice
               };
               return audit;
           }
           catch (Exception ex)
           {
               _errorService.Capture(ex);
               throw;
           }
       }

       /// <summary>
       /// PI Count Audit
       /// </summary>
       /// <param name="model"></param>
       /// <param name="sku"></param>
       /// <param name="sku_counted_qty"></param>
       /// <param name="sku_expected_qty"></param>
       /// <param name="userid"></param>
       /// <param name="storeid"></param>
       /// <param name="listid"></param>
       /// <param name="datescanned"></param>
       /// <returns></returns>
       public tbl_PICount_Audit PiCountAudit(brnstock model, string sku, int sku_counted_qty, int sku_expected_qty, string userid,
           string storeid, int listid, DateTime datescanned)
       {
            try
            {
                var stockadjustlevel = 0;

                if (model.retail > sku_counted_qty)
                    stockadjustlevel = (int)(Math.Abs(sku_counted_qty - (decimal) model.retail) * -1);
                else
                    stockadjustlevel = (int)Math.Abs(sku_counted_qty - (decimal) model.retail);

                var audit = new tbl_PICount_Audit
                {
                    DateCounted = datescanned,
                    PICount_ListId = listid,
                    UserId = userid,
                    StoreId = storeid,
                    EventDate = DateTime.Now,
                    SKU = sku,
                    StockLevelDifference = stockadjustlevel,
                };
                return audit;
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }

        }

       /// <summary>
       /// Goods Out Audit
       /// </summary>
       /// <param name="line"></param>
       /// <param name="userId"></param>
       /// <param name="storeId"></param>
       /// <param name="sku"></param>
       /// <param name="gdostring"></param>
       /// <param name="sku_counted"></param>
       /// <param name="destination"></param>
       /// <returns></returns>
       public tbl_goodsout_audit GoodsOutAudit(string userId, string storeId, string sku, string gdostring,
           string sku_counted, string destination)
       {
           try
           {
               var audit = new tbl_goodsout_audit()
               {
                   UserId = userId,
                   StoreId = storeId,
                   DestinationStoreId = destination,
                   EventDate = DateTime.Now,
                   sku_qty = int.Parse(sku_counted),
                   Sku = sku,
                   consignment = gdostring
                  
               };
               return audit;
           }
           catch (Exception ex)
           {
               _errorService.Capture(ex);
               throw;
           }
       }

    }
}