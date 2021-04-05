using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AuditDataAccessLayer;
using Mi9DataAccessLayer;
using ProcessedDAL;
using Serilog;
using TFS_API.BusinessObjects;
using TFS_API.Helpers;
using TFS_API.Models.DTO;
using TFS_API.Models.PostViewModels;
using TFS_API.Models.ViewModels;
using tfs_api.query.data.Models;
using TFS_API.Service.Interface;
using TFS_API.Services;
using TFS_API.Services.Interfaces;

namespace TFS_API.Controllers
{
    /// <summary>
    ///     Android HHT Scanner End Points
    /// </summary>
    [RoutePrefix("api/v1/HandyScanner")]
    public class HandyScannerController : ApiController
    {
        #region Constructors

        /// <summary>
        ///     Android HHT Scanner End Points
        /// </summary>
        public HandyScannerController(
            IHandyScannerService handyScannerService,
            IExceptionService errorService,
            IAuditService auditService,
            ICreateFileService createFileService,
            IScannerService scannerService,
            IARTSService artsService)
        {
            _handyScannerService = handyScannerService;
            _errorService = errorService;
            _auditService = auditService;
            _createFileService = createFileService;
            _scannerService = scannerService;
            _artsService = artsService;
        }

        #endregion

        #region Setup

        /*Setup DB context*/
        private readonly ProDbContext processedDb = new ProDbContext();
        public readonly Mi9DBEntities mi9db = new Mi9DBEntities();
        private readonly auditEntities _auditdb = new auditEntities();
        private readonly IHandyScannerService _handyScannerService;
        private readonly IExceptionService _errorService;
        private readonly IAuditService _auditService;
        private readonly ICreateFileService _createFileService;
        private readonly IScannerService _scannerService;
        private readonly IARTSService _artsService;

        #endregion


        #region Warehouse To Stores Endpoints

        /// <summary>
        ///     TOFS Warehouse Delivery, list for store from storeId
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns>HttpStatusCode with 200 for OK and 404 for not found</returns>
#if DEBUG
#else
        [Authorize]
#endif
        [HttpGet]
        [Route("warehousetostorelist/{storeId}")]
        public async Task<HttpResponseMessage> GetWarehousetoStoreDelivery(string storeId)
        {
            try
            {
                Log.Information("warehousetostorelist " + storeId);
                var getDelivery = _handyScannerService.GetWarehouseToStoreList(storeId);
                if (getDelivery.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, getDelivery);
                return Request.CreateResponse(HttpStatusCode.NotFound,
                    "No outstanding pallets found for store " + storeId);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "API Error Logged " + ex.InnerException);
            }
        }


