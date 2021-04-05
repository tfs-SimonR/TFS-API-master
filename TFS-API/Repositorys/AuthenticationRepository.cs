using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TFS_API.Mappers;
using TFS_API.Models;
using TFS_API.Models.DTO;
using TFS_API.Models.Tables;

namespace TFS_API.Repositorys
{
    public class AuthenticationRepository
    {
        private UnitOfWork _unitOfWork;
        ApplicationDbContext _db = new ApplicationDbContext();

        public AuthenticationRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public int CreateAspUser(AuthDTO model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db))
            {
                PasswordValidator = new ApplicationUserManager.CustomPasswordValidator()
            };

            try
            {
                var createAppUser = AccountMappers.vmToEntity(model);
                var result = manager.Create(createAppUser);
                if (result.Succeeded)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}