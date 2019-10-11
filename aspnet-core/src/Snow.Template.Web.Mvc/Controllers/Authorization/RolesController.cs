using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Snow.Template.Authorization;
using Snow.Template.Authorization.Roles;
using Snow.Template.Authorization.Roles.Dto;
using Snow.Template.Controllers;
using Snow.Template.Web.Models.Roles;

namespace Snow.Template.Web.Controllers.Authorization
{
    [AbpMvcAuthorize(PermissionNames.Pages_Administration_Roles)]
    public class RolesController : TemplateControllerBase
    {
        private readonly IRoleAppService _roleAppService;
        private readonly IMapper _mapper;

        public RolesController(IRoleAppService roleAppService
            , IMapper mapper)
        {
            _roleAppService = roleAppService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Load(GetRolesInput input)
        {
            var roles = await _roleAppService.GetRoles(input);
            return Json(roles);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Administration_Roles_Create, PermissionNames.Pages_Administration_Roles_Edit)]
        public async Task<ActionResult> CreateOrEditModal(int? id)
        {
            GetRoleForEditOutput output = await _roleAppService.GetRoleForEdit(new NullableIdDto { Id = id });
            var viewModel = _mapper.Map<CreateOrEditRoleModalViewModel>(output);

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}