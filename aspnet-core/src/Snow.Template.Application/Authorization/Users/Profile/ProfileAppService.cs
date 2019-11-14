using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snow.Template.Authorization.Users.Profile.Dto;

namespace Snow.Template.Authorization.Users.Profile
{
    internal class ProfileAppService : TemplateAppServiceBase, IProfileAppService
    {
        public Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input)
        {
            throw new NotImplementedException();
        }
    }
}