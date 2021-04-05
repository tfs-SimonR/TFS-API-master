using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TFS_API.Models;
using TFS_API.Models.DTO;
using TFS_API.Models.Tables;
using TFS_API.Repositorys;
using TFS_API.Services.Interfaces;

namespace TFS_API.Controllers
{
    [System.Web.Http.RoutePrefix("api/v1/AppServiceAuth")]
    public class ApplicationServiceAuthenticationController :ApiController
    {
        private readonly IExceptionService _errorService;
        ApplicationDbContext _db = new ApplicationDbContext();
        public ApplicationUserManager _userManager;

        public ApplicationServiceAuthenticationController(IExceptionService errorService)
        {
            _errorService = errorService;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("CreateASPUSER")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage CreateUser([FromBody] AuthDTO model)
        {
            using (var unitofWork = new UnitOfWork())
            {
                //TODO Check if valid TOFS user Name


                var repository = new AuthenticationRepository(unitofWork);
                var insertId = repository.CreateAspUser(model);

                if (insertId == 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to create user.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Accepted, "User Created");
                }
            }
        }

        //public HttpResponseMessage CheckADUserName()
        //{

        //}

        // GET: api/ApplicationServiceAuthentication
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ApplicationServiceAuthentication/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ApplicationServiceAuthentication
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ApplicationServiceAuthentication/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApplicationServiceAuthentication/5
        public void Delete(int id)
        {
        }
    }
}
