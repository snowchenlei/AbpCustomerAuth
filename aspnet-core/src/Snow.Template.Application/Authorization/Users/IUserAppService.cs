using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Snow.Template.Authorization.Users.Dto;
using Snow.Template.Authorization.Roles.Dto;
using Snow.Template.Dto;

namespace Snow.Template.Authorization.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task<PagedResultDto<UserListDto>> GetPagedAsync(GetUsersInput input);

        Task<FileDto> GetToExcelAsync(GetUsersToExcelInput input);

        Task<GetUserForEditOutput> GetForEditAsync(NullableIdDto<long> input);

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdateUserInput input);

        Task DeleteAsync(EntityDto<long> input);

        Task<ListResultDto<RoleDto>> GetRolesAsync();

        Task ChangeLanguageAsync(ChangeUserLanguageDto input);

        Task<bool> ChangePasswordAsync(ChangePasswordDto input);

        Task<bool> ResetPasswordAsync(ResetPasswordDto input);
    }
}