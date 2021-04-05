using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Serilog;
using TFS_API.Service.Interface;
using TFS_API.Services.Interfaces;

namespace TFS_API.Controllers
{
    public class StockCorrectionController : ApiController
    {
        private readonly IExceptionService _errorService;
        private readonly IScannerService _scannerService;

        public StockCorrectionController(IExceptionService errorService,
            IScannerService scannerService)
        {
            _errorService = errorService;
            _scannerService = scannerService;
        }


        //[Route("api/tfs-onret/v1/insert")]
        //[HttpPost]
        //public IHttpActionResult PostAdhocToMi9([FromBody] ReturnHistoryDto detail)
        //{
        //    try
        //    {
        //        Log.Information("InsertReturn: Starting");
        //        var result = new ReturnHistoryBo().InsertReturn(detail);
        //        Log.Information($"InsertReturn: {result}");
        //        return Ok(result);
        //    }
        //    catch (Exception exception)
        //    {
        //        Log.Error(exception, $"An error occured in InsertReturn ({detail.RequestId})");
        //        return InternalServerError(exception);
        //    }
        //}
    }
}
