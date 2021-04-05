using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CashingUpDAL;
using TFS_API.Models.BindingModels;

namespace TFS_API.Repositorys.Interfaces
{
    /// <summary>
    /// Interface for Cash Repo
    /// </summary>
    public interface ICashRepository
    {
        /// <summary>
        /// Update Entity
        /// </summary>
        /// <param name="cashManagement"></param>
        void Update(tbl_CashManagement cashManagement);
        /// <summary>
        /// Add Entity
        /// </summary>
        /// <param name="cashManagement"></param>
        void Add(tbl_CashManagement cashManagement);
        /// <summary>
        /// Get SPQ Tills
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        IQueryable<tbl_store_till_availabilty> GetSPQTills(string store);

        /// <summary>
        /// Get RJ Tills
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        List<tbl_store_till_availabilty> GetRJTills(string store);
        /// <summary>
        /// Checks if cash record Exists
        /// </summary>
        /// <param name="createdate"></param>
        /// <param name="storeid"></param>
        /// <param name="tillId"></param>
        /// <returns></returns>
        bool isExistingSPQ(DateTime createdate, string storeid, int tillId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="createdate"></param>
        /// <param name="storeid"></param>
        /// <param name="tillId"></param>
        /// <returns></returns>
        bool isExistingRJ(DateTime createdate, string storeid, int tillId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisWeek"></param>
        /// <param name="endofdayToday"></param>
        /// <param name="store"></param>
        /// <returns></returns>
        List<CashManagmentModel> GetRecordsToList(DateTime thisWeek, DateTime endofdayToday, string store);

        tbl_CashManagement GetTodaysCashRecord(DateTime today, string store, int tillId);
    }
}