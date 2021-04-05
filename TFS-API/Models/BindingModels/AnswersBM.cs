using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.BindingModels
{
    public class AnswersBM
    {
        public int StoredId { get; set; }
        public int CustomerId { get; set; }
        public string TransId { get; set; }
        public bool isGood { get; set; }
        public bool isBad { get; set; }
        public string Question { get; set; }
        public int TillId { get; set; }
        public DateTime AnswerDateTime { get; set; }
    }
}