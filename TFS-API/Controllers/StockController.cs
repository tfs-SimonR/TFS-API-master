using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mi9DataAccessLayer;
using Newtonsoft.Json;
using ProcessedDAL;
using TFS_API.Helpers;
using TFS_API.Models.ViewModels;
using TFS_API.Repositorys;
using TFS_API.Services;
using TFS_API.Services.Interfaces;

namespace TFS_API.Controllers
{
    public class StockController : Controller
    {
        
        public ProDbContext ProcessedDb = new ProDbContext();
        public Mi9DBEntities mi9db = new Mi9DBEntities();
        private readonly IExceptionService _errorService;
        private readonly ICountingService _countingService;
        private decimal _getbrnStockRetail;
        private decimal _getStockInTransit;
        private decimal? _retailPriceDecimal;
        private decimal _getbrnStockLists;
        private int _getProdint;
        private int _getVarint;
        private string _barcode;
        private int _getWarehouseStk;
        private decimal? _stocktoStore;
        private decimal? _currentLat;
        private decimal? _currentLong;
        private string _description;
        private int _radius;
        private int _sevenDay;
        private int? _currentArea;
        private int? _result30;
        private int? _get7DaySales;
        private IEnumerable<GeoCoordinate> _geoCoordinates;

        public StockController(IExceptionService errorService)
        {
            _errorService = errorService;
        }
        public StockController()
        {
            this._errorService = new ExceptionService();
            
        }

        [Authorize]
        public ActionResult SKUSPY()
        {
            return View();
        }

        // GET: Stock
        

        // GET: Stock/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tfs_store_details tfs_store_details = await mi9db.tfs_store_details.FindAsync(id);
            if (tfs_store_details == null)
            {
                return HttpNotFound();
            }
            return View(tfs_store_details);
        }

        // GET: Stock/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult GetNearestStoresTest()
        {
            return View();
        }

