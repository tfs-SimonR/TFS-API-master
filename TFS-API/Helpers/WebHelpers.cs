using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;

namespace TFS_API.Helpers
{
    public static class WebHelpers
    {
        public static string GetIP(this HttpRequestMessage requestMessage)
        {
            // Owin Hosting 
            if (requestMessage.Properties.ContainsKey("MS_OwinContext"))
            {
                return HttpContext.Current != null ? HttpContext.Current.Request.GetOwinContext()
                    .Request.RemoteIpAddress : null;
            }
            // Web Hosting 
            if (requestMessage.Properties.ContainsKey("MS_HttpContext"))
            {
                return HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : null;
            }
            // Self Hosting 
            if (requestMessage.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty property =
                    (RemoteEndpointMessageProperty)requestMessage.Properties[RemoteEndpointMessageProperty.Name];
                return property != null ? property.Address : null;
            }
            return null;
        }

        public static bool AllowIP(this HttpRequestMessage request)
        {
            var ipList = "10.252.10.49, 10.252.10.50, 89.145.80.11";

            var whiteListedIPs = ipList;
            if (!string.IsNullOrEmpty(whiteListedIPs))
            {
                var whiteListIPList = whiteListedIPs.Split(',').ToList();
                var ipAddressString = request.GetIP();
                var ipAddress = IPAddress.Parse(ipAddressString);
                var isInwhiteListIPList =
                    whiteListIPList
                        .Where(a => a.Trim()
                            .Equals(ipAddressString, StringComparison.InvariantCultureIgnoreCase))
                        .Any();
                return isInwhiteListIPList;
            }
            return true;
        }
    }
}