using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PICountDataAccessLayer;

namespace TFS_API.Repositorys
{
    public class PIRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private UnitOfWork _unitOfWork;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public PIRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Update(tbl_pi_count record)
        {
            _unitOfWork.PIDbContext.Entry(record).State = EntityState.Modified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<tbl_pi_count> GetAllCounts()
        {
            return _unitOfWork.PIDbContext.tbl_pi_count.ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<tbl_sku_count> GetSkusByCountId(int id)
        {
            return _unitOfWork.PIDbContext.tbl_sku_count.Where(x => x.list_id == id).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<tbl_pi_count_stores> GetStoresByListId(int id)
        {
            return _unitOfWork.PIDbContext.tbl_pi_count_stores.Where(x => x.list_id == id).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public List<tbl_pi_count_stores> GetOpenCountsByStore(string store)
        {
            return _unitOfWork.PIDbContext.tbl_pi_count_stores.Where(x => x.storeid == store).ToList();
        }
        /// <summary>
        /// Gets Count By ID
        /// </summary>
        /// <param name="RowID"></param>
        /// <returns></returns>
        public tbl_pi_count GetByID(int RowID)
        {
            return _unitOfWork.PIDbContext.tbl_pi_count.FirstOrDefault(x => x.rowid == RowID);
        }
    }
}