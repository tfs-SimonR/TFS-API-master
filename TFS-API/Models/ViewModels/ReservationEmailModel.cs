using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.ViewModels
{
    public class ReservationEmailModel
    {
        public string ReservationId { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Store { get; set; }
        public string phone { get; set; }

    }
}