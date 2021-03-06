using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TFS_API.Models
{
    public class UserAccountViewModels
    {

        public class RegisterAbstractViewModel
        {
            [Display(Name = "Forenames")]
            public string Forenames { get; set; }

            [Display(Name = "Surname")]
            public string Surname { get; set; }

            [Display(Name = "TOFS Username")]
            public string SearchAD { get; set; }

            [Display(Name = "Display Name")]
            public string DisplayName { get; set; }

            [DataType(DataType.EmailAddress)]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Date Joined the Trust")]
            public DateTime DateJoinedTheTrust { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Continued Service")]
            public DateTime ContinuedService { get; set; }

            [Display(Name = "Full time")]
            public bool isFulltime { get; set; }

            [Display(Name = "Work Role Type")]
            public int ShiftTypeId { get; set; }

            [Display(Name = "Division")]
            public int DivisionID { get; set; }

            [Display(Name = "Sub Division")]
            public int SubDivisionID { get; set; }

            [Display(Name = "Department")]
            public int DepartmentID { get; set; }

            [Display(Name = "Assign Team")]
            public int TeamID { get; set; }

            [Display(Name = "Assign Primary Job role")]
            public int AssignPrimaryJobRole { get; set; }

            [Display(Name = "Assign Primary ShiftPattern")]
            public int AssignPrimaryShiftPattern { get; set; }

            [Display(Name = "Current Flexi (Hours)")]
            public double CurrentFlexi { get; set; }

            [Display(Name = "Hours Carried Forward")]
            public double HoursCarriedForward { get; set; }

            [Display(Name = "Contracted Hours (Weekly)")]
            public double AssignedMonthlyHours { get; set; }


            public int? InsertedId { get; set; }

            public string RoleId { get; set; }

            /*Hidden*/
            public string Action { get; set; }
            public string ErrorState { get; set; }
            public string HiddenUsername { get; set; }
        }



        public class RegisterViewModel : RegisterAbstractViewModel
        {
            public SelectList DivisionList { get; set; }
            public SelectList DivisionSubList { get; set; }
            public SelectList ShiftTypeList { get; set; }
            public SelectList RoleList { get; set; }
            public SelectList ContractedHoursList { get; set; }
        }

        //public class PossibleAnswer
        //{
        //    public int Id { get; set; }
        //    public bool Answer { get; set; }
        //}

        public class BulkLeaveAbstractViewModel
        {

        }

        public class BulkLeaveViewModel
        {

        }

        #region Index
        public class UserListIndexViewModel
        {
            public int StaffProfileID { get; set; }

            public string Fullname { get; set; }

            public string Department { get; set; }

            public string Team { get; set; }

            public DateTime? RoleStartDate { get; set; }
        }

        #endregion

        #region RoleAdmin
        public class RoleAdministrationAbstractViewModel
        {
            public RoleAdministrationAbstractViewModel()
            {
                Roles = new List<_Roles>();
            }

            public virtual StaffOverview StaffOverview { get; set; }

            public ICollection<_Roles> Roles { get; set; }

            /*Audit Values*/
            public int UpdatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }

        }

        public class StaffOverview
        {
            [Display(Name = "Staff ID")]
            public int StaffProfileId { get; set; }

            public string Fullname { get; set; }
        }

        public class _Roles
        {
            public string RoleId { get; set; }
            public string RoleDescription { get; set; }
        }

        #endregion  

        public class AssignedRolesViewModel
        {

        }
    }
}