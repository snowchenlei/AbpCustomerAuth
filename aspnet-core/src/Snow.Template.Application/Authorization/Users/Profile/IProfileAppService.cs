using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Snow.Template.Authorization.Users.Profile.Dto;

namespace Snow.Template.Authorization.Users.Profile
{
    public interface IProfileAppService : IApplicationService
    {
        Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input);

        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateProfilePicture(UpdateHeadImageInputInput input);
    }
}