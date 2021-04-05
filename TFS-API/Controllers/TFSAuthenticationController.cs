using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AuditDataAccessLayer;
using Serilog;
using TFS_API.Services.Interfaces;

namespace TFS_API.Controllers
{
    [RoutePrefix("api/v1/tfs.authentication")]
    public class TFSAuthenticationController : ApiController
    {
        private readonly IExceptionService _errorService;

        public TFSAuthenticationController(IExceptionService errorService)
        {
            _errorService = errorService;

        }


        [Route("GetAuth")]
        [HttpGet]
        public HttpResponseMessage GetAuth([FromUri]string username, string password) {
            try {
                var authresult = AuthenticateAd(username, password);
                return this.Request.CreateResponse(
                    HttpStatusCode.OK,
                    new { authenticated = authresult });
            }
            catch (Exception ex) {
                Log.Error(ex, "An error occured in GetResponseCodes");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool AuthenticateAd(string username, string password) {
            using (var context = new PrincipalContext(ContextType.Domain, "")) {
                var result = context.ValidateCredentials(username, password);
                return result;
            }
        }
    }
}
