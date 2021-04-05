using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClickAndCollectDAL;
using TFS_API.Factories.IFactories;
using TFS_API.Repositorys.Interfaces;

namespace TFS_API.Repositorys
{
    /// <summary>
    /// 
    /// </summary>
    public class ClickAndCollectReserveRepository : IClickAndCollectReserveRepository, IDisposable
    {
        private UnitOfWork _unitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public ClickAndCollectReserveRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reservation"></param>
        public void Add(ClickAndCollectReservation reservation)
        {
            _unitOfWork.CCDBContext.ClickAndCollectReservations.Add(reservation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="update"></param>
        public void Update(ClickAndCollectReservation update)
        {
            _unitOfWork.CCDBContext.Entry(update).State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ClickAndCollectReservation> GetAll()
        {
            return _unitOfWork.CCDBContext.ClickAndCollectReservations.ToList();
        }



        /// <summary>
        /// 
        /// </summary>
        private bool disposed = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
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