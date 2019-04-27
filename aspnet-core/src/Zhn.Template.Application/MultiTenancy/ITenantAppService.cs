using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Zhn.Template.MultiTenancy.Dto;

namespace Zhn.Template.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}



