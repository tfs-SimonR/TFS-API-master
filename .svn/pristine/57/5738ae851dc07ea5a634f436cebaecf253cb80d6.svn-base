using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TFS_API.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
    

        public ActionResult LoginTest()
        {
            return View();
        }

        [Authorize]
        public ActionResult AuthenticationSuccess()
        {
            return View();
        }

        public ActionResult AuthenticationFailure()
        {
            return View();
        }
    }
}
