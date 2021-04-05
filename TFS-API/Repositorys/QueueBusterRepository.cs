using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SPARQTestDAL;
using TFS_API.Repositorys.Interfaces;

namespace TFS_API.Repositorys
{
    public class QueueBusterRepository : IQueueBusterRepository
    {
        UnitOfWork _unitOfWork;

        public QueueBusterRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<tbl_Sparq_QueueBuster_CardDetails> GetAllOpenBustedQueues()
        {
            return _unitOfWork.Context.tbl_Sparq_QueueBuster_CardDetails.Where(x => x.isOpen).ToList();
        }

        public tbl_Sparq_QueueBuster_CardDetails GetQueueByCardNumber(string cardNumber)
        {
            return _unitOfWork.Context.tbl_Sparq_QueueBuster_CardDetails.FirstOrDefault(x => x.cardnumber == cardNumber);
        }

        public tbl_Sparq_QueBuster_BasketDetails GetBasketInformation(string cardNumber)
        {
            return _unitOfWork.Context.tbl_Sparq_QueBuster_BasketDetails.FirstOrDefault(x => x.cardnumber == cardNumber);
        }
        public void InsertQueueBust(tbl_Sparq_QueueBuster_CardDetails queue)
        {
            _unitOfWork.Context.tbl_Sparq_QueueBuster_CardDetails.Add(queue);
        }

        public void InsertQueueBustBasketItems(tbl_Sparq_QueBuster_BasketDetails details)
        {
            _unitOfWork.Context.tbl_Sparq_QueBuster_BasketDetails.Add(details);
        }

        public void UpdateQueueHeader(tbl_Sparq_QueueBuster_CardDetails update)
        {
            _unitOfWork.Context.Entry(update).State = EntityState.Modified;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _unitOfWork.Context.Dispose();
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