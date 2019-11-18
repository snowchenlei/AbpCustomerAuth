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
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<UserListDto>> GetPagedAsync(GetUsersInput input);

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<FileDto> GetToExcelAsync(GetUsersToExcelInput input);

        /// <summary>
        /// 获取详细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetUserForEditOutput> GetForEditAsync(NullableIdDto<long> input);

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdateUserInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<long> input);

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<RoleDto>> GetRolesAsync();

        /// <summary>
        /// 修改语言
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangeLanguageAsync(ChangeUserLanguageDto input);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> ChangePasswordAsync(ChangePasswordDto input);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> ResetPasswordAsync(ResetPasswordDto input);

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <returns></returns>
        Task<GetHeadImageOutput> GetHeadImageAsync();

        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateHeadImage(UpdateHeadImageInput input);
    }
}