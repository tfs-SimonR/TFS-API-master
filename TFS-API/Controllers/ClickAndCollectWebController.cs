using ClickAndCollectDAL;
using Serilog;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TFS_API.Helpers;
using TFS_API.Models.DTO;
using TFS_API.Repositorys;
using TFS_API.Repositorys.Interfaces;
using TFS_API.Services.Interfaces;


namespace TFS_API.Controllers
{
    /// <summary>
    /// Click and collect web panel endpoints for Pricing People Iframe for tofs website
    /// </summary>
    [RoutePrefix("api/v1/webpaneldata")]
    public class ClickAndCollectWebController : ApiController
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
        ClickAndCollectPricingRepository _pricingRepository = new ClickAndCollectPricingRepository(unitOfWork);
        ProductsRepository _productRepository = new ProductsRepository(unitOfWork);
        StoreDestailsRepository _storeDestailsRepository = new StoreDestailsRepository(unitOfWork);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorService"></param>
        /// <param name="collectService"></param>
        /// <param name="scannerService"></param>
        /// <param name="googleApiService"></param>
        /// <param name="emailservice"></param>
        /// <param name="unitorOfWork"></param>
        /// <param name="statusRepository"></param>
        /// <param name="reserveRepository"></param>
        public ClickAndCollectWebController(IExceptionService errorService,
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
        /// Returns product options at a store
        ///  </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RCGetProductOptions")]
        public HttpResponseMessage RCGetProductOptions([FromUri] SKUBranchDTO dto)
        {
            try
            {
                var sku = _productRepository.GetTranslation(dto.guid);
                var prod_desc = _productRepository.GetProductDetails(sku.sku).proddesc ?? "";
                if (sku.hasVariant == true)
                {
                    var variantlist = new List<ClickAndCollectPanelDataDTO.PanelData>();
                    var varint = _pricingRepository.GetVarint(sku.sku);
                    var root = new ClickAndCollectPanelDataDTO();
                    var wasPrice = _pricingRepository.getwasPrice(varint);
                    var getWasPriceFromObject = wasPrice.PriceHistoryCheck(0.00m);

                    var mrp = _pricingRepository.Getmrp(varint);
                    var getMrpFromObject = mrp.CheckMrp(0.00m);

                    root.sku = sku.sku;
                    root.current_price = _pricingRepository.getcurrentPrice(varint).retailprice;

                    root.was_price = getWasPriceFromObject;
                    root.product_name = prod_desc;

                    root.retail_price = decimal.Round((decimal)getMrpFromObject, 2, MidpointRounding.AwayFromZero);
                    var saving = root.was_price - root.current_price;
                    if (saving < 0)
                        saving = 0.00m;
                    root.saving = saving;
                    var getproductObject = _productRepository.GetProductDetails(sku.sku);
                    var getProductCodeObject =
                        _productRepository.GetProductCodeDetails(getproductObject.prodint).ToList();
                    foreach (var item in getProductCodeObject)
                    {
                        var listItem = new ClickAndCollectPanelDataDTO.PanelData
                        {
                            isAvailable = true,
                            size = item.vardesc2,
                            sku_variant = item.variantcode,
                            product_name = prod_desc,
                            colour = item.vardesc1,
                            current_price = decimal.Round(_pricingRepository.getcurrentPrice(item.varint).retailprice,
                                2, MidpointRounding.AwayFromZero),
                            was_price = decimal.Round(getWasPriceFromObject, 2, MidpointRounding.AwayFromZero),
                            retail_price = decimal.Round((decimal)getMrpFromObject, 2, MidpointRounding.AwayFromZero),
                            saving = decimal.Round(saving, 2, MidpointRounding.AwayFromZero)
                        };
                        variantlist.Add(listItem);
                    }

                    variantlist.RemoveAll(x => x.size == null);
                    root.variant_list = variantlist;
                    return Request.CreateResponse(HttpStatusCode.OK, root);
                }
                else
                {
                    var varint = _pricingRepository.GetVarint(sku.sku);
                    var model = new ClickAndCollectPanelDataDTO();
                    var wasPrice = _pricingRepository.getwasPrice(varint);
                    var getWasPriceFromObject = wasPrice.PriceHistoryCheck(0.00m);

                    var mrp = _pricingRepository.Getmrp(varint);
                    var getMrpFromObject = mrp.CheckMrp(0.00m);

                    model.sku = sku.sku;
                    model.current_price = _pricingRepository.getcurrentPrice(varint).retailprice;
                    model.was_price = getWasPriceFromObject;
                    model.product_name = prod_desc;

                    model.retail_price = decimal.Round((decimal)getMrpFromObject, 2, MidpointRounding.AwayFromZero);
                    var saving = model.retail_price - model.current_price;
                    if (saving < 0)
                        saving = 0.00m;
                    model.saving = saving;
                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException);
            }
        }


        /// <summary>
        /// Returns data for the click and collect panels for the website
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RCGetSKUPanelData")]
        public HttpResponseMessage RCGetSKUPanelData([FromUri] SKUBranchDTO dto)
        {
            try
            {
                /*this repo can be used for transaltions of GUID and to find product details*/
                var productsRepository = new ProductsRepository(unitOfWork);
                var pricingRepository = new ClickAndCollectPricingRepository(unitOfWork);

                #region GUIDS

                /*Some GUIDS to use, we will statically assign these for these to SKUS for test*/
                //07fda175104f429d9bd700617e7f7f30 = 10199231 (Joop!! CLASSICCCCC) (Fragrance)
                //fc93d617d8ab4f33bc89538b7616d335 = 10400281 (DAEWOO 3 IN 1 SNACK MAKER) (Electrical)
                //c98e152337c84df2845e00f19d32e782 = 626819 (ADIDAS OUTLINE T-SHIRT NAVY) (Sports Cloting)
                //efe7e5fb8d1c413389311f880085580f = 589794 AR H Waist Comfort Trousers
                //3c79c117f1e44e2ea5f8f720d951bde9 = 619809 BootCut Denim Jean
                //a4bbc73b87c441348e5cf2b7d05c5213 =
                //0e7f43668562417a97c1b9d7f9aba830 =
                //183774aabf45423181db7203bf705a96 =
                //1b474707f44f464199a8dd8aec805f13 =
                //a11c1b0cc7014dbfa07a45170bc930b0 =

                #endregion

                var sku = productsRepository.GetTranslation(dto.guid).sku;
                var variantlist = new List<ClickAndCollectPanelDataDTO.PanelData>();
                var getproductObject = productsRepository.GetProductDetails(sku);
                var getProductCodeObject = productsRepository.GetProductCodeDetails(getproductObject.prodint).ToList();
                foreach (var item in getProductCodeObject)
                {
                    var _currentPrice = pricingRepository.getcurrentPrice(item.varint);
                    var _wasPrice = pricingRepository.getwasPrice(item.varint);
                    var mrp = _pricingRepository.Getmrp(item.varint);
                    var getMrpFromObject = mrp.CheckMrp(0.00m);
                    var getCurrentPriceFromObject = _currentPrice.PriceHeaderCheck(0.00m);
                    var getWasPriceFromObject = _wasPrice.PriceHistoryCheck(0.00m);
                    var saving = getMrpFromObject - getCurrentPriceFromObject;
                    if (saving <= 0)
                        saving = 0.00m;
                    var listItem = new ClickAndCollectPanelDataDTO.PanelData
                    {
                        isAvailable = true,
                        size = item.vardesc2,
                        sku_variant = item.variantcode,
                        colour = item.vardesc1,
                        current_price = decimal.Round(getCurrentPriceFromObject, 2, MidpointRounding.AwayFromZero),
                        was_price = decimal.Round(getWasPriceFromObject, 2, MidpointRounding.AwayFromZero),
                        retail_price = decimal.Round((decimal)getMrpFromObject, 2, MidpointRounding.AwayFromZero),
                        saving = decimal.Round((decimal)saving, 2, MidpointRounding.AwayFromZero)
                    };
                    variantlist.Add(listItem);
                }

                var topLevelcurrent = variantlist.Select(x => x.current_price).FirstOrDefault();
                var toplevelWas = variantlist.Select(x => x.was_price).FirstOrDefault();
                var toplevelMrp = variantlist.Select(x => x.retail_price).FirstOrDefault();
                var topSaving = toplevelMrp - topLevelcurrent;
                if (topSaving < 0)
                    topSaving = 0.00m;

                var model = new ClickAndCollectPanelDataDTO
                {
                    isAvailable = true,
                    sku = sku,
                    default_variant_sku = variantlist.Select(x => x.sku_variant).FirstOrDefault(),
                    current_price = decimal.Round(topLevelcurrent, 2, MidpointRounding.AwayFromZero),
                    was_price = decimal.Round(toplevelWas, 2, MidpointRounding.AwayFromZero),
                    saving = topSaving,
                    retail_price = decimal.Round(toplevelMrp, 2, MidpointRounding.AwayFromZero),
                    variant_list = variantlist
                };
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }
        /// <summary>
        /// Returns product info by variant
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-product-info-by-variant")]
        public HttpResponseMessage RCGetSKUPanelDatav2([FromUri] SKUBranchDTOv2 dto)
        {
            try
            {
                if (dto.sku_variant != null)
                {
                    var varint = _pricingRepository.GetVarint(dto.sku_variant);
                    var model = new ClickAndCollectPanelDataDTOv2();
                    var wasPrice = _pricingRepository.getwasPrice(varint);
                    var mrp = _pricingRepository.Getmrp(varint);
                    var getMrpFromObject = mrp.CheckMrp(0.00m);
                    var getWasPriceFromObject = wasPrice.PriceHistoryCheck(0.00m);

                    model.variant_sku = dto.sku_variant;
                    model.current_price = decimal.Round(_pricingRepository.getcurrentPrice(varint).retailprice, 2, MidpointRounding.AwayFromZero);
                    model.was_price = getWasPriceFromObject;

                    model.retail_price = decimal.Round((decimal)getMrpFromObject, 2, MidpointRounding.AwayFromZero);
                    var saving = (model.retail_price - model.current_price);
                    if (saving < 0)
                        saving = 0.00m;
                    model.saving = saving;
                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product Not Found");

                }
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }

        /// <summary>
        /// Returns stores with stock in a 10 mile radius
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RCGetStoresWithStock")]
        public HttpResponseMessage RCGetStoresWithStock([FromUri] SKUBranchDTO dto)
        {
            try
            {
                var sku = _productRepository.GetTranslation(dto.guid).sku;
                var getgoogleApiDetails = _googleApiService.GetGoogleGeoDetails(dto.postcode, dto.latlng);
                var lat = getgoogleApiDetails.Results.Select(x => x.Geometry.Location.Lat).FirstOrDefault();
                var longi = getgoogleApiDetails.Results.Select(x => x.Geometry.Location.Lng).FirstOrDefault();
                var getStockatLocalStores = _scannerService.GetStoreStockWebPanel(sku, longi, lat);
                return Request.CreateResponse(HttpStatusCode.OK,
                    getStockatLocalStores.list_stores.Where(s => s.stock_qty > 0).OrderBy(x => x.distance_to_store));
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException);
            }

        }

    
        /// <summary>
        /// Returns stores with stock using the Google API
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("stores")]
        public HttpResponseMessage RCGetStoresWithStockv2([FromUri] SKUBranchDTOv3 dto)
        {
            try
            {

                var getgoogleApiDetails = _googleApiService.GetGoogleGeoDetails(dto.location, dto.latlng);
                var lat = getgoogleApiDetails.Results.Select(x => x.Geometry.Location.Lat).FirstOrDefault();
                var longi = getgoogleApiDetails.Results.Select(x => x.Geometry.Location.Lng).FirstOrDefault();
                var getStockatLocalStores = _scannerService.GetStoreStockWebPanelv2(dto.sku_variant, longi, lat);
                return Request.CreateResponse(HttpStatusCode.OK,
                    getStockatLocalStores);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException);

            }

        }

