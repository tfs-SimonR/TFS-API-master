using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TFS_API.Models.ViewModels
{
    public class PricingPeopleProductCodesVM
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; set; }
        public DateTime ExceptionDate { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
        public string Area { get; set; }
        public string Message { get; set; }
        public string User { get; set; }
    }
}