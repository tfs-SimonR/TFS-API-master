using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace TFS_API.Helpers
{
    public class HTTPHelpers
    {
        public static string SendXMLFile(string xmlFilepath, string uri, int timeout)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ContentType = "application/xml";
            request.Method = "POST";

            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(xmlFilepath))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
                byte[] postBytes = Encoding.UTF8.GetBytes(sb.ToString());

                if (timeout < 0)
                {
                    request.ReadWriteTimeout = timeout;
                    request.Timeout = timeout;
                }

                request.ContentLength = postBytes.Length;

                try
                {
                    Stream requestStream = request.GetRequestStream();

                    requestStream.Write(postBytes, 0, postBytes.Length);
                    requestStream.Close();

                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        return response.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    request.Abort();
                    return string.Empty;
                }
            }
        }
    }
}