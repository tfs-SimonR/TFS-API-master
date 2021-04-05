using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClickAndCollectDAL;
using Mi9DataAccessLayer;
using TFS_API.Models.DTO;
using TFS_API.Repositorys.Interfaces;
using TFS_API.Services.Interfaces;

namespace TFS_API.Services
{
    /// <summary>
    /// Click and Collect Service
    /// </summary>
    public class ClickAndCollectService : IClickAndCollectService
    {
        public Mi9DBEntities mi9db = new Mi9DBEntities();
        private readonly IExceptionService _errorService;
        private readonly IClickAndCollectReserveRepository _clickAndCollectReserveRepository;
        //private readonly IUnitOfWork<CCDBContext> _unitOfWork;
        private IClickAndCollectService _clickAndCollectServiceImplementation;

        /// <inheritdoc />
        public ClickAndCollectService(IExceptionService errorService, 
            IClickAndCollectReserveRepository clickAndCollectReserveRepository
            /*IUnitOfWork<CCDBContext> unitOfWork*/)
        {
            _errorService = errorService;
            _clickAndCollectReserveRepository = clickAndCollectReserveRepository;
            //_unitOfWork = unitOfWork;
        }

        public void AddStatus(ClickAndCollectReservationStatu addrecord)
        {
            _clickAndCollectServiceImplementation.AddStatus(addrecord);
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="reservation"></param>
        public void AddReservation(ClickAndCollectReservation reservation)
        {
            _clickAndCollectReserveRepository.Add(reservation);
            //_unitOfWork.Commit();
        }

        public List<ClickAndCollectReservation> GetAllReservations()
        {
           return _clickAndCollectReserveRepository.GetAll();
        }

        public void AddReservation(IClickAndCollectReserveRepository reservation)
        {
            _clickAndCollectServiceImplementation.AddReservation(reservation);
        }

        /// <summary>
        /// Updates a reservation status
        /// </summary>
        /// <param name="update"></param>
        //public void UpdateReservationStatus(ClickAndCollectReservationStatu update)
        //{
        //    _clickAndCollectStatusRepository.UpdateReservationStatus(update);
        //    _unitOfWork.Commit();
        //}

        /// <summary>
        /// Adds a new reservation status
        /// </summary>
        /// <param name="addrecord"></param>
        //public void AddStatus(ClickAndCollectReservationStatu addstatus)
        //{
        //    _clickAndCollectStatusRepository.AddReservationStatus(addstatus);
        //    _unitOfWork.Commit();
        //}

        public IQueryable<GETPanelDTO> GetProductsForPanel(string sku, string storeId)
        {
            try
            {
                var getvarint = (from productcodess in mi9db.productcodes
                    where productcodess.variantcode == sku
                    select productcodess.varint).FirstOrDefault();

                if (getvarint != null)
                {
                    var getStock = (from stock in mi9db.brnstocks
                        where stock.varint == getvarint && stock.branchcode == storeId
                        select new GETPanelDTO()
                        {
                            qty = (int) stock.retail
                            
                        });
                    return getStock;
                }
                else
                {
                    return null;
                }
                

            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }

        }

        public void UpdateReservationStatus(ClickAndCollectReservationStatu update)
        {
            _clickAndCollectServiceImplementation.UpdateReservationStatus(update);
        }
    }
}