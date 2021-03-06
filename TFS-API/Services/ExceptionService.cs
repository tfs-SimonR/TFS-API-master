using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Serilog;
using TFS_API.Helpers;
using TFS_API.Models;
using TFS_API.Models.Tables;
using TFS_API.Services.Interfaces;

namespace TFS_API.Services
{
    public class ExceptionService : IExceptionService
    {
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public void Capture(Exception e)
        {
            try
            {
                Log.Information(e.HelpLink);
                Log.Information(e.Source);
                Log.Warning(e.Message);
                Log.Error(new Exception(e.ToString()), "This is an Error");
                Log.Fatal(new Exception("Oh Sh!t"), "This is a Fatal error");
            }
            catch (Exception ex)
            {
                Log.Information("This is a Test");
                Log.Warning("This is a Warning");
                Log.Error(new Exception("Oh Shit"), "This is an Error");
                Log.Fatal(new Exception("Oh Shit"), "This is a Fatal error");
            }
        }
    }
}