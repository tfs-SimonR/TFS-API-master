using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mi9DataAccessLayer;

namespace TFS_API.Repositorys
{
    public class PricingRepository
    {   
        private UnitOfWork _unitOfWork;

        public PricingRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TOFS_CostPrice GetCostPrice(string sku)
        {
            return _unitOfWork.MI9DBContext.TOFS_CostPrice.FirstOrDefault(x => x.prodcode == sku);
        }

        public TOFS_RetailPrice GetRetailPrice(int varint)
        {
            return _unitOfWork.MI9DBContext.TOFS_RetailPrice.FirstOrDefault(x => x.Varint == varint);
        }

    }
}