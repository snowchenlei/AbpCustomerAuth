using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Snow.Template.Authorization.MultiTenancy.Dto;

namespace Snow.Template.Authorization.MultiTenancy
{
    /// <summary>
    /// 租户管理
    /// </summary>
    public interface ITenantAppService : IApplicationService
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns></returns>
        Task<PagedResultDto<TenantListDto>> GetPagedAsync(GetTenantsInput input);

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        Task<TenantEditDto> GetForEditAsync(NullableIdDto<int> input);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateAsync(CreateTenantInput input);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAsync(TenantEditDto input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto input);
    }
}