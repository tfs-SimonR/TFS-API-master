using HHTTaskDAL;
using Mi9DataAccessLayer;
using mi9TESTDAL;
using ProcessedDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using TFS_API.Helpers;
using TFS_API.Models;
using TFS_API.Models.BindingModels;
using TFS_API.Models.DTO;
using TFS_API.Models.ViewModels;
using TFS_API.Services.Interfaces;

namespace TFS_API.Services
{
    /// <inheritdoc />
    public class HandyScannerService : IHandyScannerService
    {
        #region Setup
        /*Setup DB context*/
        public ApplicationDbContext db = new ApplicationDbContext();
        private readonly TaskDBContext taskDb = new TaskDBContext();
        public ProDbContext ProcessedDb = new ProDbContext();
        public Mi9DBEntities mi9db = new Mi9DBEntities();
        public mi9_mms_fs_trainEntities mi9TestEntities = new mi9_mms_fs_trainEntities();
        private readonly IExceptionService _errorService;
        private readonly ICountingService _countingService;
        private readonly IStockService _stockService;
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
        private double _lat;
        private double _lng;
        private string _description;
        private int _radius;
        private int _sevenDay;
        private int? _currentArea;
        private int? _result30;
        private int? _get7DaySales;
        private IEnumerable<GeoCoordinate> _geoCoordinates;
        #endregion

        #region Constructors

        /// <inheritdoc />
        public HandyScannerService(
            IExceptionService errorService, 
            ICountingService countingService,
            IStockService stockService)
        {
            _errorService = errorService;
            _countingService = countingService;
            _stockService = stockService;
        }
        #endregion

        /// <summary>
        /// Gets GoodsOut created in past 28 days.
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public List<PreviousGoodsOutDTO> GetPreviousGoodsOut(string storeId)
        {
            try
            {
                var currenteDate = DateTime.UtcNow.Date.AddDays(-2);

                var entity = (from consighdrs in mi9db.consighdrs
                    join consigdests in mi9db.consigdests on consighdrs.consignment equals consigdests.consignment
                    join consigliness in mi9db.consiglines on consigdests.condestint equals consigliness.condestint
                    join productcodess in mi9db.productcodes on consigliness.varint equals productcodess.varint
                    join products in mi9db.products on productcodess.prodint equals products.prodint
                    where consigdests.warehouse == storeId && consigdests.consignment.Contains("GDO") && consighdrs.createdate >= currenteDate
                    group new { consigdests, productcodess, consigliness, products } by consigdests.consignment into grp
                    select new PreviousGoodsOutDTO
                    {
                        pallet_identifier = grp.Key,
                        destination = grp.Select(x => x.consigdests.destination).FirstOrDefault(),
                        shipment_items = grp.Select(a => new PreviousGoodsOutDTO.GoodsOutPreviousItems
                        {
                            sku = a.productcodess.variantcode,
                            stock_qty = a.consigliness.issueqty,
                            description = a.products.proddesc,
                            varint = a.productcodess.varint

                        }).ToList()

                    }).ToList();

               

                return entity;
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }

        }

        public List<PreviousWarhouseDeliveriesDTO> GetPreviousWarehouseGoodsOut(string storeId)
        {
            try
            {
                var startDate = DateTime.UtcNow.Date.AddDays(-7);
                DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59

                var entity = (from consighdrs in mi9db.consighdrs
                    join consigdests in mi9db.consigdests on consighdrs.consignment equals consigdests.consignment
                    join consigliness in mi9db.consiglines on consigdests.condestint equals consigliness.condestint
                    join productcodess in mi9db.productcodes on consigliness.varint equals productcodess.varint
                    join products in mi9db.products on productcodess.prodint equals products.prodint
                    where consigdests.destination == storeId 
                          && consighdrs.createdate >= startDate
                          && consighdrs.createdate < endDateTime 
                          && consighdrs.status == "D"
                    group new { consighdrs, consigdests, productcodess, consigliness, products } by consigdests.consignment into grp
                    select new PreviousWarhouseDeliveriesDTO
                    {
                        pallet_identifier = grp.Key,
                        booked_in_date = grp.Select(x => x.consighdrs.confirmdate).FirstOrDefault(),
                        destination = grp.Select(x => x.consigdests.destination).FirstOrDefault(),
                        shipment_items = grp.Select(a => new PreviousWarhouseDeliveriesDTO.WarehouseItems
                        {
                            sku = a.productcodess.variantcode,
                            stock_qty = a.consigliness.issueqty,
                            description = a.products.proddesc,
                            varint = a.productcodess.varint
                        }).ToList()
                    }).ToList().OrderBy(z=> z.booked_in_date);

                foreach (var item in entity)
                foreach (var varint in item.shipment_items)
                {
                    var checkStock = mi9db.brnstocks.FirstOrDefault(b => b.branchcode == storeId && b.varint == varint.varint);
                    if (checkStock == null)
                        varint.priority_pick = true;
                    if (checkStock != null && checkStock.retail <= 0)
                        varint.priority_pick = true;
                }
                
                return new List<PreviousWarhouseDeliveriesDTO>(entity);
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }

        }

