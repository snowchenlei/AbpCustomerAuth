using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zhn.Template.Authorization;
using Zhn.Template.Authorization.Roles;
using Zhn.Template.Authorization.Roles.Dto;
using Zhn.Template.Controllers;
using Zhn.Template.Web.Models.Roles;

namespace Zhn.Template.Web.Controllers.Authorization
{
    [AbpMvcAuthorize(PermissionNames.Pages_Administration_Roles)]
    public class RolesController : TemplateControllerBase
    {
        private readonly IRoleAppService _roleAppService;

        public RolesController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Load(PagedRoleResultRequestDto input)
        {
            var roles = await _roleAppService.GetAll(input);
            return Json(roles);
        }
        [AbpMvcAuthorize(PermissionNames.Pages_Administration_Roles_Create, PermissionNames.Pages_Administration_Roles_Edit)]

        public async Task<ActionResult> CreateOrEditModal(int? id)
        {
            var output = await _roleAppService.GetRoleForEdit(new NullableIdDto { Id = id });
            var viewModel = new CreateOrEditRoleModalViewModel(output);

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}


