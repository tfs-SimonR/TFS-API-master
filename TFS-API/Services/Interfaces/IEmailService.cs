using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TFS_API.Models.ViewModels;

namespace TFS_API.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEmailService
    {
        Task FeedbackEmail(EmailViewModels.QuickApproveEmailResponseViewModel vm);

        Task ReservationEmail(EmailViewModels.QuickApproveEmailResponseViewModel vm);
    }
}