using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snow.Template.Authorization;
using Snow.Template.Controllers;
using Snow.Template.Authorization.MultiTenancy;
using Snow.Template.Authorization.MultiTenancy.Dto;
using Snow.Template.Web.Models.Tenants;
using AutoMapper;

namespace Snow.Template.Web.Controllers.Authorization
{
    /// <summary>
    /// 租户管理
    /// </summary>
    [AbpMvcAuthorize(PermissionNames.Pages_Administration_Tenants)]
    public class TenantsController : TemplateControllerBase
    {
        private readonly ITenantAppService _tenantAppService;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="tenantAppService"></param>
        public TenantsController(IMapper mapper
            , ITenantAppService tenantAppService)
        {
            _mapper = mapper;
            _tenantAppService = tenantAppService;
        }

        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var output = await _tenantAppService.GetPagedAsync(new GetTenantsInput { MaxResultCount = int.MaxValue }); // Paging not implemented yet
            return View(output);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public async Task<PartialViewResult> CreateModal()
        {
            return PartialView("_CreateModal");
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<PartialViewResult> EditModal(int? Id)
        {
            var tenantDto = await _tenantAppService.GetForEditAsync(new NullableIdDto(Id));
            var viewModel = new EditTenantViewModel(tenantDto);
            return PartialView("_EditModal", viewModel);
        }
    }
}