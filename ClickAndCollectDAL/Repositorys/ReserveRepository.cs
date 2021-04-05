using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClickAndCollectDAL.Repositorys.Interfaces;

namespace ClickAndCollectDAL.Repositorys 
{
    public class ReserveRepository: IReserveRepository, IDisposable
    {
        private CCDBContext _context;

        public ReserveRepository(CCDBContext context)
        {
            this._context = context;
        }

        public IEnumerable<ClickAndCollectReservation> GetReservations()
        {
            return _context.ClickAndCollectReservations.ToList();
        }

        public void AddReservation(ClickAndCollectReservation reservation)
        {
            _context.ClickAndCollectReservations.Add(reservation);
            _context.SaveChangesAsync();
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
