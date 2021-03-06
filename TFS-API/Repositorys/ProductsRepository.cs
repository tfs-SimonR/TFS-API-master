using ClickAndCollectDAL;
using Mi9DataAccessLayer;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace TFS_API.Repositorys
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductsRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private UnitOfWork _unitOfWork;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public ProductsRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public Task<List<product>> GetAllProducts()
        {
            return _unitOfWork.MI9DBContext.products.ToListAsync();
        }

        public ClickAndCollectProductTranslation GetTranslation(string guid)
        {
            return _unitOfWork.CCDBContext.ClickAndCollectProductTranslations.Find(guid);
        }

        public ClickAndCollectProductTranslation GetProductDetailsFromSku(string sku)
        {
            return _unitOfWork.CCDBContext.ClickAndCollectProductTranslations.FirstOrDefault(x => x.sku == sku);
        }

        public async Task<rpricehdr> GetRetailPrice(int varint)
        {
            return _unitOfWork.MI9DBContext.rpricehdrs.FirstOrDefault(x => x.varint == varint);
        }

        public async Task<productcode> GetProdint(string sku)
        {
            return _unitOfWork.MI9DBContext.productcodes.FirstOrDefault(x => x.variantcode == sku);
        }

        public IQueryable<productcode> GetProductCodeDetails(int prodint)
        {
            return _unitOfWork.MI9DBContext.productcodes.Where(x => x.prodint == prodint);
        }

        public async Task<product> GetProductDetailsFromProdint(int prodint)
        {
            return _unitOfWork.MI9DBContext.products.FirstOrDefault(x => x.prodint == prodint);
        }

        public product GetProductDetails(string prodcode)
        {
            return _unitOfWork.MI9DBContext.products.FirstOrDefault(x => x.prodcode == prodcode);
        }

        public brnstock GetStoreStock(int varint, string storeId)
        {
            return _unitOfWork.MI9DBContext.brnstocks.FirstOrDefault(x =>
                x.varint == varint && x.branchcode == storeId);
        }

        public productcode GetVarint(string prodcode)
        {
            return _unitOfWork.MI9DBContext.productcodes.FirstOrDefault(x => x.variantcode == prodcode);
        }

    }
}