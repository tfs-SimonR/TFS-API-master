using CashingUpDAL;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TFS_API.Helpers;
using TFS_API.Models.DTO;
using TFS_API.Repositorys;
using TFS_API.Services.Interfaces;

namespace TFS_API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/v1/cash_management")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CashAPIController : ApiController
    {
        private readonly IExceptionService _errorService;
        private readonly UnitOfWork _unitOfWork;
        static readonly UnitOfWork unitOfWork = new UnitOfWork();
        CashRepository _repository = new CashRepository(unitOfWork);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorService"></param>
        /// <param name="unitOfWork"></param>
        public CashAPIController(IExceptionService errorService,
        UnitOfWork unitOfWork)
        {
            _errorService = errorService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("PostEndOfDay")]
        [HttpPost]
        public HttpResponseMessage PostEndOfDay([FromBody]TillDataTransferDTO dto)
        {
            try
            {

                #region Date Variables
                var startDateTime = DateTime.Today; //Today at 00:00:00
                var endDateTime = DateTime.Today.AddDays(1).AddTicks(-1);
                var thisWeek = DateTime.Today.Date.AddDays(-7);
                var endofdayToday = DateTime.Today.AddDays(1).AddTicks(-1);
                var dateRangelist = DataHelpers.Range(thisWeek, endofdayToday);
                var dateTimes = dateRangelist.ToList();
                #endregion

                if (dto.storeId == null)
                {
                    var getIp = DataHelpers.GetIpAddress();
                    var thirdOctet = IPAddress.Parse(getIp).GetAddressBytes()[2];

                    var store = thirdOctet.ToString();
                    if (store.Length == 1) store = "00" + store;
                    if (store.Length == 2) store = "0" + store;

                    var getTills = _repository.GetSPQTills(store);

                    foreach (var till in getTills)
                    {
                        var getRecords = _repository.GetRecordsToList(thisWeek, endofdayToday, store);
                        var datesthatDontExist = dateTimes.Except(dateTimes.Where(o => getRecords.Select(s => s.TradingDate).ToList().Contains(o.Date))).ToList();
                        if (datesthatDontExist.Any())
                        {
                            foreach (var item in datesthatDontExist)
                            {
                                CreateCashingUpIfNotExist(item, store);
                            }
                            var getRecord = _repository.GetTodaysCashRecord(startDateTime, store, (int)till.TillId);

                            getRecord.expectedAmount = 0.00m;
                            getRecord.CreditCardTotal = 0.00m;
                            getRecord.Expense = 0.00m;
                            getRecord.Love2Shop = 0.00m;
                            getRecord.Income = 0.00m;
                            getRecord.netSales = 0.00m;
                            getRecord.LSVexpectedAmount = 0.00m;
                            getRecord.DeliverySales = 0.00m;
                            getRecord.GiftVoucherTotal = 0.00m;
                            getRecord.isEndOfDay = true;
                            using (var unitOfWork = new UnitOfWork())
                            {
                                var cashRepository = new CashRepository(unitOfWork);
                                cashRepository.Update(getRecord);
                                unitOfWork.SaveChanges();
                            }

                        }
                        else
                        {
                            var getRecord = _repository.GetTodaysCashRecord(startDateTime, store, (int)till.TillId);

                            getRecord.expectedAmount = 0.00m;
                            getRecord.CreditCardTotal = 0.00m;
                            getRecord.Expense = 0.00m;
                            getRecord.Love2Shop = 0.00m;
                            getRecord.Income = 0.00m;
                            getRecord.netSales = 0.00m;
                            getRecord.LSVexpectedAmount = 0.00m;
                            getRecord.DeliverySales = 0.00m;
                            getRecord.GiftVoucherTotal = 0.00m;
                            getRecord.isEndOfDay = true;
                            using (var unitOfWork = new UnitOfWork())
                            {
                                var cashRepository = new CashRepository(unitOfWork);
                                cashRepository.Update(getRecord);
                                unitOfWork.SaveChanges();
                            }

                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                var getSparqRecords = _repository.GetRecordsToList(thisWeek, endofdayToday, dto.storeId);
                var datesDontExist = dateTimes.Except(dateTimes.Where(o => getSparqRecords.Select(s => s.TradingDate).ToList().Contains(o.Date))).ToList();
                if (datesDontExist.Any())
                {
                    foreach (var item in datesDontExist)
                    {
                        CreateCashingUpIfNotExist(item, dto.storeId);
                    }
                    var getRecord = _repository.GetTodaysCashRecord(startDateTime, dto.storeId, dto.tillId);

                    getRecord.expectedAmount = dto.CashAmountExpected - dto.ExpensesTotal;
                    getRecord.CreditCardTotal = dto.CreditCard;
                    getRecord.Expense = dto.ExpensesTotal;
                    getRecord.Love2Shop = 0.00m;
                    getRecord.Income = dto.IncomeExpected;
                    getRecord.netSales = dto.netSales;
                    getRecord.LSVexpectedAmount = dto.LSVExpected;
                    getRecord.DeliverySales = dto.delivery_sales;
                    getRecord.GiftVoucherTotal = dto.gift_card;
                    getRecord.isEndOfDay = true;
                    using (var unitOfWork = new UnitOfWork())
                    {
                        var cashRepository = new CashRepository(unitOfWork);
                        cashRepository.Update(getRecord);
                        unitOfWork.SaveChanges();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    var getRecord = _repository.GetTodaysCashRecord(startDateTime, dto.storeId, dto.tillId);

                    getRecord.expectedAmount = dto.CashAmountExpected - dto.ExpensesTotal;
                    getRecord.CreditCardTotal = dto.CreditCard;
                    getRecord.Expense = dto.ExpensesTotal;
                    getRecord.Love2Shop = 0.00m;
                    getRecord.Income = dto.IncomeExpected;
                    getRecord.netSales = dto.netSales;
                    getRecord.LSVexpectedAmount = dto.LSVExpected;
                    getRecord.DeliverySales = dto.delivery_sales;
                    getRecord.GiftVoucherTotal = dto.gift_card;
                    getRecord.isEndOfDay = true;
                    using (var unitOfWork = new UnitOfWork())
                    {
                        var cashRepository = new CashRepository(unitOfWork);
                        cashRepository.Update(getRecord);
                        unitOfWork.SaveChanges();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createdate"></param>
        /// <param name="store"></param>
        public void CreateCashingUpIfNotExist(DateTime createdate, string store)
        {
            try
            {
                #region GetTilLDetails
                var getSpqActiveTills = _repository.GetSPQTills(store);
                var getRJActiveTills = _repository.GetRJTills(store);
                #endregion

                using (var unitOfWork = new UnitOfWork())
                {
                    var cashRepository = new CashRepository(unitOfWork);
                    foreach (var spq in getSpqActiveTills)
                    {
                        var dataModel = new tbl_CashManagement
                        {
                            StoreId = store,
                            SPQTillId = (int)spq.TillId,
                            TradingDate = createdate,
                            TransactionDate = createdate,
                            ClosingFloat = (decimal)spq.TillFloat,
                            CashAmount = 0.00m,
                            GiftVoucherTotal = 0.00m,
                            CreditCardTotal = 0.00m,
                            Love2Shop = 0.00m,
                            Income = 0.00m,
                            Expense = 0.00m,
                            isLocked = false,
                            tillisLive = true,
                            isClosed = false,
                            isSparq = true,
                            isEndOfDay = false

                        };
                        var checkifTodayExists = _repository.isExistingSPQ(createdate, store, dataModel.SPQTillId);
                        if (!checkifTodayExists)
                        {
                            cashRepository.Add(dataModel);
                            unitOfWork.SaveChanges();
                        }

                    }

                    foreach (var rj in getRJActiveTills)
                    {
                        #region Models and DB Actions
                        var dataModel = new tbl_CashManagement
                        {
                            StoreId = store,
                            RJTillId = (int)rj.TillId,
                            TradingDate = createdate,
                            TransactionDate = createdate,
                            ClosingFloat = (decimal)rj.TillFloat,
                            CashAmount = 0.00m,
                            GiftVoucherTotal = 0.00m,
                            CreditCardTotal = 0.00m,
                            Love2Shop = 0.00m,
                            Income = 0.00m,
                            Expense = 0.00m,
                            isLocked = false,
                            tillisLive = true,
                            isClosed = false,
                            isSparq = false,
                            isEndOfDay = false,
                            DeliverySales = 0.00m
                        };

                        var checkifTodayExists = _repository.isExistingRJ(createdate, store, dataModel.RJTillId);

                        if (!checkifTodayExists)
                        {
                            cashRepository.Add(dataModel);
                            unitOfWork.SaveChanges();
                        }
                        #endregion
                    }
                   
                }
                
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }

        }
    }
}
