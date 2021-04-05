using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickAndCollectDAL.Repositorys.Interfaces
{
    public interface IReservationStatusRepository : IDisposable
    {
        IEnumerable<ClickAndCollectReservationStatu> GetAllReservationStatus();
        ClickAndCollectReservationStatu GetReservationStatus(int reservationId);
        void AddReservationStatus(ClickAndCollectReservationStatu reservation);
        void Save();
    }
}