        [HttpGet]
        [Route("GetHistoricWarehouse/{storeId}")]
        public HttpResponseMessage GetHistoricWarehouse(string storeId)
        {
            try
            {
                var getDeliverys = _handyScannerService.GetPreviousWarehouseGoodsOut(storeId);
                if (getDeliverys.Any()) return Request.CreateResponse(HttpStatusCode.OK, getDeliverys);

                return Request.CreateResponse(HttpStatusCode.NotFound,
                    "No outstanding pallets found for store " + storeId);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        [HttpGet]
        [Route("GetPalletById/{palletbarcode}")]
        public HttpResponseMessage GetPalletById([FromUri] string palletbarcode)
        {
            var result = _scannerService.GetPalletByID(palletbarcode);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        private static void UpdateMovementsTable(WarehouseDto palletObject)
        {
            palletObject.BookedIn_Date = DateTime.Now;
            palletObject.IsDelivered = true;
            palletObject.IsHeld = false;
            new WarehouseBo().UpdatePallet(palletObject);
        }

        /// <summary>
        ///     Endpoint to accept a Warehouse To Store Delivery, Must send palletbarcode AND StoreId in body, palletbarcode gives
        ///     a param to use to search in the DB,
        ///     var getRecord = mi9TestEntities.consighdrs.FirstOrDefault(x => x.consignment == pm.palletbarcode);
        /// </summary>
        /// <param name="pm">
        /// </param>
        /// <returns></returns>
#if DEBUG
#else
        [Authorize]
#endif
        [HttpPut]
        [Route("warehousetostoreaccept")]
        public async Task<HttpResponseMessage> PutWarehouseToStoreAcceptAsyncTask([FromBody] WarehouseAcceptPM pm)
        {
            var palletList = new List<WarehouseDto>();
            var gdoList = new List<string>();
            try
            {
                if (!ModelState.IsValid) return Request.CreateResponse(HttpStatusCode.NotAcceptable);

                var getConsighdr = mi9db.consighdrs.FirstOrDefault(x => x.consignment == pm.palletbarcode);
                var getConsigsests = mi9db.consigdests.FirstOrDefault(x => x.consignment == getConsighdr.consignment);
                var getConsigline = mi9db.consiglines.Where(x => x.condestint == getConsigsests.condestint);

               
                var audit = _auditService.HandyScannerWarehouseToStoreAudit(getConsighdr, pm);
                _auditdb.tbl_WarehouseToStore_Audit.Add(audit);
                await _auditdb.SaveChangesAsync();

                if (getConsighdr != null)
                {
                    var processedModel = new hs_processed_consighdrs
                    {
                        consignment = getConsighdr.consignment,
                        date_bookedin = DateTime.Now,
                        isBookedin = true,
                        storeid = pm.storeId
                    };
                    processedDb.hs_processed_consighdrs.Add(processedModel);
                }

                await processedDb.SaveChangesAsync();


                var flatfile = new List<string>();

                var date = DateTime.Now.Date.ToString("ddMMyy");
                var time = DateTime.Now.ToString("hhmm");

                if (getConsighdr != null)
                {
                    var line1 = "MI" + pm.storeId + "010000" + "0002" + date + time + getConsighdr.warehouse +
                                pm.palletbarcode;
                    var store = pm.storeId;
                    flatfile.Add(line1);
                    gdoList.Add(pm.palletbarcode);

                    foreach (var shipmentitems in getConsigline)
                    {
                        var getRetailPrice = (from products in mi9db.products
                            join productcodes in mi9db.productcodes on products.prodint equals productcodes.prodint
                            join retailprice in mi9db.TOFS_RetailPrice on productcodes.varint equals retailprice.Varint
                            where retailprice.Varint == shipmentitems.varint
                            select retailprice.Retail_Price).FirstOrDefault();

                        var getSku = (from products in mi9db.products
                            join prodcodes in mi9db.productcodes on products.prodint equals prodcodes.prodint
                            where shipmentitems.varint == prodcodes.varint
                            select prodcodes.variantcode).FirstOrDefault();


                        var totalValue = Math.Round((decimal) (shipmentitems.issueqty * getRetailPrice), 2);

                        var totalInt = (int) totalValue;
                        //changed from replacelastfour
                        var retailPrice = DataHelpers.setQuantity(totalInt.ToString());
                        var qtytoEight = DataHelpers.setQuantity(shipmentitems.issueqty.ToString());
                        string lines;
                        if (getSku != null && getSku.Length <= 6)
                            /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                            lines = string.Format("MV" + getSku + "  " + retailPrice + " " + qtytoEight);
                        else
                            /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                            lines = string.Format("MV" + getSku + retailPrice + " " + qtytoEight);
                        if (getSku != "508938") flatfile.Add(lines);
                    }
                    /*Call the service to create the file and update*/

                    _createFileService.CreateFile(store, flatfile);
                }

                /*Send the OK status and a list of the GDO back to the device.*/
                return Request.CreateResponse(HttpStatusCode.OK, gdoList);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "API Error Logged, Contact Support ");
            }
        }

        #endregion

        #region Supplier To Store Endpoints

        /// <summary>
        ///     Supplier Direct List of outstanding deliveries.
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns>HttpStatusCode with 200 for OK and 404 for not found</returns>
#if DEBUG
#else
        [Authorize]
#endif
        [HttpGet]
        [Route("suppliertostorelist/{storeId}")]
        public async Task<HttpResponseMessage> GetSupplierDeliveryToStore(string storeId)
        {
            try
            {
                Log.Information("suppliertostorelist" + storeId);
                var getDelivery = await _handyScannerService.GetSupplierToStoresList(storeId);

                return Request.CreateResponse(HttpStatusCode.OK, getDelivery);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "API Error Logged " + ex.InnerException);
            }
        }

        /// <summary>
        ///     Mass Accept SUPPLIER DIRECT FROM TFS-BACKOFFICE
        /// </summary>
        /// <param name="ponumber"></param>
        /// <returns></returns>
        [Route("PutSuppliertoStoreMassAccept")]
        public async Task<HttpResponseMessage> PutSuppliertoStoreMassAccept([FromBody] SupplierAcceptDTO ponumber)
        {
            var ponumberList = new List<string>();
            var linint = 0;

            try
            {
                var getRecord = processedDb.purchorders.FirstOrDefault(x => x.ordernumber == ponumber.po_number);
                if (getRecord != null)
                {
                    getRecord.status = 7;
                    processedDb.Entry(getRecord).State = EntityState.Modified;
                }


                if (!ModelState.IsValid) return Request.CreateResponse(HttpStatusCode.NotAcceptable);
                var getPonumber = mi9db.purchorders.FirstOrDefault(x => x.ordernumber == ponumber.po_number);
                if (getPonumber != null)
                    linint = getPonumber.transint;

                var getpoLine = mi9db.purchordlines.Where(x => x.transint == linint);
                var getSupplierCode = mi9db.suppliers.FirstOrDefault(x => x.suppint == getPonumber.suppint)?.supplier1;

                var processedModel = new hs_processed_purchorders
                {
                    ponumber = ponumber.po_number,
                    date_booked_in = DateTime.Now,
                    isBookedin = true,
                    storeid = ponumber.storeid
                };
                processedDb.hs_processed_purchorders.Add(processedModel);
                await processedDb.SaveChangesAsync();


                var flatfile = new List<string>();
                var date = DateTime.Now.Date.ToString("ddMMyy");
                var time = DateTime.Now.ToString("hhmm");
                var headerLine = "PO" + ponumber.storeid + "010000" + "0002" + date + time + getSupplierCode +
                                 "        " + ponumber.po_number + "          ";

                flatfile.Add(headerLine);
                foreach (var line in getpoLine)
                {
                    var audit = _auditService.MassAcceptAudit(getPonumber, ponumber.storeid, ponumber.user_name,
                        line.varint);
                    _auditdb.tbl_SupplierMassAccept_Audit.Add(audit);
                    await _auditdb.SaveChangesAsync();

                    var getProdint = (from prodcodes in mi9db.productcodes
                        where prodcodes.varint == line.varint
                        select prodcodes.prodint).FirstOrDefault();

                    var getVariantCode = (from prodcodes in mi9db.productcodes
                        where prodcodes.varint == line.varint
                        select prodcodes.variantcode).FirstOrDefault();

                    var getRetailPrice = (from products in mi9db.products
                        join productcodes in mi9db.productcodes on products.prodint equals productcodes.prodint
                        join retailprice in mi9db.TOFS_RetailPrice on productcodes.varint equals retailprice.Varint
                        where products.prodint == getProdint
                        select retailprice.Retail_Price).FirstOrDefault();


                    var totalValue = Math.Round((decimal) (line.quantity * getRetailPrice), 2);

                    var truncate = DataHelpers.TruncateDecimal(totalValue, 2);

                    var removedecimalpoint = DataHelpers.ConvertdecimaltoInt(truncate);

                    var intToString =
                        DataHelpers.setQuantity(removedecimalpoint.ToString(CultureInfo.InvariantCulture));

                    //TODO CHANGE ME SO THIS IS A FULL INT 

                    var convertfromDecimal = DataHelpers.Normalize(line.quantity);

                    var qtytoEight = DataHelpers.setQuantity(convertfromDecimal.ToString());

                    string lines;

                    if (getVariantCode != null && getVariantCode.Length <= 6)
                        /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                        lines = string.Format("MV" + getVariantCode + "  " + intToString + " " + qtytoEight);
                    else
                        /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                        lines = string.Format("MV" + getVariantCode + intToString + " " + qtytoEight);
                    ponumberList.Add(ponumber.po_number);
                    flatfile.Add(lines);
                }

                _createFileService.CreateFile(ponumber.storeid, flatfile);

                /*Send the OK status and a list of the GDO back to the device.*/
                return Request.CreateResponse(HttpStatusCode.OK, ponumberList);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "API Error Logged " + ex.InnerException);
            }
        }

