using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuditDataAccessLayer;
using EPOSDataAccess;
using Mi9DataAccessLayer;
using Microsoft.AspNet.Identity.Owin;
using TFS_API.Models;

namespace TFS_API.Controllers
{
    public class BaseController : Controller
    {
        public ApplicationUserManager _userManager;

        public ApplicationDbContext db = new ApplicationDbContext();

        public EPOSDataConnection EposDb = new EPOSDataConnection();

        public Mi9DBEntities Mi9Db = new Mi9DBEntities();

        public auditEntities auditDb = new auditEntities();

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }


    }
}