using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TFS_API
{
    public class Globals {

        public static string ConnectionString => ConfigurationManager.ConnectionStrings["QueryConnectionString"].ConnectionString;
        public static string ClickCollectConnectionString => ConfigurationManager.ConnectionStrings["ClickCollectConnectionString"].ConnectionString;
        public static string WarehouseConnectionString => ConfigurationManager.ConnectionStrings["WarehouseConnectionString"].ConnectionString;
        public static string DWHConnectionString => ConfigurationManager.ConnectionStrings["DataWarehouse"].ConnectionString;

    }
}