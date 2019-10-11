using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Snow.Template.Authorization;
using Snow.Template.Authorization.MenuItems;
using Snow.Template.Authorization.MenuItems.Dto;
using Snow.Template.Controllers;
using Snow.Template.Web.Models.MenuItems;
using Snow.Template.Web.Models.Roles;

namespace Snow.Template.Web.Controllers.Authorization
{
    [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems)]
    public class MenuItemsController : TemplateControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMenuItemAppService _menuItemAppService;

        public MenuItemsController(IMenuItemAppService menuItemAppService
            , IMapper mapper)
        {
            _mapper = mapper;
            _menuItemAppService = menuItemAppService;
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems)]
        public ActionResult Index()
        {
            return View();
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems)]
        public async Task<JsonResult> Load(GetMenuItemsInput input)
        {
            var menuItems = await _menuItemAppService.GetMenuItems(input);
            return Json(menuItems);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Administration_MenuItems_Create, PermissionNames.Pages_Administration_MenuItems_Edit)]
        public async Task<ActionResult> CreateOrEditModal(int? menuItemId)
        {
            var output = await _menuItemAppService.GetMenuItemForEdit(new NullableIdDto { Id = menuItemId });
            var viewModel = _mapper.Map<CreateOrEditMenuItemModalViewModel>(output);

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}