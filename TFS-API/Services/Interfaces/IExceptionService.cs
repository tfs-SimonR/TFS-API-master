using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFS_API.Models.Tables;

namespace TFS_API.Services.Interfaces
{
    public interface IExceptionService
    {
        void Capture(Exception e);
    }
}