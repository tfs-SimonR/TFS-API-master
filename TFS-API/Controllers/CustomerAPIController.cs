using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SPARQTestDAL;
using TFS_API.Models.DTO;
using TFS_API.Repositorys;
using TFS_API.Repositorys.Interfaces;
using TFS_API.Services.Interfaces;

namespace TFS_API.Controllers
{
    [RoutePrefix("api/v1/CustomerAPI")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CustomerAPIController : ApiController
    {
        private IQueueBusterRepository _queueBusterRepository;
        private readonly IExceptionService _errorService;

        public CustomerAPIController(
            IExceptionService errorService, 
            IQueueBusterRepository queueBusterRepository)
        {
            _errorService = errorService;
            _queueBusterRepository = queueBusterRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("bustaque")]
        public async Task<HttpResponseMessage> PostBustAQueue([FromBody] BustAQueueDTO dto)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var repository = new QueueBusterRepository(unitOfWork);
                    var headerdataModel = new tbl_Sparq_QueueBuster_CardDetails
                    {
                        cardnumber = dto.cardNumber,
                        scandate = DateTime.Now,
                        isOpen = true
                    };
                    repository.InsertQueueBust(headerdataModel);
                    unitOfWork.SaveChanges();
                    foreach (var itemModel in dto.QueueItems.Select(item => new tbl_Sparq_QueBuster_BasketDetails
                    {
                        cardnumber = dto.cardNumber,
                        sku = item.sku,
                        quantity = item.qty,
                        scandate = DateTime.Now,
                        isOpen = true
                    }))
                    {
                        repository.InsertQueueBustBasketItems(itemModel);
                        unitOfWork.SaveChanges();
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException);
            }
        }
    }
}
