using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClickAndCollectDAL;
using Mi9DataAccessLayer;
using TFS_API.Models.ViewModels;
using static TFS_API.Models.ViewModels.EmailViewModels;

namespace TFS_API.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailMappers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="storename"></param>
        /// <returns></returns>
        public static QuickApproveEmailResponseViewModel EmailMap(tfs_stockhome_feedback vm, string storename)
        {
            var generateEmail = new QuickApproveEmailResponseViewModel
            {
                emailTitle = "A new piece of feedback has been left by " + storename,
                emailBody = "<font face='Arial'>" +
                            "<h3><strong>The Original Factory Shop</strong></h3>" +
                            "<p><strong>---------------------------------------------------</strong></p>" +
                            "<p><font color='" + "'>A new piece of feedback has been left " + ".</font></p>" +
                            "<p><strong>Store Id:&nbsp; </strong>" + vm.storeid + "</p>" +
                            "<p><strong>Store Name: </strong>" + storename + "</p>" +
                            "<p><strong>Comments: &nbsp;</strong>" + vm.comments + "</p>" +
                            "<p><strong>Comment Date: &nbsp;</strong>" + vm.Date + "</p>" +
                            "<p><strong>TOFS - Development Team</strong></p>" +
                            "</font>"
            };
            return generateEmail;
        }

        public static QuickApproveEmailResponseViewModel ReservationEmailMap(ClickAndCollectReservation vm,string storename)
        {
            var generateEmail = new QuickApproveEmailResponseViewModel
            {
                requesterEmail = vm.CustEmailAddress,
                emailTitle = "Thanks For Your reservation at our " + storename + " store.",
                emailBody = "<font face='Arial'>" +
                            "<h3><strong>The Original Factory Shop</strong></h3>" +
                            "<p><strong>---------------------------------------------------</strong></p>" +
                            "<p><font color='" + "'>A new piece of feedback has been left " + ".</font></p>" +
                            "<p><strong>Dear: </strong>" + vm.CustFirstName + "</p>" +
                            "<p><strong>Your Email Address: &nbsp;</strong>" + vm.CustEmailAddress + "</p>" +
                            "<p><font color='" + "'>Thanks for your reservation at the below store " + ".</font></p>" +
                            "<p><strong>Store Name: </strong>" + storename + "</p>" +
                            "<p><strong>Your Reservation number is: &nbsp;</strong>" + vm.ReservationID + "</p>" +
                            "<p><strong>Reservation Date: &nbsp;</strong>" + vm.MarketingConsentDate + "</p>" +
                            "<p><strong>TOFS - Development Team</strong></p>" +
                            "</font>"
            };
            return generateEmail;
        }
    }
}