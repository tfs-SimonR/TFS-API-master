using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TFS_API.Models.DTO;
using TFS_API.Services.Interfaces;

namespace TFS_API.Services
{
    public class GoogleAPIService : IGoogleAPIService
    {
        private readonly IExceptionService _errorService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorService"></param>
        public GoogleAPIService(IExceptionService errorService)
        {
            _errorService = errorService;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postCode"></param>
        /// <returns></returns>
        public GoogleApiRootObject GetGoogleGeoDetails(string postCode, string latlong)
        {
            try
            {
                var client = new HttpClient();
                var postcode = postCode;
                var latlng = latlong;
                const string apiKey = "AIzaSyDZkH3S3BNr30WYW2dTXSy1hb5PKvbzRPQ";
                var requestUri =
                    $"https://maps.googleapis.com/maps/api/geocode/json?key={apiKey}&address={postcode}&latlng={latlng}&sensor=false";
                var response = client.GetAsync(requestUri)
                    .Result;
                var json = response.Content.ReadAsStringAsync().Result;
                var results = JsonConvert.DeserializeObject<GoogleApiRootObject>(json);
                return results;
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }

        }
    }
}