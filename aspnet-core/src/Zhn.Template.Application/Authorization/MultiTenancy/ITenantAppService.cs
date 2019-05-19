using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Zhn.Template.Authorization.MultiTenancy.Dto;

namespace Zhn.Template.Authorization.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}



