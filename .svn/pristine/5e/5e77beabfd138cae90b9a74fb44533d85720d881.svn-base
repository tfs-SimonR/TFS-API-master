using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPARQTestDAL;

namespace TFS_API.Repositorys.Interfaces
{
    public interface IQueueBusterRepository
    {
        IEnumerable<tbl_Sparq_QueueBuster_CardDetails> GetAllOpenBustedQueues();

        tbl_Sparq_QueueBuster_CardDetails GetQueueByCardNumber(string cardNumber);

        tbl_Sparq_QueBuster_BasketDetails GetBasketInformation(string cardNumber);

        void InsertQueueBust(tbl_Sparq_QueueBuster_CardDetails queue);

        void UpdateQueueHeader(tbl_Sparq_QueueBuster_CardDetails update);
        void InsertQueueBustBasketItems(tbl_Sparq_QueBuster_BasketDetails details);
    }
}