using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mi9DataAccessLayer;
using TFS_API.Services.Interfaces;

namespace TFS_API.Services
{
    public class StockService : IStockService
    {
        public Mi9DBEntities mi9db = new Mi9DBEntities();
        private readonly IExceptionService _errorService;

        public StockService(IExceptionService errorService)
        {
            _errorService = errorService;

        }

        public bool CheckStock(string storeId, int varint)
        {
            try
            {
                var checkStock = mi9db.brnstocks.FirstOrDefault(x => x.branchcode == storeId && x.varint == varint);

                if (checkStock.retail <= 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }
    }
}