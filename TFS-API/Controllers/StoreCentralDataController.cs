using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using TFS_API.Service.Interface;
using TFS_API.Services.Interfaces;

namespace TFS_API.Controllers
{
    [RoutePrefix("api/v1/Stores_Cental_Data")]
    public class StoreCentralDataController : ApiController
    {
        private readonly IScannerService _scannerService;
        private readonly IExceptionService _errorService;

        public StoreCentralDataController(IScannerService scannerService,
            IExceptionService errorService)
        {
            _scannerService = scannerService;
            _errorService = errorService;
        }

        [HttpGet]
        [Route("GetInterStoreList/{storeId}")]
        public HttpResponseMessage GetStoretoStore([FromUri]string storeId) {
            try {
                var getData = _scannerService.GetInterStoreListStoresCentral(int.Parse(storeId));
                var getStores = _scannerService.GetStores();
                if (!getData.Any())
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No IST history for store " + storeId);

                //if (store.Length == 1)
                //{
                //    store = "00" + store;
                //}
                //if (store.Length == 2)
                //{
                //    store = "0" + store;
                //}

                foreach (var item in getData)
                {

                    if (item.Destination_Id.Length == 1)
                    {
                        item.Destination_Id = "00" + item.Destination_Id;
                    }
                    if (item.Destination_Id.Length == 2)
                    {
                        item.Destination_Id = "0" + item.Destination_Id;
                    }
                }
                //var dest = (from a in getData
                //    join b in getStores on a.Destination_Id equals b.store_id
                //    select b.store_name);

                var destinationStore = getData.Join(getStores, a => a.Destination_Id, b => b.store_id,
                    (a, b) => b.store_name);

                var sourceStore = getData.Join(getStores, a => a.Source_Id, b => b.store_id,

                    (a, b) => b.store_name);
                var status = "In Transit";
                if (getData.First().IsDelivered == true)
                    status = "Accepted";

                var data = getData
                    .OrderBy(x => x.Pallet_Number)
                    .GroupBy(x => x.Pallet_Number)
                    .Select(g => new
                    {
                        consignment = g.Key,
                        sourceName = sourceStore.FirstOrDefault(),
                        destination = destinationStore.FirstOrDefault(),
                        status = status,
                        shippeddatetime = g.Select(x => x.Shipment_Date).FirstOrDefault()
                    });
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex) {
                _errorService.Capture(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERROR HERE" + storeId);
            }
        }

        [HttpGet]
        [Route("GetInterStoreDetail/{consignment}")]
        public HttpResponseMessage GetStoretoStoreDetails(string consignment)
        {
            var getData = _scannerService.GetInterStoreDetail(consignment);


            var data = getData
                .Select(a => 
                    new 
                        {sku = a.SKU, 
                            qty = a.Qty, 
                            description = a.Description}).ToList();
           
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
    }
}
