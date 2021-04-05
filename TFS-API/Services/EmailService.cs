using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using TFS_API.Models.ViewModels;
using TFS_API.Services.Interfaces;
using static TFS_API.Models.ViewModels.EmailViewModels;

namespace TFS_API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IExceptionService _errorService;

        public EmailService(IExceptionService errorService)
        {
            _errorService = errorService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task FeedbackEmail(QuickApproveEmailResponseViewModel vm)
        {
            try
            {
                var credentialUserName = "sroberts";
                var pwd = "Burnley123";
                var sentFrom = "handyscanner@tofs.com";
                var client = new SmtpClient("tfs-exchange01.fsdomain.local")
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true
                };

                var credentials = new NetworkCredential(credentialUserName, pwd);
                client.EnableSsl = false;
                client.Credentials = credentials;

                var emailList = new List<string> {"mw@tofs.com", "sroberts@tofs.com", "slyons@tofs.com", "jim@thepricingpeople.co.uk", "sh@tofs.com", "amcateer@tofs.com" };

                foreach (var email in emailList)
                {
                    var mail = new MailMessage(sentFrom, email)
                    {
                        Subject = vm.emailTitle,
                        Body = vm.emailBody,
                        IsBodyHtml = true
                    };

                    await client.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }

        public async Task ReservationEmail(QuickApproveEmailResponseViewModel vm)
        {
            try
            {
                const string credentialUserName = "sroberts";
                const string pwd = "Burnley123";
                const string sentFrom = "OrderReservation@tofs.com";
                var client = new SmtpClient("tfs-exchange01.fsdomain.local")
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true
                };

                var credentials = new NetworkCredential(credentialUserName, pwd);
                client.EnableSsl = false;
                client.Credentials = credentials;

                var emailList = new List<string> {vm.requesterEmail};

                foreach (var mail in emailList.Select(email => new MailMessage(sentFrom, email)
                {
                    Subject = vm.emailTitle,
                    Body = vm.emailBody,
                    IsBodyHtml = true
                }))
                {
                    await client.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;
            }
        }
    }
}