        /// <summary>
        /// /Warehouse to Store List
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public List<GoodInWarehouseBM> GetWarehouseToStoreList(string storeId)
        {
            try
            {
                if (storeId != "241")
                {
                    DateTime startDateTime = DateTime.Today.AddDays(-365); //Today at 00:00:00
                    DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
                    List<GoodInWarehouseBM> entity = null;

                    var getConsignmentbookedin = ProcessedDb.hs_processed_consighdrs
                        .Where(x => x.storeid == storeId && x.date_bookedin >= startDateTime && x.date_bookedin <= endDateTime)
                        .Select(x => x.consignment).ToList();
                    
                    if (getConsignmentbookedin.Any())
                    {
                        entity = GoodInWarehouseBms(storeId, getConsignmentbookedin);
                        foreach (var item in entity)
                        foreach (var varint in item.shipment_items)
                        {
                            var checkStock = mi9db.brnstocks.FirstOrDefault(b =>
                                b.branchcode == storeId && b.varint == varint.varint);
                            if (checkStock == null)
                                varint.priority_pick = false;
                            if (checkStock != null && checkStock.retail <= 0)
                                varint.priority_pick = true;
                        }
                    }
                    else
                    {
                        entity = Entity(storeId);
                    }
                    return entity;
                }
                else
                {
                    DateTime startDateTime = DateTime.Today.AddDays(-365); //Today at 00:00:00
                    DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
                    List<GoodInWarehouseBM> entity = null;

                    var getConsignmentbookedin = ProcessedDb.hs_processed_consighdrs
                        .Where(x => x.storeid == storeId && x.date_bookedin >= startDateTime && x.date_bookedin <= endDateTime)
                        .Select(x => x.consignment).ToList();

                    if (getConsignmentbookedin.Any())
                    {
                        entity = new List<GoodInWarehouseBM>((from consighdrs in mi9TestEntities.consighdrs
                                                              join consigdests in mi9TestEntities.consigdests on consighdrs.consignment equals consigdests
                                                                  .consignment
                                                              join consigliness in mi9TestEntities.consiglines on consigdests.condestint equals consigliness
                                                                  .condestint
                                                              join productcodess in mi9TestEntities.productcodes on consigliness.varint equals productcodess.varint
                                                              join products in mi9TestEntities.products on productcodess.prodint equals products.prodint
                                                              where consigdests.destination == storeId && consighdrs.status == "T" && consighdrs.warehouse == "900"
                                                              group new { consigdests, productcodess, consigliness, products } by consigdests.consignment into grp
                                                              select new GoodInWarehouseBM
                                                              {
                                                                  pallet_identifier = grp.Key,
                                                                  shipment_items = grp.Select(a => new GoodInWarehouseBM.ShipmentItems
                                                                  {
                                                                      sku = a.productcodess.variantcode,
                                                                      stock_qty = a.consigliness.issueqty,
                                                                      description = a.products.proddesc

                                                                  }).ToList()

                                                              }).Where(i => getConsignmentbookedin.All(z => z != i.pallet_identifier)).ToList());

                    }
                    else
                    {
                        entity = (from consighdrs in mi9TestEntities.consighdrs
                                  join consigdests in mi9TestEntities.consigdests on consighdrs.consignment equals consigdests
                                      .consignment
                                  join consigliness in mi9TestEntities.consiglines on consigdests.condestint equals consigliness
                                      .condestint
                                  join productcodess in mi9TestEntities.productcodes on consigliness.varint equals productcodess.varint
                                  join products in mi9TestEntities.products on productcodess.prodint equals products.prodint
                                  where consigdests.destination == storeId && consighdrs.status == "T" && consighdrs.warehouse == "900"
                                  group new { consigdests, productcodess, consigliness, products } by consigdests.consignment into grp
                                  select new GoodInWarehouseBM
                                  {
                                      pallet_identifier = grp.Key,
                                      shipment_items = grp.Select(a => new GoodInWarehouseBM.ShipmentItems
                                      {
                                          sku = a.productcodess.variantcode,
                                          stock_qty = a.consigliness.issueqty,
                                          description = a.products.proddesc

                                      }).ToList()

                                  }).ToList();
                    }
                    return entity;

                }
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }

        private List<GoodInWarehouseBM> Entity(string storeId)
        {
            if (storeId == "078" || storeId == "120")
            {
                List<GoodInWarehouseBM> entity;
                entity = (from consighdrs in mi9db.consighdrs
                    join consigdests in mi9db.consigdests on consighdrs.consignment equals consigdests
                        .consignment
                    join consigliness in mi9db.consiglines on consigdests.condestint equals consigliness
                        .condestint
                    join productcodess in mi9db.productcodes on consigliness.varint equals productcodess.varint
                    join products in mi9db.products on productcodess.prodint equals products.prodint
                    where consigdests.destination == storeId && consighdrs.status == "T" &&
                          consighdrs.warehouse == "900"
                    group new { consigdests, productcodess, consigliness, products } by consigdests.consignment
                    into grp
                    select new GoodInWarehouseBM
                    {
                        pallet_identifier = grp.Key,
                        shipment_items = grp.Select(a => new GoodInWarehouseBM.ShipmentItems
                            {
                                sku = a.productcodess.variantcode,
                                stock_qty = a.consigliness.issueqty,
                                description = a.products.proddesc
                            }).Distinct().ToList()
                    }).ToList();
                return entity;
            }
            else
            {
                List<GoodInWarehouseBM> entity;
                entity = (from consighdrs in mi9db.consighdrs
                    join consigdests in mi9db.consigdests on consighdrs.consignment equals consigdests
                        .consignment
                    join consigliness in mi9db.consiglines on consigdests.condestint equals consigliness
                        .condestint
                    join productcodess in mi9db.productcodes on consigliness.varint equals productcodess.varint
                    join products in mi9db.products on productcodess.prodint equals products.prodint
                    where consigdests.destination == storeId && consighdrs.status == "T" &&
                          consighdrs.warehouse == "900"
                    group new { consigdests, productcodess, consigliness, products } by consigdests.consignment
                    into grp
                    select new GoodInWarehouseBM
                    {
                        pallet_identifier = grp.Key,
                        shipment_items = grp.Select(a => new GoodInWarehouseBM.ShipmentItems
                            {
                                sku = a.productcodess.variantcode,
                                stock_qty = a.consigliness.issueqty,
                                description = a.products.proddesc
                            }).OrderBy(t => t.priority_pick)
                            .Distinct().ToList()
                    }).ToList();
                return entity;
            }
           
        }

        public virtual List<GoodInWarehouseBM> GoodInWarehouseBms(string storeId, List<string> getConsignmentbookedin)
        {
            List<GoodInWarehouseBM> entity;
            entity = new List<GoodInWarehouseBM>((from consighdrs in mi9db.consighdrs
                join consigdests in mi9db.consigdests on consighdrs.consignment equals consigdests
                    .consignment
                join consigliness in mi9db.consiglines on consigdests.condestint equals consigliness
                    .condestint
                join productcodess in mi9db.productcodes on consigliness.varint equals productcodess.varint
                join products in mi9db.products on productcodess.prodint equals products.prodint
                where consigdests.destination == storeId && consighdrs.status == "T" && consighdrs.warehouse == "900"
                group new {consigdests, productcodess, consigliness, products} by consigdests.consignment
                into grp
                select new GoodInWarehouseBM
                {
                    pallet_identifier = grp.Key,
                    shipment_items = grp.Select(a => new GoodInWarehouseBM.ShipmentItems
                        {
                            sku = a.productcodess.variantcode,
                            stock_qty = a.consigliness.issueqty,
                            description = a.products.proddesc,
                            varint = a.productcodess.varint,
                            priority_pick = false
                        }).OrderBy(t => t.priority_pick)
                        .ToList()
                }).Where(i => getConsignmentbookedin.All(z => z != i.pallet_identifier)).ToList());
            return entity;
        }

        /// <summary>
        /// Supplier to Store List
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public async Task <List<GoodsInInterStoresBM>> GetSupplierToStoresList(string storeId)
        {
            try
            {
                Log.Information("GetSupplierToStoresList" + storeId);
                
                DateTime startDateTime = DateTime.Today.AddDays(-280); //Today at 00:00:00
                DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59

                var getConsignmentbookedin = await ProcessedDb.hs_processed_purchorders
                   .Where(x => x.storeid == storeId && x.date_booked_in >= startDateTime && x.date_booked_in <= endDateTime)
                   .Select(x => x.ponumber).AsQueryable().ToListAsync();

                List<GoodsInInterStoresBM> entity = null;

                if (getConsignmentbookedin.Any())
                {
                   entity = new List<GoodsInInterStoresBM>(( from ponumbers in  mi9db.purchorders
                                                             join purchorderline in  mi9db.purchordlines on ponumbers.transint equals purchorderline.transint
                                                             join productcodes in  mi9db.productcodes on purchorderline.varint equals productcodes.varint
                                                             where ponumbers.location == storeId && ponumbers.status.ToString() == "0" 
                                                                                                 && ponumbers.lastdeldate == null 
                                                                                                 && purchorderline.status == 0
                                                             group new { ponumbers, purchorderline, productcodes } by ponumbers.ordernumber into grp
                                                             select new GoodsInInterStoresBM
                                                             {
                                                                 delivery_number = grp.Key,
                                                                 expected_date = grp.Select(x => x.ponumbers.deliverydate).FirstOrDefault(),
                                                                 shipment_items = grp.Select(a => new GoodsInInterStoresBM.ShipmentItems
                                                                 {
                                                                     sku = a.productcodes.variantcode,
                                                                     sku_qty = a.purchorderline.quantity,
                                                                     varint = a.productcodes.varint
                                                                     
                                                                 }).ToList()

                                                             }).Where(i => getConsignmentbookedin.All(z => z != i.delivery_number)).ToList());
                    foreach (var item in entity)
                    {
                        foreach (var varint in item.shipment_items)
                        {
                            var checkStock = mi9db.brnstocks.FirstOrDefault(b => b.branchcode == storeId && b.varint == varint.varint);
                            if (checkStock == null)
                                varint.priority_pick = false;
                            if (checkStock != null && checkStock.retail <= 0)
                                varint.priority_pick = true;
                           
                        }
                    }
                }
                else
                {
                    entity = (from ponumbers in mi9db.purchorders
                              join purchorderline in mi9db.purchordlines on ponumbers.transint equals purchorderline.transint
                              join productcodes in mi9db.productcodes on purchorderline.varint equals productcodes.varint
                              where ponumbers.location == storeId && ponumbers.status.ToString() == "0" 
                                                                  && ponumbers.lastdeldate == null 
                                                                  && purchorderline.status == 0
                              group new { ponumbers, purchorderline, productcodes } by ponumbers.ordernumber into grp
                              select new GoodsInInterStoresBM
                              {
                                  delivery_number = grp.Key,
                                  expected_date = grp.Select(x => x.ponumbers.deliverydate).FirstOrDefault(),
                                  shipment_items = grp.Select(a => new GoodsInInterStoresBM.ShipmentItems
                                  {
                                      sku = a.productcodes.variantcode,
                                      sku_qty = a.purchorderline.quantity
                                  }).ToList()

                              }).ToList();

                }
                return entity;

            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;

            }

        }

        /// <summary>
        /// Store to Store List 
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public List<StoreToStoreBM> GetStoreToStoreList(string storeId)
        {
            try
            {
                DateTime startDateTime = DateTime.Today.AddDays(-365);  //Today at 00:00:00
                DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
                var getConsignmentbookedin = ProcessedDb.hs_processed_interstore
                    .Where(x => x.storeid == storeId && x.date_booked_in >= startDateTime && x.date_booked_in <= endDateTime)
                    .Select(x => x.gdonumber).ToList();

                List<StoreToStoreBM> data = null;

                    if (getConsignmentbookedin.Any())
                    {
                        data = new List<StoreToStoreBM>((from consighdrs in mi9db.consighdrs
                                                         join consigdestinations in mi9db.consigdests on consighdrs.consignment equals consigdestinations.consignment
                                                         join consignmentlines in mi9db.consiglines on consigdestinations.condestint equals consignmentlines.condestint
                                                         join prodictcodes in mi9db.productcodes on consignmentlines.varint equals prodictcodes.varint
                                                         join products in mi9db.products on prodictcodes.prodint equals products.prodint
                                                         where consighdrs.status == "T" && consigdestinations.destination == storeId && consigdestinations.destination != "900" && consigdestinations.destination != "901"
                                                               && consighdrs.warehouse != "900" && consigdestinations.desttype != "03" &&
                                                               consigdestinations.desttype != "04"
                                                         group new { consigdestinations, consighdrs, products, prodictcodes, consignmentlines } by consigdestinations.consignment into grp
                                                         select new StoreToStoreBM
                                                         {
                                                             gdo_number = grp.Key,
                                                             shipment_items = grp.Select(x => new StoreToStoreBM.ShipmentItems
                                                             {
                                                                 store_origin = x.consighdrs.warehouse,
                                                                 store_destination = x.consigdestinations.destination,
                                                                 description = x.products.proddesc,
                                                                 sku = x.prodictcodes.variantcode,
                                                                 sku_qty = x.consignmentlines.issueqty,
                                                                 //priority_pick = false
                                                             }).ToList()

                                                         }).Where(i => getConsignmentbookedin.All(z => z != i.gdo_number)).ToList());

                        foreach (var item in data)
                        {
                            foreach (var varint in item.shipment_items)
                            {
                                var checkStock = mi9db.brnstocks.FirstOrDefault(b => b.branchcode == storeId && b.varint == varint.varint);
                                if (checkStock == null)
                                    varint.priority_pick = false;
                                if (checkStock != null && checkStock.retail <= 0)
                                    varint.priority_pick = true;
                            }
                        }

                    }
                    else
                    {
                        data = (from consighdrs in mi9db.consighdrs
                            join consigdestinations in mi9db.consigdests on consighdrs.consignment equals consigdestinations.consignment
                            join consignmentlines in mi9db.consiglines on consigdestinations.condestint equals consignmentlines.condestint
                            join prodictcodes in mi9db.productcodes on consignmentlines.varint equals prodictcodes.varint
                            join products in mi9db.products on prodictcodes.prodint equals products.prodint
                            where consighdrs.status == "T" && consigdestinations.destination == storeId && consigdestinations.destination != "900" && consigdestinations.destination != "901"
                                  && consighdrs.warehouse != "900" && consigdestinations.desttype != "03" &&
                                  consigdestinations.desttype != "04"
                            group new { consigdestinations, consighdrs, products, prodictcodes, consignmentlines } by consigdestinations.consignment into grp
                            select new StoreToStoreBM
                            {
                                gdo_number = grp.Key,
                                shipment_items = grp.Select(x => new StoreToStoreBM.ShipmentItems
                                {
                                    store_origin = x.consighdrs.warehouse,
                                    store_destination = x.consigdestinations.destination,
                                    description = x.products.proddesc,
                                    sku = x.prodictcodes.variantcode,
                                    sku_qty = x.consignmentlines.issueqty
                                }).ToList()

                            }).ToList();

                    }
                    return data;

            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;

            }
        }

        public List<SupplierListBM> GetSupplierList()
        {
            try
            {
                var getSupplierList = (from suppliers in mi9db.suppliers
                    select new SupplierListBM()
                    {
                        supplierid = suppliers.supplier1,
                        short_name = suppliers.shortname,
                        supplier_name = suppliers.name
                    }).ToList();
                return getSupplierList;

            }
            catch (Exception e)
            {
                _errorService.Capture(e);
                throw;
            }
        }

        /// <summary>
        /// Gets list of all stores in the estate
        /// </summary>
        /// <returns></returns>
        public List<StoresListBM> GetStoresList()
        {
            try
            {
                var getStoresList = (from storeslist in mi9db.tfs_store_details
                                     where storeslist.storeId != "999" && storeslist.storeId != "241"
                    select new StoresListBM
                    {
                        store_id = storeslist.storeId,
                        store_name = storeslist.store_name,
                        lat = storeslist.properties_lat,
                        longitude = storeslist.properties_long
                        
                    }).ToList();
                return getStoresList;
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
            
        }

        /// <summary>
        /// Gets stock level of SKU and a Branch
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public HandyScannerBM GetStockAtBranch(string productCode, string branchId)
        {
            try
            {
                var query = (from products in mi9db.products
                    join productcodes in mi9db.productcodes on products.prodint equals productcodes.prodint
                    join prodkeys in mi9db.prodkeys on productcodes.varint equals prodkeys.prodint
                    join retailprice in mi9db.TOFS_RetailPrice on productcodes.varint equals retailprice.Varint
                    join structlevel in mi9db.structlevels on products.level3 equals structlevel.level3
                    join brnstocks in mi9db.brnstocks on productcodes.varint equals brnstocks.varint
                    where products.prodcode == productCode && brnstocks.branchcode == branchId
                    select new HandyScannerBM
                    {
                        description = products.proddesc,
                        sku = productcodes.variantcode,
                        barcode = prodkeys.prodaltkey,
                        sellingprice = retailprice.Retail_Price,
                        stock_qty = brnstocks.retail,
                        stk_enroute = mi9db.consigdests
                                     .Join(mi9db.consiglines, consigdests => consigdests.condestint,
                                         consiglines => consiglines.condestint,
                                         (consigdests, consiglines) => new { consigdests, consiglines })
                                     .Where(@t => @t.consigdests.destination == branchId)
                                     .Select(@t => @t.consiglines.InTransitQuantity).FirstOrDefault(),
                        whs_stock = (mi9db.whstocks
                            .Join(mi9db.whlocs, a => a.binint, d => d.binint, (a, d) => new {a, d})
                            .Join(mi9db.productcodes, @t => @t.a.varint, b => b.varint, (@t, b) => new {@t, b})
                            .Join(mi9db.products, @t => @t.b.prodint, c => c.prodint, (@t, c) => new {@t, c})
                            .Where(@t => @t.c.prodcode == productCode && @t.@t.@t.a.binint == 1977280)
                            .Select(@t => @t.@t.@t.a.stock)).FirstOrDefault()
                    }).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }

        }

        /// <summary>
        /// Not used at the moment
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public StoreDistanceDTO GetStockatLocal(string productCode, string branchId)
        {
            try
            {
                var query = (from products in mi9db.products
                             join productcodes in mi9db.productcodes on products.prodint equals productcodes.prodint
                             join prodkeys in mi9db.prodkeys on productcodes.varint equals prodkeys.prodint
                             join retailprice in mi9db.TOFS_RetailPrice on productcodes.varint equals retailprice.Varint
                             join structlevel in mi9db.structlevels on products.level3 equals structlevel.level3
                             join brnstocks in mi9db.brnstocks on productcodes.varint equals brnstocks.varint
                             where products.prodcode == productCode && brnstocks.branchcode == branchId
                             select new StoreDistanceDTO
                             {
                                 storename = mi9db.tfs_store_details.FirstOrDefault(x => x.store_name == branchId).store_name,
                                 description = products.proddesc,
                                 sku = productcodes.variantcode,
                                 barcode = prodkeys.prodaltkey,
                                 sellingprice = retailprice.Retail_Price,
                                 stock_qty = brnstocks.retail,
                                 stk_enroute = mi9db.consigdests
                                     .Join(mi9db.consiglines, consigdests => consigdests.condestint,
                                         consiglines => consiglines.condestint,
                                         (consigdests, consiglines) => new { consigdests, consiglines })
                                     .Where(@t => @t.consigdests.destination == branchId)
                                     .Select(@t => @t.consiglines.InTransitQuantity).FirstOrDefault()
                             }).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }

        /// <summary>
        /// Not used any more.
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public HandyScannerBM GetStockAtBranchBarcode(string barcode, string branchId)
        {
            try
            {
                var query = (from products in mi9db.products
                             join productcodes in mi9db.productcodes on products.prodint equals productcodes.prodint
                             join prodkeys in mi9db.prodkeys on productcodes.varint equals prodkeys.prodint
                             join retailprice in mi9db.TOFS_RetailPrice on productcodes.varint equals retailprice.Varint
                             join structlevel in mi9db.structlevels on products.level3 equals structlevel.level3
                             join brnstocks in mi9db.brnstocks on productcodes.varint equals brnstocks.varint
                             where prodkeys.prodaltkey == barcode && brnstocks.branchcode == branchId
                             select new HandyScannerBM
                             {
                                 description = products.proddesc,
                                 sku = productcodes.variantcode,
                                 barcode = prodkeys.prodaltkey,
                                 sellingprice = retailprice.Retail_Price,
                                 stock_qty = brnstocks.retail,
                                 stk_enroute = (mi9db.consigdests
                                     .Join(mi9db.consiglines, consigdests => consigdests.condestint,
                                         consiglines => consiglines.condestint,
                                         (consigdests, consiglines) => new {consigdests, consiglines})
                                     .Where(@t => @t.consigdests.destination == branchId)
                                     .Select(@t => @t.consiglines.InTransitQuantity)).FirstOrDefault()
                                 
                             }).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }

        }

        /// <summary>
        /// Gets All Stock on way to a store.
        /// </summary>
        /// <param name="storeid"></param>
        /// <returns></returns>
        public List<AllStockEnRouteDTO> GetAllStockEnRoute(string storeid)
        {
            try
            {
                var data = (from consighdrs in mi9db.consighdrs
                            join consigdestinations in mi9db.consigdests on consighdrs.consignment equals
                                consigdestinations.consignment
                            join consignmentlines in mi9db.consiglines on consigdestinations.condestint equals
                                consignmentlines.condestint
                            join productcodes in mi9db.productcodes on consignmentlines.varint equals productcodes
                                .varint
                            join products in mi9db.products on productcodes.prodint equals products.prodint
                            where consigdestinations.destination == storeid &&
                                  consigdestinations.status == "T"
                            group new {consighdrs, consigdestinations, consignmentlines, productcodes, products} by consigdestinations.consignment into grp
                            select new AllStockEnRouteDTO
                            {
                                stock_items = grp.Select(a => new AllStockEnRouteDTO.PalletItems
                                {
                                    description = a.products.proddesc,
                                    sku = a.productcodes.variantcode,
                                    qty_onway = a.consignmentlines.issueqty

                                }).ToList()

                            }).ToList();

                return data;

            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }

        }


        /// <summary>
        /// This is the test method for the new Stock at 5 nearest stores and the weekly sales per sku
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="storeid"></param>
        /// <returns></returns>
        public NearestStoresVM GetNearestStoresTest(string productCode, string storeid)
        {
            try
            {
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
                var storesList = new List<NearestStoresVM.ListOfStores>(5);
                var areaList = new List<NearestStoresVM.AreaStores>();
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
                        var listModel = new NearestStoresVM.ListOfStores();

                        #region BrnStocks for List Stores
                        var getVarint = _countingService.GetVarInt(productCode);

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
                        var storeClosest = new GeoCoordinate((double) stores.properties_lat, (double)stores.properties_long);
                        double distanceBetween = center.GetDistanceTo(storeClosest);
                        double meterstoKM = distanceBetween / 1000;
                        var kmphtoMiles = DataHelpers.kmphTOmph(meterstoKM);

                        #region Model List Mapping

                        listModel.store_id = stores.storeId;
                        listModel.store_name = stores.store_name;
                        if (stores.properties_lat != null) listModel.lat = (double)stores.properties_lat;
                        if (stores.properties_long != null) listModel.longi = (double)stores.properties_long;
                        listModel.stock_qty = Math.Max(0, _getbrnStockLists);
                        listModel.stock_in_transit_to_remote_store = _stocktoStore;
                        listModel.distance_to_store = Math.Round(kmphtoMiles,2);
                        if (storesList.Count < 5)
                        {
                            storesList.Add(listModel);
                        }
                        
                        #endregion

                    }
                }

                var getAreaStoresList = mi9db.tfs_store_details.Where(x => x.area == _currentArea);

                foreach (var area in getAreaStoresList)
                {

                    var areaModel = new NearestStoresVM.AreaStores();

                    #region BrnStocks for List Stores
                    var getVarint = _countingService.GetVarInt(productCode);

                    var getbrnStock = mi9db.brnstocks
                        .FirstOrDefault(x => x.varint == getVarint
                                             && x.branchcode == area.storeId);

                    if (getbrnStock != null)
                        _getbrnStockLists = getbrnStock.retail;
                    if (getbrnStock == null)
                        _getbrnStockLists = 0;

                    var stockToStore = mi9db.brnstocks
                        .FirstOrDefault(x => x.varint == getVarint
                                             && x.branchcode == area.storeId);
                    if (stockToStore != null)
                        _stocktoStore = stockToStore.allocated;
                    if (stockToStore == null)
                        _stocktoStore = 0;
                    #endregion

                    #region Model List Mapping
                    {

                        areaModel.store_id = area.storeId;
                        areaModel.store_name = area.store_name;
                        if (area.properties_lat != null) areaModel.lat = (double)area.properties_lat;
                        if (area.properties_long != null) areaModel.longi = (double)area.properties_long;
                        areaModel.stock_qty = Math.Max(0, _getbrnStockLists);
                        areaModel.stock_in_transit_to_remote_store = _stocktoStore;
                        areaList.Add(areaModel);
                    }
                    #endregion

                }

                var toplevel = new NearestStoresVM();
                {
                    #region Retail Pricing
                    //CHANGED FROM 6 to 4 to test 
                    if (productCode.Length >= 4)
                    {
                        var getProdint = mi9db.products.FirstOrDefault(x => x.prodcode == productCode);
                        if (getProdint != null)
                            _getProdint = getProdint.prodint;
                        if (getProdint == null)
                            _getProdint = 0;

                        var getvarint = mi9db.productcodes.FirstOrDefault(x => x.prodint == _getProdint);
                        if (getvarint != null)
                            _getVarint = getvarint.varint;

                        var getRetail = mi9db.TOFS_RetailPrice.FirstOrDefault(x => x.Varint == _getVarint);

                        if (getRetail != null)
                            _retailPriceDecimal = getRetail.Retail_Price;
                        if (getRetail == null)
                            _retailPriceDecimal = 0;

                        var barcode = mi9db.prodkeys.FirstOrDefault(x => x.prodint == _getVarint);
                        if (barcode != null)
                            _barcode = barcode.prodaltkey;
                        if (barcode == null)
                            _barcode = "No Barcode or product does not exist.";
                    }
                    #endregion

                    #region BrnStocks
                    var getVarint = _countingService.GetVarInt(productCode);
                    if (getVarint == 0)
                        getVarint = Convert.ToInt32(productCode);


                    var getbrnStockTop = mi9db.brnstocks.FirstOrDefault(x => x.varint == getVarint && x.branchcode == storeid);

                    if (getbrnStockTop != null)
                        _getbrnStockRetail = getbrnStockTop.retail;

                    if (getbrnStockTop == null)
                        _getbrnStockRetail = 0;

                    var getStockinTransit = mi9db.brnstocks
                            .FirstOrDefault(x => x.varint == getVarint
                                                 && x.branchcode == storeid);

                    if (getStockinTransit != null)
                        _getStockInTransit = getStockinTransit.allocated;

                    if (getStockinTransit == null)
                        _getStockInTransit = 0;
                    #endregion

                    #region region Get Previous Sales
                    DateTime today = DateTime.Today;
                    int taxYear = today.Month > 4 || today.Month == 4 && today.Day >= 6
                        ? today.Year : today.Year - 1;
                    DateTime taxYearStart = new DateTime(taxYear, 4, 1);
                    TimeSpan elapsedTaxYear = today - taxYearStart;
                    int days = elapsedTaxYear.Days;
                    int taxYearWeek = (days / 7) + 1;

                    var taxYearWeekString = taxYearWeek.ToString();
                    if (taxYearWeekString.Length == 1)
                        taxYearWeekString = "0" + taxYearWeekString;

                    string YearWeek = taxYear + taxYearWeekString;

                    
                    var getSalesList = ProcessedDb.TOFS_12WeeksSales.Where(x => x.varint == getVarint && x.branchcode == storeid);

                    if (getSalesList.Count() != 0)
                    {
                        var get7DaySales = getSalesList.FirstOrDefault(x => x.salesweek == YearWeek).Sales;
                        _get7DaySales = get7DaySales;

                        var get30DaySales = getSalesList.OrderBy(x => x.salesweek).Take(4);

                        var result30 = get30DaySales.Select(x => x.Sales).Count();
                        _result30 = result30;
                    }
                    else
                    {
                        _get7DaySales = 0;
                        _result30 = 0;
                    }
                    #endregion

                    #region Remove decimal point
                    var truncate = DataHelpers.TruncateDecimal(_retailPriceDecimal, 2);
                    var removedecimalpoint = DataHelpers.ConvertdecimaltoInt(truncate);
                    #endregion

                    #region Warehouse Stock
                    var getWarehouseStock = mi9db.whstocks
                        .Join(mi9db.whlocs, a => a.binint, d => d.binint, (a, d) => new { a, d })
                        .Join(mi9db.productcodes, t => t.a.varint, b => b.varint, (t, b) => new { t, b })
                        .Join(mi9db.products, t => t.b.prodint, c => c.prodint, (t, c) => new { t, c })
                        .Where(t => t.c.prodcode == productCode && t.t.t.a.binint == 1977280)
                        .Select(t => t.t.t.a).FirstOrDefault();

                    if (getWarehouseStock != null)
                        _getWarehouseStk = getWarehouseStock.stock;
                    if (getWarehouseStock == null)
                        _getWarehouseStk = 0;
                    #endregion

                    #region Model Mappings

                    //toplevel.top_two_hundred = true;
                    toplevel.thirty_day = _result30;
                    toplevel.seven_day = _get7DaySales;
                    //storeid
                    toplevel.current_store = storeid;
                    //sku description
                    toplevel.description = _description;
                    //store lat
                    toplevel.current_lat = _currentLat;
                    //store long
                    toplevel.current_long = _currentLong;
                    //stock quantity
                    toplevel.stock_qty = Math.Max(0, _getbrnStockRetail);
                    //wharehouse stpock levels
                    toplevel.whs_stock_qty = _getWarehouseStk;
                    //retail price as int
                    toplevel.retail_price = removedecimalpoint;
                    //bar code 
                    toplevel.barcode = _barcode;
                    //any stock in transit to store
                    toplevel.in_transit_stock_qty = _getStockInTransit;
                    //list of local stores within 15KM
                    toplevel.list_stores = storesList;
                    toplevel.area_stores = areaList;

                    #endregion

                }

                return toplevel;
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }

        /// <summary>
        /// New endpoint to support click and collect
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="storeid"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public StoreWebPanelDTO GetStoreStockWebPanel(string productCode, double longitude, double latitude)
        {
            try
            {
                #region Get Lat and Long calculate stores in radius of 15KM

                _lng = longitude;
                _lat = latitude;
                var radius = 45000000;
                //Current Radius Centre taken from store Lat Long
                if (_lng != null && _lat != null)
                {
                    var center = new GeoCoordinate(_lat, _lng);
                    //Create List of Lat long co ordinates for stores in 25KM range
                    _geoCoordinates = mi9db.tfs_store_details
                        .Where(x => x.storeId != "900" && x.storeId != "901" && x.storeId != "913" 
                                    && x.storeId != "999" && x.storeId != "970" && x.storeId != "971")
                        .ToList().Select(x => new GeoCoordinate((double) x.properties_lat, (double) x.properties_long))
                        .Where(x => x.GetDistanceTo(center) < radius);

                    var geoCoordinates = _geoCoordinates.ToList();
                    #endregion

                    //Create the stores list here
                    var storesList = new List<StoreWebPanelDTO.StoresList>();
                    foreach (var item in geoCoordinates)
                    {
                        #region CoOrdinates For List Stores

                        var latitue = item.Latitude;
                        var longi = item.Longitude;
                        var getstoreslist = mi9db.tfs_store_details
                            .Where(x => x.properties_lat == (decimal?) latitue &&
                                        x.properties_long == (decimal?) longi).ToList();
                        #endregion

                        foreach (var stores in getstoreslist)
                        {
                            var storeslistModel = new StoreWebPanelDTO.StoresList();

                            #region BrnStocks for List Stores

                            var getVarint = _countingService.GetVarInt(productCode);

                            var getbrnStock = mi9db.brnstocks
                                .FirstOrDefault(x => x.varint == getVarint
                                                     && x.branchcode == stores.storeId);

                            if (getbrnStock != null)
                                _getbrnStockLists = getbrnStock.retail;
                            if (getbrnStock == null)
                                _getbrnStockLists = 0;

                            #endregion

                            if (stores.properties_lat != null && stores.properties_long != null)
                            {
                                var storeClosest = new GeoCoordinate((double) stores.properties_lat,
                                    (double) stores.properties_long);
                                var getdistanceBetweenstores = center.GetDistanceTo(storeClosest);
                                var meterstoKm = getdistanceBetweenstores / 1000;
                                var kmphtoMiles = DataHelpers.kmphTOmph(meterstoKm);
                                string address;
                                var storeAddress = (from a in mi9db.TFS_Store_Updated
                                    where a.StoreNumber == stores.storeId
                                    select new
                                    {
                                        address = a.Address1 + ", " + a.Address3
                                    }).FirstOrDefault();
                                address = storeAddress != null ? storeAddress.address : "Not in db";

                                #region Model List Mapping
                                //Build Model for stores array
                                storeslistModel.store_id = stores.storeId;
                                storeslistModel.store_name = stores.store_name;
                                storeslistModel.store_address = address;
                                if (stores.properties_lat != null) storeslistModel.lat = (double) stores.properties_lat;
                                if (stores.properties_long != null) storeslistModel.longi = (double) stores.properties_long;
                                //storeslistModel.stock_qty = Math.Max(3, _getbrnStockLists);
                                storeslistModel.stock_qty =  _getbrnStockLists;
                                storeslistModel.distance_to_store = Math.Round(kmphtoMiles, 2);
                            }

                            //if (storesList.Count < 7) storesList.Add(storeslistModel);
                            var storeHasSpace = stores.store_name.Contains(" ");
                            if (storeHasSpace) stores.store_name = stores.store_name.Replace(" ", "-");
                            storeslistModel.url = stores.store_name;
                            storesList.Add(storeslistModel);
                            #endregion
                        }
                    }

                    var ordereddistance = storesList.Where(s => s.stock_qty > 0).OrderBy(x => x.distance_to_store).Take(5);
                    var storeid = ordereddistance.Select(x => x.store_id).FirstOrDefault();

                    var nearestStoresModel = new StoreWebPanelDTO();
                    {
                        
                        #region BrnStocks

                        var getVarint = _countingService.GetVarInt(productCode);

                        var getbrnStock =
                            mi9db.brnstocks.FirstOrDefault(x => x.varint == getVarint && x.branchcode == storeid);

                        if (getbrnStock != null)
                            _getbrnStockRetail = 3;
                        if (getbrnStock == null)
                            _getbrnStockRetail = 3;

                        var getStockinTransit =
                            mi9db.brnstocks.FirstOrDefault(x => x.varint == getVarint && x.branchcode == storeid);

                        if (getStockinTransit != null)
                            _getStockInTransit = getStockinTransit.allocated;
                        if (getStockinTransit == null)
                            _getStockInTransit = 0;

                        #endregion
                        #region Model Mappings
                        //storeid
                        nearestStoresModel.current_store = storeid;
                        //sku description
                        nearestStoresModel.description = _description;
                        //store lat
                        nearestStoresModel.current_lat = _currentLat;
                        //store long
                        nearestStoresModel.current_long = _currentLong;
                        //stock quantity
                        //nearestStoresModel.stock_qty = Math.Max(0, _getbrnStockRetail);
                        nearestStoresModel.stock_qty = _getbrnStockRetail;
                        //list of local stores within 15KM
                        nearestStoresModel.list_stores = storesList;

                        #endregion
                    }

                    return nearestStoresModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public StoreWebPanelDTOv2 GetStoreStockWebPanelv2(string productCode, double longitude, double latitude)
        {
            try
            {
                #region Get Lat and Long calculate stores in radius of 15KM

                _lng = longitude;
                _lat = latitude;
                var radius = 45000000;
                //Current Radius Centre taken from store Lat Long
                if (_lng != null && _lat != null)
                {
                    var center = new GeoCoordinate(_lat, _lng);
                    //Create List of Lat long co ordinates for stores in 25KM range
                    _geoCoordinates = mi9db.tfs_store_details
                        .Where(x => x.storeId != "900" && x.storeId != "901" && x.storeId != "913"
                                    && x.storeId != "999" && x.storeId != "970" && x.storeId != "971")
                        .ToList().Select(x => new GeoCoordinate((double)x.properties_lat, (double)x.properties_long))
                        .Where(x => x.GetDistanceTo(center) < radius);

                    var geoCoordinates = _geoCoordinates.ToList();
                    #endregion

                    //Create the stores list here
                    var storesList = new List<StoreWebPanelDTOv2.StoresList>();
                    foreach (var item in geoCoordinates)
                    {
                        #region CoOrdinates For List Stores

                        var latitue = item.Latitude;
                        var longi = item.Longitude;
                        var getstoreslist = mi9db.tfs_store_details
                            .Where(x => x.properties_lat == (decimal?)latitue &&
                                        x.properties_long == (decimal?)longi).ToList();
                        #endregion

                        foreach (var stores in getstoreslist)
                        {
                            //instantiate sotres list model
                            var storeslistModel = new StoreWebPanelDTOv2.StoresList();

                            #region BrnStocks for List Stores

                            //get varint from productcode
                            var getVarint = _countingService.GetVarInt(productCode);
                            //get stock from brnstocks table
                            var getbrnStock = mi9db.brnstocks
                                .FirstOrDefault(x => x.varint == getVarint
                                                     && x.branchcode == stores.storeId);
                            //check for null and set the variable
                            if (getbrnStock != null)
                                _getbrnStockLists = getbrnStock.retail;
                            if (getbrnStock == null)
                                _getbrnStockLists = 0;

                            #endregion

                            if (stores.properties_lat != null && stores.properties_long != null)
                            {
                                var storeClosest = new GeoCoordinate((double)stores.properties_lat,
                                    (double)stores.properties_long);
                                var getdistanceBetweenstores = center.GetDistanceTo(storeClosest);
                                var meterstoKm = getdistanceBetweenstores / 1000;
                                var kmphtoMiles = DataHelpers.kmphTOmph(meterstoKm);
                                string address;
                                var storeAddress = (from a in mi9db.TFS_Store_Updated
                                                    where a.StoreNumber == stores.storeId
                                                    select new
                                                    {
                                                        address = a.Address1 + ", " + a.Address3
                                                    }).FirstOrDefault();
                                address = storeAddress != null ? storeAddress.address : "Not in db";

                                #region Model List Mapping
                                //Build Model for stores array
                                storeslistModel.store_id = stores.storeId;
                                storeslistModel.store_name = stores.store_name;
                                storeslistModel.store_address = address;
                                if (stores.properties_lat != null) storeslistModel.lat = (double)stores.properties_lat;
                                if (stores.properties_long != null) storeslistModel.longi = (double)stores.properties_long;
                                
                                storeslistModel.stock_qty = _getbrnStockLists;
                                storeslistModel.distance_to_store = Math.Truncate(kmphtoMiles);
                            }

                            //if (storesList.Count < 7) storesList.Add(storeslistModel);
                            var storeHasSpace = stores.store_name.Contains(" ");
                            if (storeHasSpace) stores.store_name = stores.store_name.Replace(" ", "-");
                            storeslistModel.url = stores.store_name;
                            storesList.Add(storeslistModel);
                            #endregion
                        }
                    }

                    var ordereddistance = storesList.OrderBy(x => x.distance_to_store);
                    var storeid = ordereddistance.Select(x => x.store_id).FirstOrDefault();

                    var root = new StoreWebPanelDTOv2();
                    {
                        #region BrnStocks

                        var getVarint = _countingService.GetVarInt(productCode);

                        var getbrnStock =
                            mi9db.brnstocks.FirstOrDefault(x => x.varint == getVarint && x.branchcode == storeid);

                        if (getbrnStock != null)
                            _getbrnStockRetail = 3;
                        if (getbrnStock == null)
                            _getbrnStockRetail = 3;

                        var getStockinTransit =
                            mi9db.brnstocks.FirstOrDefault(x => x.varint == getVarint && x.branchcode == storeid);

                        var _currentPrice = 0.00m;
                        var getCurrentPrice = mi9db.rpricehdrs.FirstOrDefault(x => x.varint == getVarint);
                        if (getCurrentPrice != null)
                            _currentPrice = getCurrentPrice.retailprice;

                        var _wasPrice = 0.00m;
                        var getWasPrice = mi9db.rpricehists.FirstOrDefault(x => x.varint == getVarint);
                        if (getWasPrice != null)
                            _wasPrice = getWasPrice.retailprice;

                        if (getStockinTransit != null)
                            _getStockInTransit = getStockinTransit.allocated;
                        if (getStockinTransit == null)
                            _getStockInTransit = 0;

                        #endregion

                        #region Model Mappings
                        //sku
                        root.sku = productCode;

                        root.current_price = _currentPrice;

                        root.was_price = _wasPrice;

                        root.retail_price = 0.00m;
                        var saving = (_wasPrice - _currentPrice);
                        if (saving < 0)
                            saving = 0;

                        root.saving = saving;
                        
                        //list of local stores within 15KM
                        root.list_stores = new List<StoreWebPanelDTOv2.StoresList>(new List<StoreWebPanelDTOv2.StoresList>(storesList.Where(s => s.stock_qty > 0).OrderBy(x => x.distance_to_store)).Take(5));

                        #endregion
                    }

                    return root;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }

        /// <summary>
        /// Returns all data needed for store stock and stores within a radius of 15KM, expands to 130KM for Scotland and remote places.
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="storeid"></param>
        /// <returns></returns>
        public NearestStoresVM GetNearestStoresProdcode(string productCode, string storeid)
        {
            try
            {
                #region Get Lat and Long calculate stores in radius of 15KM

                var getcurrentLong =
                    mi9db.tfs_store_details.FirstOrDefault(x => x.storeId != null && x.storeId == storeid);
                if (getcurrentLong != null)
                    _currentLong = getcurrentLong.properties_long;

                var getcurrentLat =
                    mi9db.tfs_store_details.FirstOrDefault(x => x.storeId != null && x.storeId == storeid);
                if (getcurrentLat != null)
                    _currentLat =  getcurrentLat.properties_lat;

                var radius = 15000;
                var center = new GeoCoordinate((double)_currentLat, (double)_currentLong);

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
                var storesList = new List<NearestStoresVM.ListOfStores>();
                foreach (var item in _geoCoordinates)
                {
                    #region CoOrdinates For List Stores
                    var latitue = item.Latitude;
                    var longi = item.Longitude;
                    int numberOfrecords=5;
                    var getstoreslist = mi9db.tfs_store_details
                        .Where(x => x.properties_lat == (decimal?)latitue && x.properties_long == (decimal?)longi && x.storeId != storeid).Take(numberOfrecords);
                    
                    #endregion

                    foreach (var stores in getstoreslist)
                    {
                        var listModel = new NearestStoresVM.ListOfStores();

                        #region BrnStocks for List Stores
                        var getVarint = _countingService.GetVarInt(productCode);

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
                        {
                            listModel.store_id = stores.storeId;
                            listModel.store_name = stores.store_name;
                            if (stores.properties_lat != null) listModel.lat = (double)stores.properties_lat;
                            if (stores.properties_long != null) listModel.longi = (double)stores.properties_long;
                            listModel.stock_qty = Math.Max(0, _getbrnStockLists);
                            listModel.stock_in_transit_to_remote_store = _stocktoStore;
                            if (storesList.Count < 5)
                            {
                                storesList.Add(listModel);
                            }
                            //storesList.Add(listModel);
                        }
                        #endregion

                    }
                }

                var toplevel = new NearestStoresVM();
                {
                    #region Retail Pricing

                    if (productCode.Length >= 6)
                    {
                        var getProdint = mi9db.products.FirstOrDefault(x => x.prodcode == productCode);
                        if (getProdint != null)
                            _getProdint = getProdint.prodint;
                        if (getProdint == null)
                            _getProdint = 0;

                        var getvarint = mi9db.productcodes.FirstOrDefault(x => x.prodint == _getProdint);
                        if (getvarint != null)
                            _getVarint = getvarint.varint;

                        var getRetail = mi9db.TOFS_RetailPrice.FirstOrDefault(x => x.Varint == _getVarint);

                        if (getRetail != null)
                            _retailPriceDecimal = getRetail.Retail_Price;
                        if (getRetail == null)
                            _retailPriceDecimal = 0;

                        var barcode = mi9db.prodkeys.FirstOrDefault(x => x.prodint == _getVarint);
                        if (barcode != null)
                            _barcode = barcode.prodaltkey;
                        if (barcode == null)
                            _barcode = "No Barcode or product does not exist.";
                    }
                    #endregion

                    #region BrnStocks
                    var getVarint = _countingService.GetVarInt(productCode);
                    var getbrnStockTop = mi9db.brnstocks.FirstOrDefault(x => x.varint == getVarint && x.branchcode == storeid);

                    if (getbrnStockTop != null)
                        _getbrnStockRetail = getbrnStockTop.retail;

                    if (getbrnStockTop == null)
                        _getbrnStockRetail = 0;

                    var getStockinTransit = mi9db.brnstocks
                            .FirstOrDefault(x => x.varint == getVarint
                                                 && x.branchcode == storeid);

                    if (getStockinTransit != null)
                        _getStockInTransit = getStockinTransit.allocated;

                    if (getStockinTransit == null)
                        _getStockInTransit = 0;
                    #endregion

                    #region Remove decimal point
                    var truncate = DataHelpers.TruncateDecimal(_retailPriceDecimal, 2);
                    var removedecimalpoint = DataHelpers.ConvertdecimaltoInt(truncate);
                    #endregion

                    #region Warehouse Stock
                    var getWarehouseStock = mi9db.whstocks
                        .Join(mi9db.whlocs, a => a.binint, d => d.binint, (a, d) => new { a, d })
                        .Join(mi9db.productcodes, t => t.a.varint, b => b.varint, (t, b) => new { t, b })
                        .Join(mi9db.products, t => t.b.prodint, c => c.prodint, (t, c) => new { t, c })
                        .Where(t => t.c.prodcode == productCode && t.t.t.a.binint == 1977280)
                        .Select(t => t.t.t.a).FirstOrDefault();

                    if (getWarehouseStock != null)
                        _getWarehouseStk = getWarehouseStock.stock;
                    if (getWarehouseStock == null)
                        _getWarehouseStk = 0;
                    #endregion

                    #region Model Mappings
                    //storeid
                    toplevel.current_store = storeid;
                    //sku description
                    toplevel.description = _description;
                    //store lat
                    toplevel.current_lat = _currentLat;
                    //store long
                    toplevel.current_long = _currentLong;
                    //stock quantity
                    toplevel.stock_qty = Math.Max(0, _getbrnStockRetail);
                    //wharehouse stpock levels
                    toplevel.whs_stock_qty = _getWarehouseStk;
                    //retail price as int
                    toplevel.retail_price = removedecimalpoint;
                    //TODO Need to chack current barcode use with Maggie
                    toplevel.barcode = _barcode;
                    //any stock in transit to store
                    toplevel.in_transit_stock_qty = _getStockInTransit;
                    //list of local stores within 15KM
                    toplevel.list_stores = storesList;
                    #endregion

                }

                return toplevel;
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeid"></param>
        /// <returns></returns>
        public List<HHTActionDTO> GetTaskLists(string storeid)
        {
            var getList = (from tasks in taskDb.tbl_tasks
                           join taskasctions in taskDb.tbl_task_actions on tasks.id equals taskasctions.taskid
                           join actiontype in taskDb.tbl_action_types on taskasctions.actiontypeid equals actiontype.action_id
                           where tasks.storeid == storeid
                           group new { tasks, taskasctions, actiontype } by tasks.decrption
                into grp
                           select new HHTActionDTO
                           {
                               task_id = grp.Select(x => x.tasks.id).FirstOrDefault(),
                               store_id = grp.Select(x => x.tasks.storeid).FirstOrDefault(),
                               description = grp.Key,
                               instructions = grp.Select(x => x.tasks.instructions).FirstOrDefault(),
                               task_actions = grp.Select(x => new HHTActionDTO.TaskActions
                               {
                                   action_type_id = x.actiontype.action_id,
                                   isMandatory = x.taskasctions.isMandatory,
                                   question = x.taskasctions.questions,
                                   param_1 = x.taskasctions.param1,
                                   param_2 = x.taskasctions.param2,
                                   param_3 = x.taskasctions.param3,
                                   param_4 = x.taskasctions.param4

                               }).ToList()

                           }).ToList();

            return getList;
        }

        

    }
}