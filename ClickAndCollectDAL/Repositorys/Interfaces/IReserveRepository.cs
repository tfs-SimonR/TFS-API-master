using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickAndCollectDAL.Repositorys.Interfaces
{
    public interface IReserveRepository : IDisposable
    {
        IEnumerable<ClickAndCollectReservation> GetReservations();
        void AddReservation(ClickAndCollectReservation reservation);
    }
}
