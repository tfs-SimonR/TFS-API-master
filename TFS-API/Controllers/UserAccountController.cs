using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TFS_API.Models;
using TFS_API.Models.Tables;
using System.DirectoryServices;
using Microsoft.AspNet.Identity.EntityFramework;
using TFS_API.Mappers;
using static TFS_API.Models.UserAccountViewModels;
using static TFS_API.ApplicationUserManager;

namespace TFS_API.Controllers
{
    /// <summary>
    /// We use these methods for AD user searches
    /// </summary>
    public class UserAccountController : BaseController
    {
        ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Login()
        {
            return View();
        }


        //[Authorize]
        public ActionResult CreateUser()
        {
            return View();
        }

        /// <summary>
        /// This is for the creation of users via AD
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(RegisterViewModel model)
        {
            ModelState.Clear();
            if (model.Action == "Search for User")
            {
                try
                {
                    #region ErrorChecking
                    if (string.IsNullOrWhiteSpace(model.SearchAD))
                    {
                        var fs = string.Format("Please enter a valid TOFS Username");
                        ModelState.AddModelError("SearchAD", fs);
                        return View(model);
                    }


                    var adLookup = AdUserSearch(model.SearchAD);

                    if (adLookup == null)
                    {
                        var fs = $"Unable to locate {model.SearchAD}, please enter a valid TOFS username";
                        ModelState.AddModelError("SearchAD", fs);
                        return View(model);
                    }


                    if (UserManager.FindByName(model.SearchAD) != null)
                    {
                        var fs = $"{model.SearchAD} Already Exists within the database";
                        ModelState.AddModelError("SearchAD", fs);
                        return View(model);
                    }
                    #endregion

                    #region AD Service Call & Mapping
                    var newModel = AdLookupToViewModel(model, adLookup);
                    return View("CreateUser", newModel);
                    #endregion
                }
                catch (Exception e)
                {
                    #region ExceptionHandle
                    ExceptionDetails error = new ExceptionDetails();
                    error.Message = e.Message;
                    error.InnerException = e.InnerException.ToString();
                    error.ExceptionDate = DateTime.Now;
                    error.Area = "Create User";
                    error.User = User.Identity.GetUserName();
                    _db.ExceptionDetails.Add(error);
                    _db.SaveChanges();
                    #endregion
                }
            }
            else
            {
                try
                {
                    var insertId = CreateAspUser(model);

                    if (insertId == null)
                    {
                        Response.StatusCode = 500;
                        return View(model);
                    }
                    else
                    {
                        ViewBag.Success = "Record Successfully Created";
                        if (Request.IsAjaxRequest())
                        {
                            var viewModel = GetRecord(insertId);
                            if (viewModel == null)
                            {
                                #region ValidationErrors?
                                Response.StatusCode = 404;
                                Response.TrySkipIisCustomErrors = true;
                                ViewBag.ErrorMessage = "Unable to process due to validation errors, please check form and try again.";
                                #endregion
                            }

                            #region Created
                            Response.StatusCode = 201;//Created Status Code
                            Response.RedirectLocation = Url.Action("CreateUser", "UserAccount");
                            #endregion
                        }
                    }

                    return RedirectToAction("CreateUser", "UserAccount");

                }
                catch (Exception ex)
                {
                    #region ExceptionHandle
                    ExceptionDetails error = new ExceptionDetails();

                    error.Message = ex.Message;
                    error.InnerException = ex.InnerException.ToString();
                    error.ExceptionDate = DateTime.Now;
                    error.Area = "Create User";
                    error.User = User.Identity.GetUserName();
                    _db.ExceptionDetails.Add(error);
                    _db.SaveChanges();

                    #endregion
                }

            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public RegisterViewModel AdLookupToViewModel(RegisterViewModel vm, RegisterViewModel adLookupToViewModel)
        {
            try
            {
                var x = AccountMappers.ADLookupToViewModel(vm, adLookupToViewModel);
                return x;
            }
            catch(Exception ex)
            {
                ExceptionDetails error = new ExceptionDetails();

                error.Message = ex.Message;
                if (ex.InnerException != null) error.InnerException = ex.InnerException.ToString();
                error.ExceptionDate = DateTime.Now;
                error.Area = "Create User";
                error.User = User.Identity.GetUserName();
                _db.ExceptionDetails.Add(error);
                _db.SaveChanges();
                return ViewBag(error);
            }

        }
  

        public RegisterViewModel AdUserSearch(string userAd)
        {
            // Attaches to local computer's Active Directory
            // Uses the username passed via the controller to search for that account
            // If account exists, maps properties to viewmodel and returns to controller, else returns null
            try
            {
                var vm = new RegisterViewModel();
                var adConnect = new DirectoryEntry();

                var userSearcher = new DirectorySearcher
                {
                    /*Searches AD with given sAMAccount name*/
                    Filter = $"(sAMAccountName={userAd})",
                    SearchRoot = adConnect
                };

                using (userSearcher)
                {
                    var userResults = userSearcher.FindAll();

                    /*Checks to see if query returned one result*/
                    if (userResults.Count == 1)
                    {
                        #region ADLookupResults
                        /*Loop through results*/
                        foreach (SearchResult userResult in userResults)
                        {
                            // Foreach property, the length needs to be checked
                            // If the length isn't greater than zero, to set the field as an empty string
                            // This will stop any null exceptions relating to a user account

                            /*Username - Hidden: Validation Purposes*/
                            if (userResult.Properties["sAMAccountName"].Count > 0)
                            {
                                vm.HiddenUsername = userResult.Properties["sAMAccountName"][0].ToString();
                            }

                            /*Forenames*/
                            if (userResult.Properties["givenName"].Count > 0)
                            {
                                vm.Forenames = userResult.Properties["givenName"][0].ToString();
                            }
                            else
                            {
                                vm.Forenames = "";
                            }

                            /*Surname*/
                            if (userResult.Properties["sn"].Count > 0)
                            {
                                vm.Surname = userResult.Properties["sn"][0].ToString();
                            }
                            else
                            {
                                vm.Surname = "";
                            }

                            /*Email*/
                            if (userResult.Properties["mail"].Count > 0)
                            {
                                vm.Email = userResult.Properties["mail"][0].ToString();
                            }
                            else
                            {
                                vm.Email = "";
                            }

                            /*Display name*/
                            if (userResult.Properties["displayName"].Count > 0)
                            {
                                vm.DisplayName = userResult.Properties["displayName"][0].ToString();
                            }
                            else
                            {
                                vm.DisplayName = "";
                            }
                        }
                        return vm;
                        #endregion
                    }
                    else
                    {
                        /*Returns null if no Username is found*/
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                /*New Exception Model*/
                ExceptionDetails exceptionDetails = new ExceptionDetails();

                /*Map & Save exception details*/
                using (_db)
                {
                    exceptionDetails.Message = ex.Message.ToString();
                    exceptionDetails.StackTrace = ex.StackTrace.ToString();
                    exceptionDetails.Area = "Active Directory Lookup Service";
                    exceptionDetails.ExceptionDate = DateTime.Now;
                    _db.ExceptionDetails.Add(exceptionDetails);
                    _db.SaveChanges();
                    return ViewBag("Error");
                }
                
            }
        }

        public dynamic CreateAspUser(RegisterViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db))
            {
                PasswordValidator = new CustomPasswordValidator()
            };

            try
            {
                var createAppUser = AccountMappers.vmToEntity(model);

                var result = manager.Create(createAppUser);

                if (result.Succeeded)
                {
                    
                }
                else
                {
                    
                }

                return createAppUser.Email;
            }
            catch (Exception ex)
            {
                var exceptionDetails = new ExceptionDetails();

                /*Map & Save exception details*/
                using (_db)
                {
                    exceptionDetails.Message = ex.Message.ToString();
                    exceptionDetails.StackTrace = ex.StackTrace.ToString();
                    exceptionDetails.Area = "Active Directory Lookup Service";
                    exceptionDetails.ExceptionDate = DateTime.Now;
                    _db.ExceptionDetails.Add(exceptionDetails);
                    _db.SaveChanges();
                    return ViewBag("Error");
                }
            }
        }

        public RegisterViewModel AdLookupToViewModels(RegisterViewModel vm, RegisterViewModel adLookupToViewModel)
        {
            try
            {
                var x = AccountMappers.ADLookupToViewModel(vm, adLookupToViewModel);
                return x;
            }
            catch
            {
                throw;
            }
        }

        public BulkLeaveViewModel GetRecord(int recordId)
        {
            try
            {
                return null;

            }
            catch (Exception ex)
            {

                var exceptionDetails = new ExceptionDetails();

                /*Map & Save exception details*/
                using (_db)
                {
                    exceptionDetails.Message = ex.Message;
                    exceptionDetails.StackTrace = ex.StackTrace;
                    exceptionDetails.Area = "HandyScanner Goods Inward";
                    exceptionDetails.ExceptionDate = DateTime.Now;
                    _db.ExceptionDetails.Add(exceptionDetails);
                    _db.SaveChanges();
                }

                return ViewBag.ToString("Error Caught in DB");

            }
        }
    }
}