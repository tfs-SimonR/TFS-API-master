using CustomerDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TFS_API.Models;
using TFS_API.Models.BindingModels;
using TFS_API.Models.Tables;


namespace TFS_API.Controllers
{
    /// <summary>
    /// API for all TOFS Customer Related endpoints.
    /// </summary>
    [RoutePrefix("api/v1/Customer")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CustomerController : ApiController
    {
        #region ApplicationDBContext
        ApplicationDbContext _db = new ApplicationDbContext();
        tfs_customer_liveEntities db = new tfs_customer_liveEntities();

        #endregion

        // GET: api/Customer
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Gets customer details via search by phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [Route("GetCustomer/{phoneNumber}")]
        public HttpResponseMessage Get(string phoneNumber)
        {
            try
            {
                var query = (from customer in db.tbl_tofscarddetails
                    where customer.phonenumber.Contains(phoneNumber)
                    select customer).ToList();

                var message = Request.CreateResponse(HttpStatusCode.Created, query);
                message.Headers.Location = new Uri(Request.RequestUri + "");
                return message;
            }
            catch (Exception ex)
            {

                var exceptionDetails = new ExceptionDetails();
                

                /*Map & Save exception details*/
                using (_db)
                {
                    exceptionDetails.Message = ex.Message;
                    exceptionDetails.StackTrace = ex.StackTrace;
                    exceptionDetails.Area = "HandyScanner SKU with storeId Get method";
                    exceptionDetails.ExceptionDate = DateTime.Now;
                    _db.ExceptionDetails.Add(exceptionDetails);
                    _db.SaveChanges();
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }

        /// <summary>
        /// Enables search for customer by any details available
        /// </summary>
        /// <param name="searchstring"></param>
        /// <returns></returns>
        [Route("GetCustomerSearch/{searchstring}")]
        public HttpResponseMessage GetByRandom(string searchstring)
        {
            try
            {
                var query = (from customer in db.tbl_tofscarddetails
                             where customer.firstname.Contains(searchstring)|| 
                                   customer.lastname.Contains(searchstring) || 
                                   customer.postcode.Contains(searchstring) ||
                                   customer.phonenumber.Contains(searchstring) ||
                                   customer.clubcardnumber.Contains(searchstring)
                             select customer).ToList();

                var message = Request.CreateResponse(HttpStatusCode.Created, query);
                message.Headers.Location = new Uri(Request.RequestUri + "");
                return message;
            }
            catch (Exception ex)
            {

                var exceptionDetails = new ExceptionDetails();

                /*Map & Save exception details*/
                using (_db)
                {
                    exceptionDetails.Message = ex.Message;
                    exceptionDetails.StackTrace = ex.StackTrace;
                    exceptionDetails.Area = "HandyScanner SKU with storeId Get method";
                    exceptionDetails.ExceptionDate = DateTime.Now;
                    _db.ExceptionDetails.Add(exceptionDetails);
                    _db.SaveChanges();
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }

        /// <summary>
        /// Endpoint to capture new TOFS Card Details 
        /// </summary>
        /// <param name="details"></param>
        /// <returns>201 or 400</returns>
        [Route("PostCustomer")]
        public HttpResponseMessage Post([FromBody]CustomerBM details)
        {
            try
            {
                var dataModel = new tbl_tofscarddetails
                {
                    firstname = details.FirstName,
                    lastname = details.LastName,
                    phonenumber = details.PhoneNumber,
                    email = details.CustomerEmail,
                    postcode = details.PostCode,
                    clubcardnumber = details.CardNumber
                };

                db.tbl_tofscarddetails.Add(dataModel);
                db.SaveChanges();

                var message = Request.CreateResponse(HttpStatusCode.Created, details);
                message.Headers.Location = new Uri(Request.RequestUri + dataModel.Id.ToString());
                return message;
            }
            catch (Exception ex)
            {
                var exceptionDetails = new ExceptionDetails();

                /*Map & Save exception details*/
                using (_db)
                {
                    exceptionDetails.Message = ex.Message;
                    exceptionDetails.StackTrace = ex.StackTrace;
                    exceptionDetails.Area = "POSTCustomer TOFS Card Details ";
                    exceptionDetails.ExceptionDate = DateTime.Now;
                    _db.ExceptionDetails.Add(exceptionDetails);
                    _db.SaveChanges();
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT: api/Customer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Customer/5
        public void Delete(int id)
        {
        }
    }
}
