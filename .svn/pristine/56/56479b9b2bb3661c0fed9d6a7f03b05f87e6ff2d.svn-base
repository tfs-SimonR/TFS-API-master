using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClickAndCollectDAL.Repositorys.Interfaces;

namespace ClickAndCollectDAL.Repositorys
{
    public class ReservationStatusRepository : IReservationStatusRepository, IDisposable
    {
        private readonly CCDBContext _context;
        public ReservationStatusRepository(CCDBContext context)
        {
            _context = context;
        }

        public IEnumerable<ClickAndCollectReservationStatu> GetAllReservationStatus()
        {
            return _context.ClickAndCollectReservationStatus.ToList();
        }

        public ClickAndCollectReservationStatu GetReservationStatus(int reservationId)
        {
            return _context.ClickAndCollectReservationStatus.FirstOrDefault(r => r.ReservationID == reservationId);
        }

        public void AddReservationStatus(ClickAndCollectReservationStatu reservation)
        {
            _context.ClickAndCollectReservationStatus.Add(reservation);
        }

        public void Save()
        {
            _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
