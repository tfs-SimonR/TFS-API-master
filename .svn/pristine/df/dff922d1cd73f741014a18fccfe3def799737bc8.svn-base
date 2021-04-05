using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClickAndCollectDAL;
using TFS_API.Models.DTO;
using TFS_API.Repositorys.Interfaces;

namespace TFS_API.Services.Interfaces
{
    public interface IClickAndCollectService
    {
        List<ClickAndCollectReservation> GetAllReservations();
        IQueryable<GETPanelDTO> GetProductsForPanel(string sku, string storeId);
        void UpdateReservationStatus(ClickAndCollectReservationStatu update);

        void AddStatus(ClickAndCollectReservationStatu addrecord);

        void AddReservation(ClickAndCollectReservation reservation);

        void AddReservation(IClickAndCollectReserveRepository reservation);
    }
}