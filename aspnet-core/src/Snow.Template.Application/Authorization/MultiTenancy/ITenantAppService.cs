using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Snow.Template.Authorization.MultiTenancy.Dto;

namespace Snow.Template.Authorization.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}




