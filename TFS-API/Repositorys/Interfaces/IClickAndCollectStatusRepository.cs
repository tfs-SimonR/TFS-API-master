using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ClickAndCollectDAL;

namespace TFS_API.Repositorys.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClickAndCollectStatusRepository
    {
        void UpdateReservationStatus(ClickAndCollectReservationStatu status);

        ClickAndCollectReservationStatu GetReservationStatus(int reservationId);
    }
}