        [HttpPut]
        [Route("suppliertostoreacceptlist")]
        public async Task<HttpResponseMessage> PutSupplierAcceptList([FromBody] List<string> vm)
        {
            try
            {
                var GDOList = new List<string>();

                string store = null;
                foreach (var po in vm)
                {
                    var flatfile = new List<string>();
                    //Get the PO
                    var getPO = mi9db.purchorders.FirstOrDefault(x => x.ordernumber == po);
                    var getPOLines = mi9db.purchordlines.Where(x => x.transint == getPO.transint);
                    //Get the supplier code
                    var getSupplierCode = mi9db.suppliers.FirstOrDefault(x => x.suppint == getPO.suppint)?.supplier1;
                    //Get date and time as strings
                    var date = DateTime.Now.Date.ToString("ddMMyy");
                    var time = DateTime.Now.ToString("hhmm");
                    //Build the header line
                    var headerLine = "PO" + getPO.location + "010000" + "0002" + date + time + getSupplierCode +
                                     "        " + getPO.ordernumber + "          ";
                    //Hold some variables for later use.
                    store = getPO.location;
                    flatfile.Add(headerLine);
                    GDOList.Add(po);
                    //Process the items scanned in
                    foreach (var shipmentitems in getPOLines)
                    {
                        var getProdint = (from prodcodes in mi9db.productcodes
                            where prodcodes.prodint == shipmentitems.prodint
                            select prodcodes.prodint).FirstOrDefault();

                        var getSKU = (from prodcodes in mi9db.productcodes
                            where prodcodes.prodint == shipmentitems.prodint
                            select prodcodes.variantcode).FirstOrDefault();
                        var getRetailPrice = (from products in mi9db.products
                            join productcodes in mi9db.productcodes on products.prodint equals productcodes.prodint
                            join retailprice in mi9db.TOFS_RetailPrice on productcodes.varint equals retailprice.Varint
                            where products.prodint == getProdint
                            select retailprice.Retail_Price).FirstOrDefault();

                        var totalValue = Math.Round((decimal) (shipmentitems.quantity * getRetailPrice), 2);
                        var truncate = DataHelpers.TruncateDecimal(totalValue, 2);
                        var removedecimalpoint = DataHelpers.ConvertdecimaltoInt(truncate);
                        var intToString =
                            DataHelpers.setQuantity(removedecimalpoint.ToString(CultureInfo.InvariantCulture));
                        var qtytoEight = DataHelpers.setQuantity(shipmentitems.quantity.ToString());

                        string lines;
                        if (getSKU.Length <= 6)
                            /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                            lines = string.Format("MV" + getSKU + "  " + intToString + " " + qtytoEight);
                        else
                            /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                            lines = string.Format("MV" + getSKU + intToString + " " + qtytoEight);
                        flatfile.Add(lines);
                    }

                    //_createFileService.CreateFile(store, flatfile);
                }


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "API Error Logged " + ex.InnerException);
            }
        }

        /// <summary>
        ///     This is the accept for supplier to store, send back object as per the spec and i can loop the json array
        /// </summary>
        /// ///
        /// <param name="vm"></param>
        /// <returns></returns>
