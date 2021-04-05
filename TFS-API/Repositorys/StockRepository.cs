using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Mi9DataAccessLayer;

namespace TFS_API.Repositorys
{
    public class StockRepository
    {
        private UnitOfWork _unitOfWork;

        public StockRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Returns stock information at a store location 
        /// </summary>
        /// <param name="varint"></param>
        /// <param name="store"></param>
        /// <returns></returns>
        public brnstock GetStockInformtaionAtStore(int varint, string store)
        {
            return _unitOfWork.MI9DBContext.brnstocks.FirstOrDefault(x => x.varint == varint && x.branchcode == store); 
        }

        /// <summary>
        /// Returns Varint for a given variantcode
        /// </summary>
        /// <param name="variantcode"></param>
        /// <returns></returns>
        public int GetVarint(string variantcode)
        {
            var getVarint = _unitOfWork.MI9DBContext.productcodes.FirstOrDefault(x => x.variantcode == variantcode);

            return getVarint?.varint ?? 0;
        }
    }
}