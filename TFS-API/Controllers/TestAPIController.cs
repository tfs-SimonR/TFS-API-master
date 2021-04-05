using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ErrorViewDAL;
using Mi9DataAccessLayer;
using TFS_API.Models.ViewModels;
using static TFS_API.Models.ViewModels.EmailViewModels;
using SelectListItem = System.Web.WebPages.Html.SelectListItem;

namespace TFS_API.Controllers
{
    public class TestAPIController : BaseController
    {
        // GET: TestAPI
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult NetworkTest()
        {
            return View();
        }

        public ActionResult WareHouseAccept()
        {
            return View();
        }
       

        public ActionResult AuthenticationSuccess()
        {

            return View();
        }

        public JsonResult GetStoresList()
        {
            var query = (from stores in Mi9Db.tfs_store_details
                select new SelectListItem {Value = stores.storeId, Text = stores.store_name});

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AuthenticationFailure()
        {
            return View();
        }

        public ActionResult StoreFeedBack()
        {
            return View();
        }

        public ActionResult GetFeedback()
        {
            var getData = Mi9Db.tfs_stockhome_feedback;
            return Json( new { data = getData }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Errors()
        {
            return View();
        }

        public ActionResult GetErrors()
        {
            ErrorDBContext errorDB = new ErrorDBContext();

            var getData = errorDB.ExceptionDetails;

            return Json(new { data = getData }, JsonRequestBehavior.AllowGet);
        }

       

        public ActionResult AuthenticatedUsers()
        {

            return View();
        }

        public ActionResult GetAuth()
        {
            var currenteDate = DateTime.UtcNow.Date.AddDays(-1);

            var getData = auditDb.tbl_authentication_audit.Where(x => x.timeloggedin >= currenteDate).ToList();

            return Json(new {data = getData}, JsonRequestBehavior.AllowGet);
        }

        
    }
}