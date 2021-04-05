using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models
{
    public class SPTestModel
    {
        public string consignment { get; set; }
        public string warehouse { get; set; }
        public string destination { get; set; }
        public string proddesc { get; set; }
        public string variantcode { get; set; }
        public int issueqty { get; set; }
        public int receiveqty { get; set; }

    }
}