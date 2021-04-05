using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mi9DataAccessLayer;
using TFS_API.Repositorys.Interfaces;

namespace TFS_API.Repositorys
{
    public class ClickAndCollectPricingRepository : IClickAndCollectPricingRepository
    {
        private UnitOfWork _unitOfWork;

        public ClickAndCollectPricingRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int GetVarint(string sku)
        {
           var varint = _unitOfWork.MI9DBContext.productcodes.FirstOrDefault(x => x.variantcode == sku);
           if (varint != null)
               return varint.varint;
           return varint.varint = 0;
        }

        public rpricehdr getcurrentPrice(int varint)
        {
            return _unitOfWork.MI9DBContext.rpricehdrs.FirstOrDefault(x => x.varint == varint);
        }

        public rpricehist getwasPrice(int varint)
        {
            return _unitOfWork.MI9DBContext.rpricehists.FirstOrDefault(x => x.varint == varint);
        }

        public supproduct Getmrp(int varint)
        {
            return _unitOfWork.MI9DBContext.supproducts.Where(x => x.varint == varint).OrderByDescending(x => x.DateLastModified).FirstOrDefault();
        }
    }

}