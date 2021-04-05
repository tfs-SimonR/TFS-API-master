using System;
using System.Linq;

using TFS_API.Models;
using TFS_API.Services.Interfaces;
using Mi9DataAccessLayer;
using brnstock = Mi9DataAccessLayer.brnstock;

namespace TFS_API.Services
{
    public class CountingService : ICountingService
    {
        #region Setup

        /*Setup DB context*/
        public ApplicationDbContext db = new ApplicationDbContext();

        private readonly Mi9DBEntities _mi9Db; 
        private readonly IExceptionService _errorService;

        #endregion

        /// <summary>
        ///     Counitng Service
        /// </summary>
        /// <param name="errorService"></param>
        public CountingService(IExceptionService errorService,
            Mi9DBEntities mi9Db)
        {
            _errorService = errorService;
            _mi9Db = mi9Db;
        }


        /// <summary>
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="store_id"></param>
        /// <returns></returns>
        public brnstock GetBrnstock(int getvarint, string store_id)
        {
            try
            {
                var getRecord = _mi9Db.brnstocks
                    .Where(x => x.varint == getvarint).FirstOrDefault(x => x.branchcode == store_id);
                return getRecord;
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public int GetVarInt(string sku)
        {
            try
            {
                var getvarint = _mi9Db.productcodes.FirstOrDefault(x => x.variantcode == sku);
                if (getvarint != null)
                {
                    return getvarint.varint;

                }
                else
                {
                    return 0;
                }
                
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }
    }
}