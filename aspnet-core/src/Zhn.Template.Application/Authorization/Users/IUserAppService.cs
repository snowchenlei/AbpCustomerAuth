using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Zhn.Template.Authorization.Users.Dto;
using Zhn.Template.Authorization.Roles.Dto;

namespace Zhn.Template.Authorization.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input);

        Task ChangeLanguage(ChangeUserLanguageDto input);

        Task BatchDelete(List<long> input);
    }
}