        /// <summary>
        /// Distance from a postcode to a store using Google API
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-distance")]
        public HttpResponseMessage MeasureDistance([FromUri] HomeDeliveryDTO dto)
        {
            try
            {
                var getStoreDetails = _storeDestailsRepository.GetStoreDetails(dto.storeId);

                var getgoogleApiDetails = _googleApiService.GetGoogleGeoDetails(dto.postcode, null);

                var lat = getgoogleApiDetails.Results.Select(x => x.Geometry.Location.Lat).FirstOrDefault();
                var longi = getgoogleApiDetails.Results.Select(x => x.Geometry.Location.Lng).FirstOrDefault();

                var center = new GeoCoordinate((double) getStoreDetails.Longitude, (double) getStoreDetails.Latitude);
                var storeClosest = new GeoCoordinate(lat,
                    longi);
                var getdistanceBetweenstores = center.GetDistanceTo(storeClosest);
                var meterstoKm = getdistanceBetweenstores / 1000;
                var kmphtoMiles = DataHelpers.kmphTOmph(meterstoKm);
                var truncate = Math.Round(kmphtoMiles, 2);

                return Request.CreateResponse(HttpStatusCode.OK, truncate);

            }
            catch(Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Request reservation of items
        /// </summary>
        /// <returns></returns>
        /// [HttpGet]
        [Route("RCRequestReservation")]
        [HttpPost]
        public async Task<HttpResponseMessage> RcRequestReservation([FromBody] ClickAndCollectReserveDTO dto)
        {
            try
            {
                var sku = "";
                var postCode = dto.postcode ?? "";
                Log.Information(dto.sku_variant);
                Log.Information(dto.guid);
                if (dto.guid != null)
                {
                    sku = _productRepository.GetTranslation(dto.guid).sku;
                    using (var unitOfWork = new UnitOfWork())
                    {
                        var reserveRepository = new ClickAndCollectReserveRepository(unitOfWork);
                        var reserveStatusrepository = new ClickAndCollectStatusRepository(unitOfWork);
                        var storesRepository = new StoreDestailsRepository(unitOfWork);
                        var productsRepository = new ProductsRepository(unitOfWork);
                        var productdetails = _productRepository.GetTranslation(dto.guid);

                        var newreservationRecord = new ClickAndCollectReservation
                        {
                            StoreNumber = dto.storeid,
                            SKU = sku,
                            Quantity = dto.quantity,
                            CustFirstName = dto.name,
                            CustSurName = dto.name,
                            CustPostCode = postCode,
                            CustEmailAddress = dto.email,
                            CustMobileNumber = dto.mobilenumber,
                            TofsCardNumber = dto.tofscardnumber,
                            MarketingConsent = dto.marketing_consent,
                            MarketingConsentDate = DateTime.Today,
                            TermsAndCondsConsentDate = DateTime.Today
                        };
                        reserveRepository.Add(newreservationRecord);
                        //_reserveRepository.Add(newreservationRecord);
                        unitOfWork.SaveChanges();

                        var reservationId = newreservationRecord.ReservationID;
                        var newreservationStaturRecord = new ClickAndCollectReservationStatu
                        {
                            ReservationID = reservationId,
                            ReservationStatus = 1,
                            StoreId = dto.storeid,
                            StatusDate = DateTime.Now,
                            PreviousStatus = 0,
                            LastAction = "",
                            BasketTransActionId = "",
                            DateCreated = DateTime.Now
                        };
                        reserveStatusrepository.Add(newreservationStaturRecord);
                        unitOfWork.SaveChanges();
                        var storedetails = storesRepository.GetStoreDetails(dto.storeid);

                        var returnReservationRequest = new ReservationAddedDTO
                        {
                            reservationId = reservationId,
                            store_name = storedetails.StoreDescription,
                            store_address = storedetails.Address1 + ", " + storedetails.Address2 + ", " +
                                            storedetails.Address3,
                            order_item_description = productdetails.description,
                            customer_name = dto.name,
                            email = dto.email,
                            phone_number = dto.mobilenumber
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, returnReservationRequest);
                    }

                }
                else
                {
                    using (var unitOfWork = new UnitOfWork())
                    {
                        var reserveRepository = new ClickAndCollectReserveRepository(unitOfWork);
                        var reserveStatusrepository = new ClickAndCollectStatusRepository(unitOfWork);
                        var storesRepository = new StoreDestailsRepository(unitOfWork);
                        var productsRepository = new ProductsRepository(unitOfWork);
                        var productdetails = _productRepository.GetProductDetailsFromSku(dto.sku_variant);

                        var newreservationRecord = new ClickAndCollectReservation
                        {
                            StoreNumber = dto.storeid,
                            SKU = dto.sku_variant,
                            Quantity = dto.quantity,
                            CustFirstName = dto.name,
                            CustSurName = dto.name,
                            CustPostCode = postCode,
                            CustEmailAddress = dto.email,
                            CustMobileNumber = dto.mobilenumber,
                            TofsCardNumber = dto.tofscardnumber,
                            MarketingConsent = dto.marketing_consent,
                            MarketingConsentDate = DateTime.Today,
                            TermsAndCondsConsentDate = DateTime.Today
                        };
                        reserveRepository.Add(newreservationRecord);
                        unitOfWork.SaveChanges();

                        var reservationId = newreservationRecord.ReservationID;
                        var newreservationStaturRecord = new ClickAndCollectReservationStatu
                        {
                            ReservationID = reservationId,
                            ReservationStatus = 1,
                            StoreId = dto.storeid,
                            StatusDate = DateTime.Now,
                            PreviousStatus = 0,
                            LastAction = "",
                            BasketTransActionId = "",
                            DateCreated = DateTime.Now
                        };
                        reserveStatusrepository.Add(newreservationStaturRecord);
                        unitOfWork.SaveChanges();
                        var storedetails = storesRepository.GetStoreDetails(dto.storeid);

                        var returnReservationRequest = new ReservationAddedDTO
                        {
                            reservationId = reservationId,
                            store_name = storedetails.StoreDescription,
                            store_address = storedetails.Address1 + ", " + storedetails.Address2 + ", " +
                                            storedetails.Address3,
                            order_item_description = productdetails.description,
                            customer_name = dto.name,
                            email = dto.email,
                            phone_number = dto.mobilenumber
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, returnReservationRequest);
                    }
                }




            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException);

            }

        }

        /// <summary>
        /// Returns all reservations in a list
        /// </summary>
        /// <returns></returns>
        [Route("RCGetAllReservations")]
        [HttpGet]
        public HttpResponseMessage RCGetAllReservations()
        {
            try
            {
                var getAll = _collectService.GetAllReservations();
                return Request.CreateResponse(HttpStatusCode.OK, getAll);

            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException);
            }
        }


