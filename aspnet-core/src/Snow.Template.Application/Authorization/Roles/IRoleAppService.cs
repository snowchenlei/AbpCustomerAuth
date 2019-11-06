using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Snow.Template.Authorization.Roles.Dto;

namespace Snow.Template.Authorization.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task<PagedResultDto<RoleListDto>> GetPagedAsync(GetRolesInput input);

        Task<GetRoleForEditOutput> GetForEditAsync(NullableIdDto input);

        Task<ListResultDto<PermissionDto>> GetAllPermissionsAsync();

        Task<ListResultDto<RoleListDto>> GetListAsync(GetRolesInput input);

        Task CreateOrUpdateAsync(CreateOrUpdateRoleInput input);

        Task DeleteAsync(EntityDto<int> input);
    }
}