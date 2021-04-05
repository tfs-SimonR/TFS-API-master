using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CashingUpDAL;
using TFS_API.Models.BindingModels;
using TFS_API.Repositorys.Interfaces;

namespace TFS_API.Repositorys
{
    public class CashRepository : ICashRepository, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private UnitOfWork _unitOfWork;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public CashRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="cashManagement"></param>
        public void Update(tbl_CashManagement cashManagement)
        {
            _unitOfWork.CashDbContext.Entry(cashManagement).State = EntityState.Modified;
        }
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="cashManagement"></param>
        public void Add(tbl_CashManagement cashManagement)
        {
            _unitOfWork.CashDbContext.tbl_CashManagement.Add(cashManagement);
        }

        public IQueryable<tbl_store_till_availabilty> GetSPQTills(string store)
        {
            return _unitOfWork.CashDbContext.tbl_store_till_availabilty.Where(x =>
                x.storeNumber == store && x.isSparq == true && x.isAlive == true);
        }

        public List<tbl_store_till_availabilty> GetRJTills(string store)
        {
            return _unitOfWork.CashDbContext.tbl_store_till_availabilty.Where(x =>
                x.storeNumber == store && x.isSparq == false && x.isAlive == true).ToList();
        }

        public bool isExistingSPQ(DateTime createdate, string storeid, int tillId)
        {
            return _unitOfWork.CashDbContext.tbl_CashManagement.Any(x =>
                x.StoreId == storeid && x.TradingDate == createdate
                                     && x.SPQTillId == tillId
                                     && x.tillisLive == true);
        }

        public bool isExistingRJ(DateTime createdate, string storeid, int tillId)
        {
            return _unitOfWork.CashDbContext.tbl_CashManagement.Any(x =>
                x.StoreId == storeid && x.TradingDate == createdate
                                     && x.RJTillId == tillId
                                     && x.tillisLive == true);
        }

        public List<CashManagmentModel> GetRecordsToList(DateTime thisWeek, DateTime endofdayToday, string store)
        {
            List<CashManagmentModel> list = new List<CashManagmentModel>();
            foreach (CashManagmentModel model in (_unitOfWork.CashDbContext.tbl_CashManagement.Where(a => a.TradingDate >= thisWeek && a.TradingDate <= endofdayToday && a.StoreId == store && a.isSparq == true)
            .Select(a => new CashManagmentModel
            {
                RowId = a.RowId,
                StoreId = a.StoreId,
                TradingDate = (DateTime) a.TradingDate,
                isClosed = (bool) a.isClosed,
                isSparq = (bool) a.isSparq
            })))
                list.Add(model);
            return new List<CashManagmentModel>(list.OrderBy(x => x.TradingDate));
        }

        public tbl_CashManagement GetTodaysCashRecord(DateTime today, string store, int tillId)
        {
            return _unitOfWork.CashDbContext.tbl_CashManagement
                .FirstOrDefault(x => x.TradingDate == today
                                     && x.StoreId == store
                                     && x.SPQTillId == tillId
                                     && x.isSparq == true);
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _unitOfWork.CashDbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}