using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zhn.Template.Authorization;
using Zhn.Template.Authorization.MenuItems;
using Zhn.Template.Authorization.MenuItems.Dto;
using Zhn.Template.Controllers;
using Zhn.Template.Web.Models.MenuItems;
using Zhn.Template.Web.Models.Roles;

namespace Zhn.Template.Web.Controllers.Authorization
{
    public class MenuItemsController : TemplateControllerBase
    {
        private readonly IMenuItemAppService _menuItemAppService;

        public MenuItemsController(IMenuItemAppService menuItemAppService)
        {
            _menuItemAppService = menuItemAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Load(GetMenuItemsInput input)
        {
            var menuItems = await _menuItemAppService.GetMenuItems(input);
            return Json(menuItems);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Administration_MenuItems_Create, PermissionNames.Pages_Administration_MenuItems_Edit)]
        public async Task<ActionResult> CreateOrEditModal(int? id)
        {
            var output = await _menuItemAppService.GetMenuItemForEdit(new NullableIdDto { Id = id });
            var viewModel = new CreateOrEditMenuItemModalViewModel(output);

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}