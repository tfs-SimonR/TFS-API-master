using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using Mi9DataAccessLayer;
using Newtonsoft.Json;
using TFS_API.Models;
using TFS_API.Models.DTO;
using TFS_API.Models.ViewModels;
using TFS_API.Repositorys;
using TFS_API.Repositorys.Interfaces;
using TFS_API.Services.Interfaces;

namespace TFS_API.Controllers
{
    [System.Web.Http.AllowAnonymous]
    [System.Web.Http.RoutePrefix("api/v1/productlookupapi")]
    public class ProductLookupAPIController : ApiController
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

        public ProductLookupAPIController(IExceptionService errorService,
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

        /// <summary>
        /// Pass storeId and variantcode (SKU) and returns stock information for that sku at that store.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [System.Web.Http.Route("get-stock-info")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetStockInformation([FromUri] StockLookUpDTO dto)
        {
            try
            {
                var getStoreDetails = _storeDestailsRepository.GetStoreDetails(dto.storeId);
                var getgoogleApiDetails = _googleApiService.GetGoogleGeoDetails(getStoreDetails.Postcode, null);
                var lat = getgoogleApiDetails.Results.Select(x => x.Geometry.Location.Lat).FirstOrDefault();
                var longi = getgoogleApiDetails.Results.Select(x => x.Geometry.Location.Lng).FirstOrDefault();
                var getStockatLocalStores = _scannerService.GetStoreStockWebPanelv2(dto.variantcode, longi, lat);
                return Request.CreateResponse(HttpStatusCode.OK,
                    getStockatLocalStores);
            }
            catch (Exception e)
            {
                _errorService.Capture(e);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        /// <summary>
        /// Pass variantcode (SKU) and returns product information (department etc).
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [System.Web.Http.Route("get-product-details")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetProductDepartmentCodes([FromUri] ProductLookUpDTO dto)
        {
            try
            {
                var getProdcodes = await _productRepository.GetProdint(dto.sku);
                var prodint = getProdcodes.prodint;
                var config = new MapperConfiguration(cfg => cfg.CreateMap<product, ProductDTOVM>());
                var mapper = config.CreateMapper();
                return Request.CreateResponse(HttpStatusCode.OK, mapper.Map<ProductDTOVM>(await _productRepository.GetProductDetailsFromProdint(prodint)));
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Pass variantcode (SKU) and returns product vatcode.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [System.Web.Http.Route("get-product-vatcode")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetProductVatCode([FromUri] ProductLookUpDTO dto)
        {
            try
            {
                var getProdcodes = await _productRepository.GetProdint(dto.sku);
                
                var config = new MapperConfiguration(cfg => cfg.CreateMap<productcode, ProductCodesDTO>());
                
                var mapper = config.CreateMapper();

                return Request.CreateResponse(HttpStatusCode.OK, mapper.Map<ProductCodesDTO>(getProdcodes));
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [System.Web.Http.Route("get-product-stock-at-store")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetStockAtStore([FromUri] StoreSkuDTO dto)
        {
            try
            {
                using (var unitOFWork = new UnitOfWork())
                {
                    
                    var stockRepository = new StockRepository(unitOFWork);
                    var productVarint = stockRepository.GetVarint(dto.variantcode);
                    var stock = stockRepository.GetStockInformtaionAtStore(productVarint, dto.storeId);

                    var model = new StockNumberDTO()
                    {
                        qty = stock.retail

                    };
                    return Request.CreateResponse(HttpStatusCode.Accepted, model);
                }

                
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
                throw;
            }
        }

        /// <summary>
        /// Returns stock amounts at all stores for a given variantcode
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [System.Web.Http.Route("get-product-stock")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetAllStockForSku([FromUri] ProductLookUpDTO dto)
        {
            try
            {
                var stockList = new List<ProductStockDTO>();
                using (var unitOFWork = new UnitOfWork())
                {
                    var storeRepository = new StoreDestailsRepository(unitOFWork);
                    var stockRepository = new StockRepository(unitOFWork);
                    var storedetailList = new List<StoreSpoitfyDetailsDTO>();
                    var getStores = storeRepository.GetAllStores();

                    if (getStores != null) CreateStoreInfo(getStores, storedetailList);

                    AddProdutInfo(dto, storedetailList, stockRepository, stockList);
                }
                return Request.CreateResponse(HttpStatusCode.Accepted, stockList);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        private static void CreateStoreInfo(IQueryable<TFS_Store_Updated> getStores, ICollection<StoreSpoitfyDetailsDTO> storedetailList)
        {
            foreach (var store in getStores)
            {
                var model = new StoreSpoitfyDetailsDTO
                {
                    storeID = store.StoreNumber,
                    storeName = store.StoreDescription
                };
                storedetailList.Add(model);
            }
        }

        private static void AddProdutInfo(ProductLookUpDTO dto, 
            IEnumerable<StoreSpoitfyDetailsDTO> storedetailList, 
            StockRepository stockRepository,
            ICollection<ProductStockDTO> stockList)
        {
            foreach (var storeInfo in storedetailList)
            {
                var productVarint = stockRepository.GetVarint(dto.sku);

                var getStockInfo = stockRepository.GetStockInformtaionAtStore(productVarint, storeInfo.storeID);
                if (getStockInfo == null) continue;
                var model = new ProductStockDTO
                {
                    storeId = storeInfo.storeID,
                    Key = "Inventory Available:",
                    storeName = "Inventory Available:" + storeInfo.storeName,
                    variantcode = dto.sku,
                    qty = (long) getStockInfo.retail
                };
                stockList.Add(model);
            }
        }

        [System.Web.Http.Route("get-isskuvalid")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetCheckSku([FromUri] ProductLookUpDTO dto)
        {
            try
            {
                using (var unitOFWork = new UnitOfWork())
                {
                    var repository = new StockRepository(unitOFWork);
                    var productVarint = repository.GetVarint(dto.sku);
                    if (productVarint != 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                }
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                Console.WriteLine(ex);
                throw;
            }
        }



        /// <summary>
        /// pass in variantcode (SKU) returns latest retail price in use.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [System.Web.Http.Route("get-product-retail")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetProductRetailPrice([FromUri] ProductLookUpDTO dto)
        {
            try
            {
                var getVarint = _productRepository.GetVarint(dto.sku);
                
                var config = new MapperConfiguration(cfg => cfg.CreateMap<rpricehdr, RetailPriceDTO>());
                var mapper = config.CreateMapper();

                return Request.CreateResponse(HttpStatusCode.OK, mapper.Map<RetailPriceDTO>(_productRepository.GetRetailPrice(getVarint.varint)));
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        /// <summary>
        /// Pass in variantcode (SKU) returns current product cost.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [System.Web.Http.Route("get-product-cost")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetCostPrice([FromUri] ProductLookUpDTO dto)
        {
            try
            {
                var getcostPrice = _pricingRepository.GetCostPrice(dto.sku);

                if (getcostPrice != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, getcostPrice);
                }
                else
                {
                    var model = new TOFS_CostPrice()
                    {
                        prodcode = dto.sku,
                        Cost = 0.00,
                        prodint = 0,
                        currency = ""

                    };
                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }

                
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
