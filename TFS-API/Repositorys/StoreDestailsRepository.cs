using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mi9DataAccessLayer;

namespace TFS_API.Repositorys
{
    /// <summary>
    /// 
    /// </summary>
    public class StoreDestailsRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private UnitOfWork _unitOfWork;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public StoreDestailsRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public TFS_Store_Updated GetStoreDetails(string storeId)
        {
            return _unitOfWork.MI9DBContext.TFS_Store_Updated.FirstOrDefault(x => x.StoreNumber == storeId);
        }

        public IQueryable<TFS_Store_Updated> GetAllStores()
        {
            return _unitOfWork.MI9DBContext.TFS_Store_Updated;
        }
    }
}