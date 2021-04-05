using AuditDataAccessLayer;
using EPOSDataAccess;
using Mi9DataAccessLayer;
using ProcessedDAL;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TFS_API.BusinessLogic;
using TFS_API.BusinessObjects;
using TFS_API.Models;
using TFS_API.Models.ViewModels;
using TFS_API.Service.Interface;
using TFS_API.Services;
using TFS_API.Services.Interfaces;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;

namespace TFS_API.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/v1/HandyScannerGoodsOut")]
    public class HandyScannerGoodsOutController : ApiController
    {
        #region Setup
        /*Setup DB context*/
        public readonly ApplicationDbContext _db = new ApplicationDbContext();
        public readonly Mi9DBEntities mi9db = new Mi9DBEntities();
        private readonly auditEntities _auditdb = new auditEntities();
        private readonly EPOSDataConnection _gdodb = new EPOSDataConnection();
        private readonly ProDbContext proDb = new ProDbContext();
        private readonly IHandyScannerService _handyScannerService;
        private readonly IExceptionService _errorService;
        private readonly IAuditService _auditService;
        private readonly IStoredProcedureService _storedProcedureService;
        private readonly ICountingService _countingService;
        private readonly ICreateFileService _createFileService;
        private readonly IScannerService _scannerService;
        #endregion

        #region Ctor
        /// <inheritdoc />
        public HandyScannerGoodsOutController(
            IHandyScannerService handyScannerService,
            IExceptionService errorService,
            IAuditService auditService,
            IStoredProcedureService storedProcedureService,
            ICountingService countingService,
            ICreateFileService createFileService,
            IScannerService scannerService)
        {
            _errorService = errorService;
            _handyScannerService = handyScannerService;
            _auditService = auditService;
            _storedProcedureService = storedProcedureService;
            _countingService = countingService;
            _createFileService = createFileService;
            _scannerService = scannerService;
        }
        #endregion

        #region Reason and Stores Lists
        /// <summary>
        /// Endpoint for returning list of stores and store Id's
        /// </summary>
        /// <returns></returns>
        [Route("storeslist")]
        [HttpGet]
        public IHttpActionResult GetStoresList() {
            try {
                var result = _scannerService.GetStores();
                return result != null ? Ok(result) : (IHttpActionResult)NotFound();
            }
            catch (Exception ex) {
                Log.Error(ex, "An error occured in GetStoresList");
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Reason Code List
        /// </summary>
        /// <returns></returns>
        [Route("reasoncodelist")]
        public IHttpActionResult GetResponseCodes() { 
            try {
                var result = _scannerService.GetRTWCodes();
                return result != null ? Ok(result) : (IHttpActionResult) NotFound();
            }catch (Exception ex) {
                Log.Error(ex, "An error occured in GetResponseCodes");
                return InternalServerError(ex);
            }
        }


        /// <summary>
        /// List of supplier names, shortnames and supplier codes.
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetSuppliers()
        {
            try {
                var getSuppliers = _handyScannerService.GetSupplierList();
                return Request.CreateResponse(HttpStatusCode.Accepted, getSuppliers);
            }
            catch (Exception e)
            {
                _errorService.Capture(e);
                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, e.InnerException);
            }
        }

        
        #endregion

        [Route("getseasoncodefromsku/{sku}")]
        public HttpResponseMessage GetSeasonCode(string sku)
        {
            try
            {
                var getSeasonCode = (from productcodes in mi9db.productcodes
                    join prodcuts in mi9db.products on productcodes.prodint equals prodcuts.prodint
                    where productcodes.variantcode == sku
                    select new
                    {
                        sku = productcodes.variantcode,
                        description = prodcuts.proddesc,
                        seasoncode = prodcuts.introseason

                    }).FirstOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, getSeasonCode);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "SKU " + sku + " Not found");
            }
        }


        /// <summary>
        /// Creates Goods out from a Store, this will create IST only (Back to warehouse is IST also,
        /// We dont create return to suppliers as this goes back to the Warehouse)
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        #if DEBUG
        #else
        [System.Web.Http.Authorize]
        #endif
        [Route("goodsout")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostGoodsOut([FromBody] List<CreateISTDTO> vm) {
            try {
                if (vm.Any(item => item.store_id == "000")) {
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "STORE is 000");
                }

                Log.Information("goodsout " + vm.First().store_id);

                #region GDO Things
                var getGdo = _scannerService.GetGdo().First();
                if (getGdo == null)
                    throw new ArgumentNullException(nameof(getGdo));
                var storeid = vm.First().store_id;
                var createGdoString = $"GDO{storeid}{getGdo.RandomValue}";
                createGdoString = createGdoString.Substring(0, createGdoString.Length - 2);
                getGdo.isUSed = true;
                getGdo.StoreSource = vm.First().store_id;
                getGdo.StoreDestination = vm.First().destination_store_id;
                getGdo.GeneratedGDO = createGdoString;
                var updateGDO = new ScannerBo().UpdateGDOTable(getGdo);
                bool result = false;
                bool createFlatfile = false;
                #endregion
                
                foreach (var item in vm) {
                    
                    storeid = item.store_id;
                    if (item.user_name == "burnley")
                        storeid = "913";
                    var insertList = GoodsOutLogic.CreateWarehouseInsertList(vm, createGdoString);

                    foreach (var items in insertList)
                    {
                        Log.Information("Goods Out Info: " + "SKU " + items.SKU + " " + "Qty " + items.Qty );
                        Log.Information("Goods Out Info: " + "Description " + items.Description);
                    }
                    
                    Log.Information("goodsout " + item.gdo_number);
                    //Add the insertList objects to the table
                    if (insertList.Any())
                    {
                        foreach (var record in insertList)
                        {
                            result = new WarehouseBo().InsertGoodsOutPallet(record);
                        }
                        Log.Information("Creating FlatFile");
                        createFlatfile = GoodsOutLogic.CreateDataForFlatFile(item, storeid, createGdoString);
                    }
                   
                }
                if (updateGDO && result && createFlatfile)
                    return Request.CreateResponse(HttpStatusCode.Created, createGdoString);
                else
                    return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException.ToString());
                return Request.CreateResponse(HttpStatusCode.Created, "API Error Logged " + ex.InnerException);
            }
        }
        

        /// <summary>
        /// Returns a list of Goodsout Created in last 28 days from store.
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        [Route("goodsouthistory/{storeid}")]
        public HttpResponseMessage GetConsignhistory(string storeId)
        {
            try
            {
                Log.Information("goodsouthistory" + storeId);
                var getRecords = _handyScannerService.GetPreviousGoodsOut(storeId);

                if (getRecords.Count != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, getRecords);

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, getRecords);
                }
               
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "API Error Logged " + ex.InnerException);

            }

        }

    }
}
