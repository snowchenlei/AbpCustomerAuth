using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Snow.Template.Authorization.Roles.Dto;

namespace Snow.Template.Authorization.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task<PagedResultDto<RoleListDto>> GetRoles(GetRolesInput input);

        Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input);

        Task<ListResultDto<PermissionDto>> GetAllPermissions();

        Task<ListResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input);

        Task CreateOrUpdateRole(CreateOrUpdateRoleInput input);

        Task DeleteRole(EntityDto<int> input);
    }
}
