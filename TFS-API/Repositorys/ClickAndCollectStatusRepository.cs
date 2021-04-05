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
    public class ClickAndCollectStatusRepository : IClickAndCollectStatusRepository
    {
        private UnitOfWork _unitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbFactory"></param>
        public ClickAndCollectStatusRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        public void UpdateReservationStatus(ClickAndCollectReservationStatu status)
        {
            _unitOfWork.CCDBContext.Entry(status).State = System.Data.Entity.EntityState.Modified;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        public ClickAndCollectReservationStatu GetReservationStatus(int reservationId)
        {
            return _unitOfWork.CCDBContext.ClickAndCollectReservationStatus.FirstOrDefault(r => r.ReservationID == reservationId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reservation"></param>
        public void Add(ClickAndCollectReservationStatu reservation)
        {
            _unitOfWork.CCDBContext.ClickAndCollectReservationStatus.Add(reservation);
        }
    }
}