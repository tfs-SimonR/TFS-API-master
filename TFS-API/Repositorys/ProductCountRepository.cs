using System;
using ClickAndCollectDAL;
using Mi9DataAccessLayer;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PICountDataAccessLayer;
using TFS_API.Repositorys.Interfaces;

namespace TFS_API.Repositorys
{
    public class ProductCountRepository : IProductCountRepository, IDisposable
    {
        private UnitOfWork _unitOfWork;

        public ProductCountRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(tbl_product_count entity)
        {
            _unitOfWork.PIDbContext.tbl_product_count.Add(entity);
        }

        public void Update(tbl_product_count entity)
        {
            _unitOfWork.PIDbContext.Entry(entity).State = EntityState.Modified;
        }

        //public void InsertOrUpdate(tbl_product_count blog)
        //{
        //      _unitOfWork.PIDbContext.Entry(blog).State = blog.sku == 0 ?
        //            EntityState.Added :
        //            EntityState.Modified;

        //}


        public List<tbl_product_count> GetAllRecords()
        {
            return _unitOfWork.PIDbContext.tbl_product_count.ToList();
        }

        public tbl_product_count GetBySkuStoreWOCode(string store, string sku, string code)
        {
            return _unitOfWork.PIDbContext.tbl_product_count.FirstOrDefault(x => x.store_id == store && x.sku == sku && x.reason_code == code && x.isCompleted == false);
        }

        public tbl_product_count GetBySkuAndStores(string store, string sku)
        {
            return _unitOfWork.PIDbContext.tbl_product_count.FirstOrDefault(x => x.store_id == store && x.sku == sku && x.isCompleted == false);
        }

        public tbl_product_count GetById(int RowId)
        {
            return _unitOfWork.PIDbContext.tbl_product_count.Find(RowId);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _unitOfWork.CashDbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}