using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Snow.Template.Authorization.MultiTenancy.Dto;

namespace Snow.Template.Authorization.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>//IApplicationService
    {
        Task<PagedResultDto<TenantListDto>> GetPagedAsync(GetTenantsInput input);

        Task<GetTenantForEditOutput> GetForEditAsync(NullableIdDto<int> input);

        Task CreateOrEditAsync(CreateOrUpdateTenantInput input);

        Task DeleteAsync(EntityDto input);
    }
}