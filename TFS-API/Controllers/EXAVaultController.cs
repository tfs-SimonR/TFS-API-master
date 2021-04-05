using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using TFS_API.Helpers;
using TFS_API.Models.DTO;

namespace TFS_API.Controllers
{
    [System.Web.Http.RoutePrefix("api/v1/SFTPActions")]
    public class EXAVaultController : ApiController
    {
        /// <summary>
        /// Lists all files on SFTP if you pass in the location
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [System.Web.Http.Route("GetFileList/{path}")]
        public HttpResponseMessage GetListOfFiles(string path)
        {
            var getFiles = SFTPHelpers.listFiles(path);

            var ListSFTPFiles = getFiles.Select(file => new SFTPFileNames() {fileName = file.FullName}).ToList();

            return Request.CreateResponse(HttpStatusCode.Accepted, ListSFTPFiles);
        }

        /// <summary>
        /// Downloads specific hardcoded file from Exa SFTP 
        /// </summary>
        /// <returns>C:\SFTPDownloads", "RC_Products.csv</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("DownloadFile")]
        public ResponseMessageResult DownloadFile()
        {
            SFTPHelpers.downloadFile("RC_Products.csv");

            string pathLocalFile = Path.Combine(@"C:\SFTPDownloads\RC_Products.csv");
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(pathLocalFile, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/csv");
            result.Content.Headers.ContentLength = stream.Length;

            string input = $"filename=RC_Products.csv";
            ContentDispositionHeaderValue contentDisposition = null;
            if (ContentDispositionHeaderValue.TryParse(input, out contentDisposition))
            {
                result.Content.Headers.ContentDisposition = contentDisposition;
            }
            return ResponseMessage(result);
            
        }

       
        /// <summary>
        /// Deletes files in the directory C:\SFTPDownloads
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("DeleteFiles")]
        public HttpResponseMessage DeleteLocalFiles()
        {
            string sourceDir = @"C:\SFTPDownloads";
            string[] csvList = Directory.GetFiles(sourceDir, "*.csv");
            Console.WriteLine();
            Console.WriteLine("Removing Local versions");
            // Delete source files that were copied.
            foreach (string f in csvList)
            {
                File.Delete(f);
            }
            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Dynamic download of file from EXA SFTP
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("DownloadFile/{fileName}")]
        public HttpResponseMessage DownloadFile(string fileName)
        {
            HttpResponseMessage result = null;
            try
            {
                SFTPHelpers.downloadFile(fileName);

                string pathLocalFile = Path.Combine(@"C:\SFTPDownloads", fileName);

                if (false)
                {
                    result = Request.CreateResponse(HttpStatusCode.Gone);
                }
                else
                {
                    result = new HttpResponseMessage(HttpStatusCode.OK);
                    var stream = new FileStream(pathLocalFile, FileMode.Open, FileAccess.Read);
                    result.Content = new StreamContent(stream);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "document.docx"
                    };
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                    return result;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.Gone);
            }
        }
    }
}
