using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Zhn.Template.Authorization.Users.Dto;
using Zhn.Template.Authorization.Roles.Dto;

namespace Zhn.Template.Authorization.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task<PagedResultDto<UserListDto>> GetUsers(GetUsersInput input);
        Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input);
        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateUser(CreateOrUpdateUserInput input);

        Task DeleteUser(EntityDto<long> input);
        Task<ListResultDto<RoleDto>> GetRoles();
        

        Task ChangeLanguage(ChangeUserLanguageDto input);

        Task<bool> ChangePassword(ChangePasswordDto input);

        Task<bool> ResetPassword(ResetPasswordDto input);
    }
}