#if DEBUG
#else
        [System.Web.Http.Authorize]
#endif
        [HttpPut]
        [Route("suppliertostoreaccept")]
        public async Task<HttpResponseMessage> PutSupplierDeliveryToStore([FromBody] List<SupplierToStoreDTO> vm)
        {
            try
            {
                if (!ModelState.IsValid) return Request.CreateResponse(HttpStatusCode.NotAcceptable);


                var GDOList = new List<string>();
                var flatfile = new List<string>();
                string store = null;

                foreach (var item in vm)
                {
                    //Get the PO
                    var getPO = mi9db.purchorders.FirstOrDefault(x => x.ordernumber == item.po_number);
                    //Get the supplier code
                    var getSupplierCode = mi9db.suppliers.FirstOrDefault(x => x.suppint == getPO.suppint)?.supplier1;

                    //This updates the processed order table for spoofing
                    var getRecord = processedDb.purchorders.FirstOrDefault(x => x.ordernumber == item.po_number);
                    if (getRecord != null)
                    {
                        getRecord.status = 7;
                        processedDb.Entry(getRecord).State = EntityState.Modified;
                    }

                    //Get date and time as strings
                    var date = DateTime.Now.Date.ToString("ddMMyy");
                    var time = DateTime.Now.ToString("hhmm");

                    //Build the header line
                    var headerLine = "PO" + item.store_id + "010000" + "0002" + date + time + getSupplierCode +
                                     "        " + getPO.ordernumber + "          ";
                    //Hold some variables for later use.
                    store = item.store_id;
                    flatfile.Add(headerLine);
                    GDOList.Add(item.po_number);

                    //Add to the processed table so doesnt show on the scanner any longer.
                    var processedModel = new hs_processed_purchorders
                    {
                        ponumber = item.po_number,
                        date_booked_in = DateTime.Now,
                        isBookedin = true,
                        storeid = item.store_id
                    };

                    processedDb.hs_processed_purchorders.Add(processedModel);
                    await processedDb.SaveChangesAsync();

                    //Process the items scanned in
                    foreach (var shipmentitems in item.shipment_items)
                    {
                        /*Audit all the lines in the shipment*/
                        var audit = _auditService.HandyScannerSupplierToStoreAudit(getPO, item.user_name, item.store_id,
                            shipmentitems.sku, shipmentitems.sku_counted_qty, shipmentitems.sku_expected_qty);
                        _auditdb.tbl_SupplierToStore_Audit.Add(audit);
                        await _auditdb.SaveChangesAsync();

                        var getProdint = (from prodcodes in mi9db.productcodes
                            where prodcodes.variantcode == shipmentitems.sku
                            select prodcodes.prodint).FirstOrDefault();

                        var getRetailPrice = (from products in mi9db.products
                            join productcodes in mi9db.productcodes on products.prodint equals productcodes.prodint
                            join retailprice in mi9db.TOFS_RetailPrice on productcodes.varint equals retailprice.Varint
                            where products.prodint == getProdint
                            select retailprice.Retail_Price).FirstOrDefault();


                        var totalValue = Math.Round((decimal) (shipmentitems.sku_counted_qty * getRetailPrice), 2);
                        var truncate = DataHelpers.TruncateDecimal(totalValue, 2);
                        var removedecimalpoint = DataHelpers.ConvertdecimaltoInt(truncate);

                        var intToString =
                            DataHelpers.setQuantity(removedecimalpoint.ToString(CultureInfo.InvariantCulture));
                        var qtytoEight = DataHelpers.setQuantity(shipmentitems.sku_counted_qty.ToString());

                        string lines;
                        if (shipmentitems.sku.Length <= 6)
                            /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                            lines = string.Format("MV" + shipmentitems.sku + "  " + intToString + " " + qtytoEight);
                        else
                            /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                            lines = string.Format("MV" + shipmentitems.sku + intToString + " " + qtytoEight);
                        flatfile.Add(lines);
                    }
                }

                /*Call the service to create the file and update*/
                _createFileService.CreateFile(store, flatfile);

                /*Send the OK status and a list of the GDO back to the device.*/
                return Request.CreateResponse(HttpStatusCode.OK, GDOList);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "API Error Logged " + ex.InnerException);
            }
        }

        #endregion

        #region Store to Store Endpoints

        /// <summary>
        ///     Gets list of Store to Store transfers
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
#if DEBUG
#else
        [System.Web.Http.Authorize]
