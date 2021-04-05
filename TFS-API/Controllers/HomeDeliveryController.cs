using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TFS_API.Models.DTO;
using TFS_API.Repositorys;
using TFS_API.Repositorys.Interfaces;
using TFS_API.Services.Interfaces;

namespace TFS_API.Controllers
{
    [RoutePrefix("api/v1/HomeDeliveryAPI")]
    public class HomeDeliveryController : ApiController
    {
        private readonly IExceptionService _errorService;
        private readonly IClickAndCollectService _collectService;
        private readonly IHandyScannerService _scannerService;
        private readonly IGoogleAPIService _googleApiService;
        private readonly IEmailService _emailService;
        private readonly UnitOfWork _unitOfWork;
        private readonly IClickAndCollectReserveRepository _reserveRepository;
        private readonly IClickAndCollectStatusRepository _statusRepository;
        static readonly UnitOfWork unitOfWork = new UnitOfWork();
        PricingRepository _pricingRepository = new PricingRepository(unitOfWork);
        ProductsRepository _productRepository = new ProductsRepository(unitOfWork);
        StockRepository _stockRepository = new StockRepository(unitOfWork);
        StoreDestailsRepository _storeDestailsRepository = new StoreDestailsRepository(unitOfWork);

        public HomeDeliveryController(IExceptionService errorService,
            IClickAndCollectService collectService,
            IHandyScannerService scannerService,
            IGoogleAPIService googleApiService,
            IEmailService emailservice,
            UnitOfWork unitorOfWork,
            IClickAndCollectStatusRepository statusRepository,
            IClickAndCollectReserveRepository reserveRepository)
        {
            _errorService = errorService;
            _collectService = collectService;
            _scannerService = scannerService;
            _googleApiService = googleApiService;
            _emailService = emailservice;
            _unitOfWork = unitorOfWork;
            _reserveRepository = reserveRepository;
            _statusRepository = statusRepository;
        }

        [Route("get-stockatstore-info")]
        [HttpGet]
        public HttpResponseMessage GetStockInformation([FromUri] StoreDetailsDTO dto)
        {
            try
            {
                //May need store details to pass on to customer
                var getStoreDetails = _storeDestailsRepository.GetStoreDetails(dto.storeId);
               
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                _errorService.Capture(e);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        public HttpResponseMessage GetAllProducts()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
