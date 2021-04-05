using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using TFS_API.Models.Tables;
using PropertiesDAL;

namespace TFS_API.Controllers
{
    [RoutePrefix("api/v1/Image")]
    public class ImageController : ApiController
    {

        [Route("PostImage")]
        public async Task<HttpResponseMessage> Post()
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            if (Request.Content.IsMimeMultipartContent())
            {
                String fullPath = HostingEnvironment.MapPath("~/Images/");
                 MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(fullPath);
                await Request.Content.ReadAsMultipartAsync(new MultipartMemoryStreamProvider()).ContinueWith((task) =>
                 {
                     

                     var fileInfo = streamProvider.FileData.Select(i =>
                     {
                         propDBContext db = new propDBContext();
                         tbl_ImageData img = new tbl_ImageData();

                         var info = new FileInfo(i.LocalFileName);

                         img.Data = File.ReadAllBytes(info.FullName);
                         img.fileName = info.Name;
                         db.tbl_ImageData.Add(img);
                         db.SaveChangesAsync();

                         return "File uploaded successfully!";

                     });

                 });
                return Request.CreateResponse(HttpStatusCode.Accepted, "File uploaded successfully!");
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }


        }
       
    }
}