#endif
        [HttpGet]
        [Route("storetostorelist/{storeId}")]
        public HttpResponseMessage GetStoretoStore(string storeId)
        {
            try
            {
                //if (storeId == "206")
                //{
                //    var getList = _handyScannerService.GetStoreToStoreList(storeId);
                //    return Request.CreateResponse(HttpStatusCode.OK, getList);
                //}
                Log.Information("storetostorelist" + storeId);

                var getData = _scannerService.GetInterStoreList(int.Parse(storeId));
                if (getData.Any())
                {
                    var grouped = getData
                        .OrderBy(x => x.Pallet_Number)
                        .GroupBy(x => x.Pallet_Number)
                        .Select(g => new
                        {
                            gdo_number = g.Key,
                            shipment_items = g.Select(ShipmentItems => new
                            {
                                store_origin = ShipmentItems.Source_Id,
                                store_destination = ShipmentItems.Destination_Id,
                                description = ShipmentItems.Description,
                                sku = ShipmentItems.SKU,
                                sku_qty = ShipmentItems.Qty
                            })
                        });
                    
                    return Request.CreateResponse(HttpStatusCode.OK, grouped);
                }

                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No IST for store " + storeId);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERROR HERE" + storeId);
            }
        }

        /// <summary>
        ///     This is the accept for supplier to store, send back object as per the spec and i can loop the json array
        /// </summary>
        /// <param name="vm"></param>
        /// <returns>200 OK</returns>
