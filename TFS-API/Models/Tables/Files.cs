using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TFS_API.Models.Tables
{
    [Table("tbl_api_files")]
    public class Files
    {
        public int Id { get; set; }

        public string filename { get; set; }

        public string content { get; set; }
    }
}