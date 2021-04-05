using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.Tables
{
    public class ImageModelViewModel

    {
        public int Id { get; set; }

        public byte[] Data { get; set; }

        public string fileName { get; set; }

        public string mimeType { get; set; }

        public int EmployeeId { get; set; }
    }
}