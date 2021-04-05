using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFS_API.Models;
using TFS_API.Models.DTO;

namespace TFS_API.Mappers
{
    public class AccountMappers
    {
        public static UserAccountViewModels.RegisterViewModel ADLookupToViewModel(UserAccountViewModels.RegisterViewModel model, UserAccountViewModels.RegisterViewModel ADLU)
        {
            model.Surname = ADLU.Surname;
            model.Forenames = ADLU.Forenames;
            model.Email = ADLU.Email;
            model.DisplayName = ADLU.DisplayName;
            model.HiddenUsername = ADLU.HiddenUsername;
            return model;
        }

        public static ApplicationUser vmToEntity(UserAccountViewModels.RegisterViewModel model)
        {
            ApplicationUser newUser = new ApplicationUser
            {
                UserName = model.HiddenUsername,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()

            };

            return newUser;
        }

        public static ApplicationUser vmToEntity(AuthDTO model)
        {
            ApplicationUser newUser = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()

            };
            return newUser;
        }
    }
}