        public HttpResponseMessage RCGetReservation([FromUri] ClickAndCollectReserveDTO dto)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException);
            }
        }

        /// <summary>
        /// /get-product-stock-level-by-store?guid={guid}&storeId={store_id}&quantity={quantity}&sku_variant={sku of the variant}
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("get-product-stock-level-by-store")]
        [HttpGet]
        public HttpResponseMessage RCGetStockLevelByStore([FromUri] StockByStoreDTO dto)
        {
            try
            {
                if (dto.sku_variant != null && dto.store_id != null)
                {
                    var getVarint = _pricingRepository.GetVarint(dto.sku_variant);
                    var getStock = _productRepository.GetStoreStock(getVarint, dto.store_id);
                    var checkStock = getStock.StockCheck(0.00m);
                    var model = new StockByStoreReturnDTO { stock_qty = (int)checkStock };
                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }
                else
                {
                    var getSku = _productRepository.GetTranslation(dto.guid).sku;
                    var getVarint = _pricingRepository.GetVarint(getSku);
                    var getStock = _productRepository.GetStoreStock(getVarint, dto.store_id);
                    var checkStock = getStock.StockCheck(0.00m);
                    var model = new StockByStoreReturnDTO { stock_qty = (int)checkStock };
                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }

            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException);
            }

        }
    }
}