#if DEBUG
#else
        [System.Web.Http.Authorize]
#endif
        [HttpPut]
        [Route("storetostoreaccept")]
        public async Task<HttpResponseMessage> PutStoreDeliveryToStore([FromBody] List<StoreToStoreDTO> vm)
        {
            var palletList = new List<WarehouseDto>();
            var GDOList = new List<string>();
            try
            {
                if (!ModelState.IsValid) return Request.CreateResponse(HttpStatusCode.NotAcceptable);

                var flatfile = new List<string>();
                var date = DateTime.Now.Date.ToString("ddMMyy"); //example = 230219
                var time = DateTime.Now.ToString("hhmm"); //example = 0900
                foreach (var item in vm)
                foreach (var line in item.shipment_items)
                {
                    string line1;
                    string store = null;

                    var result = _scannerService.GetPalletByID(item.gdo_number);
                    if (result == null)
                    {
                        var getConsighdr = mi9db.consighdrs.FirstOrDefault(x => x.consignment == item.gdo_number);
                        line1 = "MI" + item.store_id + "010000" + "0002" + date + time + getConsighdr.warehouse +
                                item.gdo_number + "  ";
                        store = item.store_id;
                        flatfile.Add(line1);
                        GDOList.Add(item.gdo_number);

                        var processedModel = new hs_processed_interstore
                        {
                            gdonumber = getConsighdr.consignment,
                            date_booked_in = DateTime.Now,
                            isBookedin = true,
                            storeid = item.store_id
                        };

                        processedDb.hs_processed_interstore.Add(processedModel);
                        await processedDb.SaveChangesAsync();

                        foreach (var shipmentitems in item.shipment_items)
                        {
                            var getSource =
                                mi9db.consigdests.FirstOrDefault(x => x.consignment == getConsighdr.consignment);
                            string source = null;
                            if (getSource != null)
                                source = getSource.warehouse;

                            var getDestination =
                                mi9db.consigdests.FirstOrDefault(x => x.consignment == getConsighdr.consignment);
                            string destination = null;
                            if (getDestination != null)
                                destination = getDestination.destination;

                            var audit = _auditService.HandyScannerISTStockPutAudit(getConsighdr, shipmentitems.sku,
                                shipmentitems.sku_counted_qty, shipmentitems.sku_expected_qty, item.user_name,
                                source, destination);
                            _auditdb.tbl_InterStore_Audit.Add(audit);
                            await _auditdb.SaveChangesAsync();

                            var getProdint = mi9db.productcodes
                                .Where(prodcodes => prodcodes.variantcode == shipmentitems.sku)
                                .Select(prodcodes => prodcodes.prodint).FirstOrDefault();

                            var getRetailPrice = DataHelpers.GetRetailPrice(getProdint);

                            var totalValue = Math.Round((decimal) (shipmentitems.sku_counted_qty * getRetailPrice),
                                2);

                            var totalInt = (int) totalValue;
                            var intToString = DataHelpers.setQuantity(totalInt.ToString());
                            var qtytoEight = DataHelpers.setQuantity(shipmentitems.sku_counted_qty.ToString());

                            string lines;
                            //if the sku is not 8 chars long then we need to add in the whitespace
                            if (shipmentitems.sku.Length <= 6)
                                /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                                lines = string.Format("MV" + shipmentitems.sku + "  " + intToString + " " +
                                                      qtytoEight);
                            else
                                /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                                lines = string.Format("MV" + shipmentitems.sku + intToString + " " + qtytoEight);
                            flatfile.Add(lines);
                        }

                        _createFileService.CreateFile(store, flatfile);
                        /*Send the OK status and a list of the GDO back to the device.*/
                        return Request.CreateResponse(HttpStatusCode.OK, GDOList);
                    }

                    var pallet_number = result.First();
                    palletList.AddRange(result);
                    UpdateMovementsTable(result.First());
                    var newFlatfile = new List<string>();
                    if (pallet_number != null)
                    {
                        line1 = "MI" + item.store_id + "010000" + "0002" + date + time + result.First().Shipment_Date +
                                item.gdo_number + "  ";
                        store = item.store_id;
                        newFlatfile.Add(line1);

                        var totalValue = Math.Round((decimal) (line.sku_counted_qty * 2.0), 2);
                        var totalInt = (int) totalValue;
                        //changed from replacelastfour
                        var retailPrice = DataHelpers.setQuantity(totalInt.ToString());
                        var qtytoEight = DataHelpers.setQuantity(line.sku_counted_qty.ToString());

                        string lines;
                        if (line.sku != null && line.sku.Length <= 6)
                            /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                            lines = string.Format("MV" + line.sku + "  " + retailPrice + " " + qtytoEight);
                        else
                            /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                            lines = string.Format("MV" + line.sku + retailPrice + " " + qtytoEight);
                        if (line.sku != "508938") newFlatfile.Add(lines);

                        /*Call the service to create the file and update*/

                        _createFileService.CreateFile(store, newFlatfile);
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                    {
                        var getConsighdr = mi9db.consighdrs.FirstOrDefault(x => x.consignment == item.gdo_number);
                        line1 = "MI" + item.store_id + "010000" + "0002" + date + time + getConsighdr.warehouse +
                                item.gdo_number + "  ";
                        store = item.store_id;
                        flatfile.Add(line1);
                        GDOList.Add(item.gdo_number);

                        var processedModel = new hs_processed_interstore
                        {
                            gdonumber = getConsighdr.consignment,
                            date_booked_in = DateTime.Now,
                            isBookedin = true,
                            storeid = item.store_id
                        };

                        processedDb.hs_processed_interstore.Add(processedModel);
                        await processedDb.SaveChangesAsync();

                        foreach (var shipmentitems in item.shipment_items)
                        {
                            var getSource =
                                mi9db.consigdests.FirstOrDefault(x => x.consignment == getConsighdr.consignment);
                            string source = null;
                            if (getSource != null)
                                source = getSource.warehouse;

                            var getDestination =
                                mi9db.consigdests.FirstOrDefault(x => x.consignment == getConsighdr.consignment);
                            string destination = null;
                            if (getDestination != null)
                                destination = getDestination.destination;

                            var audit = _auditService.HandyScannerISTStockPutAudit(getConsighdr, shipmentitems.sku,
                                shipmentitems.sku_counted_qty, shipmentitems.sku_expected_qty, item.user_name, source,
                                destination);
                            _auditdb.tbl_InterStore_Audit.Add(audit);
                            await _auditdb.SaveChangesAsync();

                            var getProdint = mi9db.productcodes
                                .Where(prodcodes => prodcodes.variantcode == shipmentitems.sku)
                                .Select(prodcodes => prodcodes.prodint).FirstOrDefault();

                            var getRetailPrice = DataHelpers.GetRetailPrice(getProdint);

                            var totalValue = Math.Round((decimal) (shipmentitems.sku_counted_qty * getRetailPrice), 2);

                            var totalInt = (int) totalValue;
                            var intToString = DataHelpers.setQuantity(totalInt.ToString());
                            var qtytoEight = DataHelpers.setQuantity(shipmentitems.sku_counted_qty.ToString());

                            string lines;
                            //if the sku is not 8 chars long then we need to add in the whitespace
                            if (shipmentitems.sku.Length <= 6)
                                /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                                lines = string.Format("MV" + shipmentitems.sku + "  " + intToString + " " + qtytoEight);
                            else
                                /*MV <ProductId> TotalValue (<Quantity> * <RetailPrice>) <Quantity>*/
                                lines = string.Format("MV" + shipmentitems.sku + intToString + " " + qtytoEight);
                            flatfile.Add(lines);
                        }
                    }
                    /*Call the service to create the file and update*/


                    _createFileService.CreateFile(store, flatfile);

                    /*Send the OK status and a list of the GDO back to the device.*/
                    return Request.CreateResponse(HttpStatusCode.OK, GDOList);
                }

                return Request.CreateResponse(HttpStatusCode.OK, GDOList);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "API Error Logged " + ex.InnerException);
            }
        }

        /// <summary>
        ///     Returns stock at current store and within a 15Km radius of the current store
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
#if DEBUG
#else
        [System.Web.Http.Authorize]
