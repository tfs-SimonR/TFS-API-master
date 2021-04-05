using AuditDataAccessLayer;
using Mi9DataAccessLayer;
using mi9TESTDAL;
using PICountDataAccessLayer;
using ProcessedDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TFS_API.Helpers;
using TFS_API.Models;
using TFS_API.Models.DTO;
using TFS_API.Models.Tables;
using TFS_API.Repositorys;
using TFS_API.Services;
using TFS_API.Services.Interfaces;

namespace TFS_API.Controllers
{

    /// <summary>
    /// Counting Stuff
    /// </summary>
    [RoutePrefix("api/v1/Counting")]
    public class PICountController : ApiController
    {
        #region Setup
        /*Setup DB context*/
        public readonly ApplicationDbContext _db = new ApplicationDbContext();
        public readonly Mi9DBEntities mi9db = new Mi9DBEntities();
        private readonly mi9_mms_fs_trainEntities _mi9TestEntities;
        private readonly auditEntities _auditdb = new auditEntities();
        private readonly piDBContext _piDbContext = new piDBContext();
        private readonly ProDbContext _holdDB = new ProDbContext();
        private readonly IHandyScannerService _handyScannerService;
        private readonly IExceptionService _errorService;
        private readonly IAuditService _auditService;
        private readonly ICountingService _countingService;
        private readonly ICreateFileService _createFileService;
        private decimal _total_weighted_average;
        private decimal _unit_cost_average;
        private readonly UnitOfWork _unitOfWork;
        static readonly UnitOfWork unitOfWork = new UnitOfWork();
        private PIRepository _piRepository = new PIRepository(unitOfWork);
        private ProductsRepository _productsRepository = new ProductsRepository(unitOfWork);
        private StockRepository _stockRepository  = new StockRepository(unitOfWork);
        private PricingRepository _pricingRepository = new PricingRepository(unitOfWork);
        #endregion

        /// <inheritdoc />
        public PICountController(
            IHandyScannerService handyScannerService,
            IExceptionService errorService,
            IAuditService auditService,
            ICountingService countingService,
            ICreateFileService createFileService,
            mi9_mms_fs_trainEntities mi9TestEntities)
        {
            _handyScannerService = handyScannerService;
            _errorService = errorService;
            _auditService = auditService;
            _countingService = countingService;
            _mi9TestEntities = mi9TestEntities;
            _createFileService = createFileService;
        }

        /// <summary>
        /// Returns list of profit protection write off / on reason codes.
        /// </summary>
        /// <returns></returns>
        #if DEBUG
#else
        [System.Web.Http.Authorize]
#endif
        [Route("reasoncodelist")]
        public HttpResponseMessage GetPPReasonCodes()
        {
            try
            {
                var reasonList = new List<PPReasonCodes>
                {
                    new PPReasonCodes {id = 1, reason_code = "SA", description = "Write Off - Damaged"},
                    new PPReasonCodes {id = 2, reason_code = "SB", description = "Write Off - Theft"},
                    new PPReasonCodes {id = 3, reason_code = "SC", description = "Write Off - Damaged on Delivery"},
                    new PPReasonCodes {id = 4, reason_code = "SD", description = "Write Off - Out Of Date"},
                    new PPReasonCodes {id = 5, reason_code = "SF", description = "Write Off - Head Office"},
                    new PPReasonCodes {id = 6, reason_code = "SI", description = "Write Off - Own Store Use"},
                    new PPReasonCodes {id = 7, reason_code = "SJ", description = "Write Off - Odd Shoes"},
                    new PPReasonCodes {id = 8, reason_code = "SM", description = "Write Off - Colleague Uniform"},
                    new PPReasonCodes {id = 9, reason_code = "SN", description = "Write Off - Delivery Discrepancy"},
                    new PPReasonCodes {id = 10, reason_code = "SY" ,description = "Write On - Delivery Discrepancy"}
                    
                };

                return Request.CreateResponse(HttpStatusCode.Accepted, reasonList);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException);

            }
        }


