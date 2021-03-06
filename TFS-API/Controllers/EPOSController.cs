using EPOSDataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Mi9DataAccessLayer;
using Newtonsoft.Json;
using TFS_API.Models;
using TFS_API.Models.BindingModels;
using TFS_API.Models.PostViewModels;
using TFS_API.Models.Tables;

namespace TFS_API.Controllers
{
    /// <summary>
    /// Endpoints for the EPOS Customer facing touchscreen
    /// </summary>
    [RoutePrefix("api/v1/EPOS")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class EPOSController : ApiController
    {

        /*EPOS Data*/
        readonly EPOSDataConnection entities = new EPOSDataConnection();
        /*APIData*/
        readonly ApplicationDbContext db = new ApplicationDbContext();

        readonly Mi9DBEntities mi9DbEntities = new Mi9DBEntities();

        /// <summary>
        /// Streams a large Json object for all product descriptions with SKU's
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProducts/Stream")]
        public HttpResponseMessage PushStreamContent()
        {
            var response = Request.CreateResponse();

            response.Content = new PushStreamContent((stream, content, context) =>
            {
                var query = (from products in mi9DbEntities.products
                    join prodcodes in mi9DbEntities.productcodes on products.prodint equals prodcodes.varint
                    select new 
                    {
                        sku = prodcodes.variantcode,
                        description = products.proddesc
                    }).ToList();

                foreach (var product in query)
                {
                    var serializer = new JsonSerializer();
                    using (var writer = new StreamWriter(stream))
                    {
                        serializer.Serialize(writer, product);
                        stream.Flush();
                    }
                }
            });

            return response;
        }

        [HttpGet]
        [Route("DownloadProductFile/{id}")]
        public HttpResponseMessage GetProductFile(int id)
        {
            HttpResponseMessage result = null;
            try
            {
                var file = db.Files.SingleOrDefault(b => b.Id == id);

                if (file == null)
                {
                    result = Request.CreateResponse(HttpStatusCode.Gone);
                }
                else
                {
                    // send file to client
                    byte[] bytes = Convert.FromBase64String(file.content);

                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = file.filename + ".json"
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                
                return Request.CreateResponse(HttpStatusCode.Gone);
            }
        }


        [HttpGet]
        [Route("GetQuestions")]
        public HttpResponseMessage Get()
        {
            var entity = entities.Questions.Where(x => x.isActive == true).ToList();

            if (entity != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { questions = entity });
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Question with Id  not found.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PostQuestions")]
        public async Task<HttpResponseMessage> Post([FromBody]AnswersBM answer)
        {
            try
            {
                var dataModel = new QuestionAnswer()
                {
                    Question = answer.Question,
                    StoredId = answer.StoredId,
                    CustomerId = answer.CustomerId,
                    TransId = answer.TransId,
                    isBad = answer.isBad,
                    isGood = answer.isGood,
                    TillId = answer.TillId,
                    AnswerDateTime = answer.AnswerDateTime

                };

                entities.QuestionAnswers.Add(dataModel);
                await entities.SaveChangesAsync();

                return Request.CreateResponse(HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                var exceptionDetails = new ExceptionDetails();

                /*Map & Save exception details*/
                using (db)
                {
                    exceptionDetails.Message = ex.Message;
                    exceptionDetails.StackTrace = ex.StackTrace;
                    exceptionDetails.Area = "HandyScanner SKU with storeId Get method";
                    exceptionDetails.ExceptionDate = DateTime.Now;
                    db.ExceptionDetails.Add(exceptionDetails);
                    db.SaveChanges();
                }

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException);

            }
        }

        public HttpResponseMessage Put(int id, [FromBody]Question question)
        {
            try
            {
                var entity = entities.Questions.FirstOrDefault(x => x.Id == id);

                if (entity != null)
                {
                    entity.QuestionText = question.QuestionText;

                }
                entities.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, entity);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }
}

