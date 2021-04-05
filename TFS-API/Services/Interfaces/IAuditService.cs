using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuditDataAccessLayer;
using Mi9DataAccessLayer;
using TFS_API.Models;
using TFS_API.Models.PostViewModels;
using TFS_API.Models.Tables;

namespace TFS_API.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuditService
    {
        tbl_writeoff_audit WriteoffAudit(string userid, string storeid, string sku,
            int varint, int writeofftotal, decimal cost, string reasonCode, string description,
            decimal? retailPrice, string reasoncodeDescription);

        tbl_SupplierMassAccept_Audit MassAcceptAudit(purchorder purchorder, string storeid, string userid,
            int varint);
        tbl_WarehouseToStore_Audit HandyScannerWarehouseToStoreAudit(consighdr consighdr, WarehouseAcceptPM pm);

        tbl_InterStore_Audit HandyScannerISTStockPutAudit(consighdr consighdr, string sku, int sku_qty, int sku_exp_qty,
            string userId, string sourcestoreId, string deststoreid);
        tbl_ADHOCCount_Audit AuditADHOCCount(brnstock model, string sku, int sku_counted, string userId, string storeId,
           DateTime dateTime, int varint, decimal costprice);

        tbl_goodsout_audit GoodsOutAudit(string userId, string storeId, string sku, string gdostring, string sku_counted, string destination);

        tbl_SupplierToStore_Audit HandyScannerSupplierToStoreAudit(purchorder purchorder, string userId, string storeId,
            string sku, int sku_qty, int sku_exp_qty);

        tbl_PICount_Audit PiCountAudit(brnstock model, string sku, int sku_counted_qty, int sku_expected_qty,
            string userid,
            string storeid, int listid, DateTime scanDate);
    }
}