        /// <summary>
        /// Write off endpoint for profit protection
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        #if DEBUG
#else
        [System.Web.Http.Authorize]
#endif
        [Route("productwriteoff")]
        public async Task<HttpResponseMessage> PutWriteOff([FromBody] WriteOffDTO viewModel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model state not valid.");
            }

            try
            {
                #region Variables
                string _store = null;
                string _storename = null;
                string _userid = null;
                string _sku = null;
                string _reasoncodestring = null;
                string _reasoncodeDescription = null;
                string _description = null;
                decimal? _retailPrice = null;
                decimal _costPrice = 0;
                int varint = 0;
                int qty = 0;
                #endregion
                _reasoncodeDescription = DataHelpers.GetReasonCodeDescription(viewModel.reason_code);
                //New method that saves to the db before hand, then the app runs at 19:30 called "flatfilegenerator.exe" on SQLLOG1
                CreateUpdateWriteOff(viewModel);
               
                if (viewModel.user_id != "pricing")
                {
                    foreach (var item in viewModel.write_off_items)
                    {
                        var getProdint = DataHelpers.GetProdInt(item.sku);
                        var getDescription = DataHelpers.GetProductDescription(getProdint);
                        if (getDescription != null)
                            _description = getDescription;
                        var getRetailPrice = DataHelpers.GetRetailPrice(getProdint);
                        if (getRetailPrice != null)
                            _retailPrice = getRetailPrice;

                        var audit = _auditService.WriteoffAudit(viewModel.user_id, viewModel.store_id, item.sku, varint,
                            item.sku_counted_qty, _unit_cost_average, viewModel.reason_code, _description, _retailPrice, _reasoncodeDescription);
                        _auditdb.tbl_writeoff_audit.Add(audit);
                        await _auditdb.SaveChangesAsync();
                    }

                   
                }
                return Request.CreateResponse(HttpStatusCode.OK, viewModel.store_id);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "API Error Logged", ex.InnerException);
            }
        }

        private static void CreateUpdateWriteOff(WriteOffDTO viewModel)
        {
            using (var uow = new UnitOfWork())
            {
                var repository = new ProductCountRepository(uow);
                foreach (var item in viewModel.write_off_items)
                {
                    //Checks to see if there is already that SKU with the same reason code, if there is we need to add the qty's together
                    var checkExistsToday = repository.GetBySkuStoreWOCode(viewModel.store_id, item.sku, viewModel.reason_code);
                    if (checkExistsToday == null)
                    {
                        var model = new tbl_product_count();
                        model.store_id = viewModel.store_id;
                        model.dateTime_created = DateTime.Now;
                        model.sku = item.sku;
                        model.sku_counted_qty = item.sku_counted_qty;
                        model.list_name = "Write Off";
                        model.user_id = viewModel.user_id ?? "No User";
                        model.reason_code = viewModel.reason_code;
                        model.isWriteOff = true;
                        model.isAdHoc = false;
                        model.isPiCount = false;
                        model.isCompleted = false;
                        repository.Add(model);
                        uow.SaveChanges();
                    }
                    else
                    {
                        var getRecord = repository.GetById(checkExistsToday.RowId);
                        getRecord.store_id = viewModel.store_id;
                        getRecord.dateTime_created = DateTime.Now;
                        getRecord.sku = item.sku;
                        getRecord.sku_counted_qty = getRecord.sku_counted_qty + item.sku_counted_qty;
                        getRecord.list_name = "Write Off";
                        getRecord.user_id = viewModel.user_id ?? "No User";
                        getRecord.reason_code = viewModel.reason_code;
                        getRecord.isWriteOff = true;
                        getRecord.isAdHoc = false;
                        getRecord.isPiCount = false;
                        getRecord.isCompleted = false;
                        repository.Update(getRecord);
                        uow.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the list of PI counts awaiting the store
        /// </summary>
        /// <param name="storeid"></param>
        /// <returns></returns>
#if DEBUG
#else
        [Authorize]
#endif
        [Route("getpicountlist/{storeId}")]
        public HttpResponseMessage GetPIList(string storeid)
        {
            try
            {
                var getCountList = (from list in _piDbContext.tbl_pi_count
                    join stores in _piDbContext.tbl_pi_count_stores on list.rowid equals stores.list_id
                    join skulist in _piDbContext.tbl_sku_count on stores.sku_list_id equals skulist.rowId
                    where stores.storeid == storeid && list.isDeleted == false && stores.isCompleted == false
                    group new {list, stores, skulist} by list.list_name
                    into grp
                    select new PICountVMDTO
                    {
                        list_id = grp.Select(x => x.list.rowid).FirstOrDefault(),
                        expected_date = (DateTime) grp.Select(x => x.list.expexted_count_date).FirstOrDefault(),
                        list_name = grp.Key,
                        counted_items = grp.Select(x => new PICountVMDTO.PICountItems
                        {
                            sku = x.skulist.variantcode, 
                            sku_expected_qty = (int) x.skulist.expected_qty

                        }).ToList()
                    }).ToList();

                return Request.CreateResponse(HttpStatusCode.Accepted, getCountList);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "API Error Logged", ex.InnerException);
            }
        }

        /// <summary>
        /// ADHOC Counting Submit Method
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        #if DEBUG
        #else
        [Authorize]
        #endif
        [Route("adhoccountsubmit")]
        public async Task<HttpResponseMessage> PutAdHoc([FromBody] List<AdHocDTO> viewModel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model state not valid.");
            }
            try
            {
                #region Variables

                #endregion
            
                using (var unitOfWork = new UnitOfWork())
                {
                    var repository = new ProductCountRepository(unitOfWork);

                    foreach (var item in viewModel)
                    foreach (var productinfo in item.counted_items)
                    {
                        var checkifalreadychanged = repository.GetBySkuAndStores(item.store_id, productinfo.sku);

                        if (checkifalreadychanged != null)
                        {
                            var getbyId = repository.GetById(checkifalreadychanged.RowId);

                            getbyId.store_id = item.store_id;
                            getbyId.dateTime_created = DateTime.Now;
                            getbyId.sku = productinfo.sku;
                            getbyId.sku_counted_qty = productinfo.sku_counted_qty;
                            getbyId.list_name = "Ad-Hoc";
                            getbyId.user_id = item.user_id ?? "No User";
                            getbyId.reason_code = "PP";
                            getbyId.isWriteOff = false;
                            getbyId.isAdHoc = true;
                            getbyId.isPiCount = false;
                            getbyId.isCompleted = false;

                            repository.Update(getbyId);
                            unitOfWork.SaveChanges();
                        }
                        else
                        {
                            var model = new tbl_product_count
                            {
                                store_id = item.store_id,
                                dateTime_created = DateTime.Now,
                                sku = productinfo.sku,
                                sku_counted_qty = productinfo.sku_counted_qty,
                                list_name = "Ad-Hoc",
                                user_id = item.user_id ?? "No User",
                                reason_code = "PP",
                                isWriteOff = false,
                                isAdHoc = true,
                                isPiCount = false,
                                isCompleted = false
                            };
                            repository.Add(model);
                            unitOfWork.SaveChanges();

                        }
                    }
                }


                return Request.CreateResponse(HttpStatusCode.OK);


            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "API Error Logged", ex.InnerException);
            }
        }

        /// <summary>
        /// PI Counting Submit Method
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>

        #if DEBUG
        #else
        [Authorize]
        #endif
        [Route("picountsubmit")]
        public async Task<HttpResponseMessage> PutPiCount([FromBody] List<PICountVMDTO> viewModel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model state not valid.");
            }

            try
            {
                string store;
                var flatfile = new List<string>();
                foreach (var item in viewModel)
                {
                    var date = DateTime.Now.Date.ToString("ddMMyy");
                    var time = DateTime.Now.ToString("hhmm");
                    /*MA <Storeid> 010000 <StaffId> <date> <time> */
                    var line1 = "MA" + item.store_id + "010000" + "0002" + date + time + "PP";
                    store = item.store_id;
                    flatfile.Add(line1);
                    var delta = 0.00m;
                    foreach (var listitem in item.counted_items)
                    {
                        var getcostPrice = _pricingRepository.GetCostPrice(listitem.sku);
                        var _costPrice = getcostPrice != null ? getcostPrice.Cost : 0;

                        var getVarInt = _productsRepository.GetVarint(listitem.sku);
                        var getbrnStock = _stockRepository.GetStockInformtaionAtStore(getVarInt.varint, item.store_id);
                        
                        var getProdint = _productsRepository.GetProdint(listitem.sku);
                        
                        var getRetailPrice = _pricingRepository.GetCostPrice(listitem.sku);
                        var _retailPrice = getRetailPrice != null ? getRetailPrice.Cost : 0;

                        var totalValue = Math.Round((decimal) (listitem.sku_counted_qty * _retailPrice), 2);

                        var calcDif = DataHelpers.FindDifference((int)getbrnStock.retail, listitem.sku_counted_qty);
                        var intToString = DataHelpers.replaceLastFour(totalValue.ToString());

                        string lines;
                        if (getbrnStock.retail > listitem.sku_counted_qty)
                        {
                            delta = calcDif;
                            var returnstringcharEight = DataHelpers.setQuantity(delta.ToString());
                            if (listitem.sku.Length <= 6)
                                lines = string.Format("MV" + listitem.sku + "  " + intToString + "-" + returnstringcharEight + "-");
                            else
                                lines = string.Format("MV" + listitem.sku + intToString + "-" + returnstringcharEight + "-");
                            flatfile.Add(lines);
                        }
                        if (getbrnStock.retail < listitem.sku_counted_qty)
                        {
                            delta = calcDif;
                            var returnstringcharEight = DataHelpers.setQuantity(delta.ToString());
                            if (listitem.sku.Length <= 6)
                                /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                                lines = string.Format("MV" + listitem.sku + "  " + intToString + " " + returnstringcharEight);
                            else
                                /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                                lines = string.Format("MV" + listitem.sku + intToString + " " + returnstringcharEight);
                            flatfile.Add(lines);
                        }

                        _createFileService.CreateFile(store, flatfile);
                        
                    }

                    using (var unitOfWork = new UnitOfWork())
                    {
                        var repository = new ProductCountRepository(unitOfWork);

                        foreach (var items in viewModel)
                            foreach (var productinfo in items.counted_items)
                            {
                                var checkifalreadychanged = repository.GetBySkuAndStores(items.store_id, productinfo.sku);

                                if (checkifalreadychanged != null)
                                {
                                    var getbyId = repository.GetById(checkifalreadychanged.RowId);

                                    getbyId.store_id = items.store_id;
                                    getbyId.dateTime_created = DateTime.Now;
                                    getbyId.sku = productinfo.sku;
                                    getbyId.sku_counted_qty = productinfo.sku_counted_qty;
                                    getbyId.list_name = "PI-Count";
                                    getbyId.user_id = items.user_name ?? "No User";
                                    getbyId.reason_code = "PP";
                                    getbyId.isWriteOff = false;
                                    getbyId.isAdHoc = true;
                                    getbyId.isPiCount = false;
                                    getbyId.isCompleted = false;

                                    repository.Update(getbyId);
                                    unitOfWork.SaveChanges();
                                }
                                else
                                {
                                    var model = new tbl_product_count
                                    {
                                        store_id = items.store_id,
                                        dateTime_created = DateTime.Now,
                                        sku = productinfo.sku,
                                        sku_counted_qty = productinfo.sku_counted_qty,
                                        list_name = "PI-Count",
                                        user_id = items.user_name ?? "No User",
                                        reason_code = "PP",
                                        isWriteOff = false,
                                        isAdHoc = true,
                                        isPiCount = false,
                                        isCompleted = false
                                    };
                                    repository.Add(model);
                                    unitOfWork.SaveChanges();

                                }
                            }
                    }

                    using (var UnitOFWork = new UnitOfWork())
                    {
                        var repo = new PIRepository(UnitOFWork);

                        var getCountDetails = repo.GetByID(item.list_id);

                        getCountDetails.isDeleted = true;
                        repo.Update(getCountDetails);
                        UnitOFWork.SaveChanges();
                    }

                }


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "API Error Logged", ex.InnerException);
            }

        }

    }
}