        public ActionResult GetStockData(string productCode, string storeid)
        {
            try
            {
                storeid = "201";
                #region Get Lat and Long calculate stores in radius of 15KM

                var getCurrentArea = mi9db.tfs_store_details.FirstOrDefault(x => x.storeId == storeid);
                if (getCurrentArea != null)
                    _currentArea = getCurrentArea.area;

                var getcurrentLong =
                    mi9db.tfs_store_details.FirstOrDefault(x => x.storeId != null && x.storeId == storeid);
                if (getcurrentLong?.properties_long != null)
                    _currentLong = getcurrentLong.properties_long;

                var getcurrentLat =
                    mi9db.tfs_store_details.FirstOrDefault(x => x.storeId != null && x.storeId == storeid);
                if (getcurrentLat?.properties_lat != null)
                    _currentLat = getcurrentLat.properties_lat;

                var radius = 45000;
                //Current Radius Centre taken from store Lat Long
                var center = new GeoCoordinate((double) _currentLat, (double)_currentLong);

                //Create List of Lat long co ordinates for stores in 25KM range
                _geoCoordinates = mi9db.tfs_store_details.Where(x => x.storeId != "900" && x.storeId != "901" && x.storeId != "913" && x.storeId != storeid)
                    .ToList().Select(x => new GeoCoordinate((double) x.properties_lat, (double) x.properties_long))
                    .Where(x => x.GetDistanceTo(center) < radius);

                if (_geoCoordinates.Count() == 0)
                {
                    _radius = 130000;
                    _geoCoordinates = mi9db.tfs_store_details
                        .Where(x => x.storeId != "900" && x.storeId != "901" && x.storeId != "913" &&
                                    x.storeId != storeid).ToList().Select(x =>
                            new GeoCoordinate((double)x.properties_lat, (double)x.properties_long))
                        .Where(x => x.GetDistanceTo(center) < _radius);

                }

                if (_geoCoordinates.Count() == 1)
                {
                    _radius = 45000;
                    _geoCoordinates = mi9db.tfs_store_details
                        .Where(x => x.storeId != "900" && x.storeId != "901" && x.storeId != "913" &&
                                    x.storeId != storeid).ToList().Select(x =>
                            new GeoCoordinate((double)x.properties_lat, (double)x.properties_long))
                        .Where(x => x.GetDistanceTo(center) < _radius);

                }

                var getdescription = mi9db.products
                    .Join(mi9db.productcodes, products => products.prodint, 
                        productcodes => productcodes.prodint, 
                        (products, productcodes) => new {products, productcodes})
                    .Where(t => t.products.prodcode == productCode)
                    .Select(t => t.products).FirstOrDefault();

                if (getdescription != null)
                    _description = getdescription.proddesc;
                if (getdescription == null)
                    _description = "Product description does not exist.";

                #endregion

                //Create the stores list here
                var storesList = new List<StockLocationVM.AbstractSubStockLocationVM>(5);
                foreach (var item in _geoCoordinates)
                {
                    #region CoOrdinates For List Stores
                    var latitue = item.Latitude;
                    var longi = item.Longitude;
                    int numberOfrecords = 5;
                    var getstoreslist = mi9db.tfs_store_details
                        .Where(x => x.properties_lat == (decimal?)latitue && x.properties_long == (decimal?)longi
                                                                           && x.storeId != storeid).Take(5);
                    #endregion

                    foreach (var stores in getstoreslist)
                    {
                        var listModel = new StockLocationVM.AbstractSubStockLocationVM();

                        #region BrnStocks for List Stores
                        var getVarint = mi9db.productcodes.FirstOrDefault(x => x.variantcode == productCode).varint;

                        var getbrnStock = mi9db.brnstocks
                            .FirstOrDefault(x => x.varint == getVarint
                                                 && x.branchcode == stores.storeId);

                        if (getbrnStock != null)
                            _getbrnStockLists = getbrnStock.retail;
                        if (getbrnStock == null)
                            _getbrnStockLists = 0;

                        var stockToStore = mi9db.brnstocks
                            .FirstOrDefault(x => x.varint == getVarint
                                                 && x.branchcode == stores.storeId);
                        if (stockToStore != null)
                            _stocktoStore = stockToStore.allocated;
                        if (stockToStore == null)
                            _stocktoStore = 0;
                        #endregion

                        #region Model List Mapping

                        listModel.store_id = stores.storeId;

                        listModel.description = _description;
                        listModel.store_name = stores.store_name;
                        if (stores.properties_lat != null) listModel.lat = (double)stores.properties_lat;
                        if (stores.properties_long != null) listModel.longi = (double)stores.properties_long;
                        listModel.stock_qty = Math.Max(0, _getbrnStockLists);
                        listModel.stock_in_transit_to_remote_store = _stocktoStore;
                        if (storesList.Count < 5)
                        {
                            storesList.Add(listModel);
                        }
                        
                        #endregion

                    }
                }
                return PartialView("EditorTemplates/_StockEditor", storesList);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }


        public ActionResult ShowActive()
        {
            return View();
        }

        // POST: Stock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RowId,storeId,properties_long,properties_lat,store_name,store_address,isMeraki")] tfs_store_details tfs_store_details)
        {
            if (ModelState.IsValid)
            {
                mi9db.tfs_store_details.Add(tfs_store_details);
                await mi9db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tfs_store_details);
        }

        // GET: Stock/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tfs_store_details tfs_store_details = await mi9db.tfs_store_details.FindAsync(id);
            if (tfs_store_details == null)
            {
                return HttpNotFound();
            }
            return View(tfs_store_details);
        }

        // POST: Stock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RowId,storeId,properties_long,properties_lat,store_name,store_address,isMeraki")] tfs_store_details tfs_store_details)
        {
            if (ModelState.IsValid)
            {
                mi9db.Entry(tfs_store_details).State = EntityState.Modified;
                await mi9db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tfs_store_details);
        }

        // GET: Stock/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tfs_store_details tfs_store_details = await mi9db.tfs_store_details.FindAsync(id);
            if (tfs_store_details == null)
            {
                return HttpNotFound();
            }
            return View(tfs_store_details);
        }

        // POST: Stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tfs_store_details tfs_store_details = await mi9db.tfs_store_details.FindAsync(id);
            mi9db.tfs_store_details.Remove(tfs_store_details);
            await mi9db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult GetStoreDetails()
        {
            try
            {
                var query = (from stores in mi9db.tfs_store_details
                    select stores);
                // JsonObj = JsonConvert.SerializeObject(query);
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mi9db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
