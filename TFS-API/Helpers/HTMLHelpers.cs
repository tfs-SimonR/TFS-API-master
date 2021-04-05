using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TFS_API.Helpers
{
    public static class HTMLHelpers
    {
        public static string VersionDisplay(this HtmlHelper x)
        {
            string version = System.Reflection.Assembly.GetExecutingAssembly()
                               .GetName()
                               .Version
                               .ToString();

            return version;
        }
    }
}