#endif
        [HttpGet]
        //[Route("stockatstoretest/{storeId}/{productCode}")]
        [Route("stockatstoreandradius15/{storeId}/{productCode}")]
        public HttpResponseMessage GetStockTest(string storeId, string productCode)
        {
            try
            {
                NearestStoresVM getnearestStores = null;
                if (productCode.Length <= 5)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not a valid product code");
                if (productCode.Length == 13)
                {
                    var getProductCode = (from products in mi9db.products
                        join productcodes in mi9db.productcodes on products.prodint equals productcodes.prodint
                        join prodkeys in mi9db.prodkeys on productcodes.varint equals prodkeys.prodint
                        where prodkeys.prodaltkey == productCode
                        select products.prodcode).FirstOrDefault();

                    getnearestStores = _handyScannerService.GetNearestStoresTest(getProductCode, storeId);

                    return Request.CreateResponse(HttpStatusCode.OK, getnearestStores);
                }

                if (productCode.Length <= 8)
                {
                    getnearestStores = _handyScannerService.GetNearestStoresTest(productCode, storeId);
                    return Request.CreateResponse(HttpStatusCode.OK, getnearestStores);
                }

                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "No stock with  " + productCode + " at this store.");
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "This product code does not exist.");
            }
        }

        #endregion
    }
}