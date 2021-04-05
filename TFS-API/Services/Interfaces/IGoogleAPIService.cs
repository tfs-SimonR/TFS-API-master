using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFS_API.Models.DTO;

namespace TFS_API.Services.Interfaces
{
    public interface IGoogleAPIService
    {
        GoogleApiRootObject GetGoogleGeoDetails(string postCode, string latlong);
    }
}