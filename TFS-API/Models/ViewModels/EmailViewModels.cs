using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFS_API.Models.ViewModels
{
    public class EmailViewModels
    {
        public class AbstractEmailViewModel
        {
            public string emailTitle { get; set; }
            public string emailBody { get; set; }
        }
        public class BookingEmailViewModel
        {
            public int eventId { get; set; }
            public string requesterName { get; set; }
            public string eventStatus { get; set; }
            public string eventType { get; set; }
            public string jobRole { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? endDate { get; set; }
            public string DurationType { get; set; }
            public string bookedBy { get; set; }
            public List<string> managerList { get; set; }
        }
        public class QuickApproveEmailResponseViewModel : AbstractEmailViewModel
        {
            public string requesterEmail { get; set; }
        }
        public class RequesterAlertViewModel : AbstractEmailViewModel
        {
            public List<string> managerEmails { get; set; }
            public string userEmail { get; set; }
        }


    }
}