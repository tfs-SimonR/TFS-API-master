using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Results;
using AuditDataAccessLayer;
using Microsoft.Owin.Security.Infrastructure;
using TFS_API.Models;
using TFS_API.Models.ViewModels;
using TFS_API.Services.Interfaces;

namespace TFS_API.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly IExceptionService _errorService;
        auditEntities auditDb = new auditEntities();

        public ApplicationOAuthProvider(IExceptionService errorService)
        {
            _errorService = errorService;

        }

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException(nameof(publicClientId));
            }

            _publicClientId = publicClientId;
        }

        /// <summary>
        /// This is where the Authentication Token is set if credentials are same as AD
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

                ApplicationUser user = await userManager.FindByNameAsync(context.UserName);

                if (user != null)
                {
                    if (AuthenticateAd(context.UserName, context.Password))
                    {
                        ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
                        OAuthDefaults.AuthenticationType);

                        ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                            CookieAuthenticationDefaults.AuthenticationType);

                        AuthenticationProperties properties = CreateProperties(user.UserName);

                        AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);

                        context.Validated(ticket);

                        context.Request.Context.Authentication.SignIn(cookiesIdentity);
                        return;
                    }
                    else
                    {
                        context.SetError("invalid_grant", "User not authenticated");
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
            }
        }

        //
        // Authenticate AD
        public bool AuthenticateAd(string username, string password)
        {
            //using (var context = new PrincipalContext(ContextType.Domain, ""))
            //{
            //    var result = context.ValidateCredentials(username, password);

            //    string authMessage = null;

            //    if (result == true)
            //    {
            //        authMessage = "Authenticated";
            //    }
            //    if (result == false)
            //    {
            //        authMessage = "Password Incorrect";
            //    }
            //    var dataModel = new tbl_authentication_audit
            //    {
            //        username = username, timeloggedin = DateTime.Now, message = authMessage
            //    };
            //    auditDb.tbl_authentication_audit.Add(dataModel);
            //    auditDb.SaveChanges();

            //    return result;
            //}

            return true;

        }
        /// <summary>
        /// Updated to chnage datetime format to UTC and also remove . from issued and expires.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                var key = property.Key;
                switch (key)
                {
                    case ".expires":
                        key = "expires";
                        break;
                    case ".issued":
                        key = "issued";
                        break;
                    case "userName":
                        key = "user_name";
                        break;
                }
                if (key == "issued")
                {
                    if (context.Properties.IssuedUtc != null)
                        context.AdditionalResponseParameters.Add(key,
                            context.Properties.IssuedUtc.Value.ToString("o",
                                CultureInfo.InvariantCulture));
                }
                else if (key == "expires")
                {
                    if (context.Properties.ExpiresUtc != null)
                        context.AdditionalResponseParameters.Add(key,
                            context.Properties.ExpiresUtc.Value.ToString("o",
                                CultureInfo.InvariantCulture));
                }
                else
                {
                    context.AdditionalResponseParameters.Add(key, property.Value);
                }
            }

            return Task.FromResult<object>(null);
        }

        public class ApplicationRefreshTokenProvider : AuthenticationTokenProvider
        {
            public override void Create(AuthenticationTokenCreateContext context)
            {
                // Expiration time in seconds
                int expire = 5 * 60;
                context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddSeconds(expire));
                context.SetToken(context.SerializeTicket());
            }

            public override void Receive(AuthenticationTokenReceiveContext context)
            {
                context.DeserializeTicket(context.Token);
            }
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}