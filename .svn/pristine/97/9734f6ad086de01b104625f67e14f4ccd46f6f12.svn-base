using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mi9DataAccessLayer;

namespace TFS_API.Repositorys
{
    public class DeliveriesRepository
    {
        private UnitOfWork _unitOfWork;

        public DeliveriesRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public consighdr GetConsigHdrByConsignmentNumber(string consignment)
        {
            return _unitOfWork.MI9DBContext.consighdrs.Find(consignment);
        }
        
        public consigdest GetDestinationByConsignment(string consignment)
        {
            return _unitOfWork.MI9DBContext.consigdests.Find(consignment);
        }

        public IQueryable<consigline> GetLinesBycondestInt(int condestint)
        {
            return _unitOfWork.MI9DBContext.consiglines.Where(x => x.condestint == condestint);
        }
    }
}