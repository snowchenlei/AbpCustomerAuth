using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Zhn.Template.Authorization.MenuItems;
using Zhn.Template.Authorization.MenuItems.Dto;
using Zhn.Template.Controllers;
using Zhn.Template.Web.Models.MenuItems;

namespace Zhn.Template.Web.Controllers.Authorization
{
    public class MenuItemsController : TemplateControllerBase
    {
        private readonly IMenuItemAppService _menuItemAppService;

        public MenuItemsController(IMenuItemAppService menuItemAppService)
        {
            _menuItemAppService = menuItemAppService;
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> EditUserModal(int? menuItemId)
        {
            var menuItem = await _menuItemAppService.GetForEdit(new NullableIdDto(menuItemId));
            var model = new EditMenuItemModalViewModel()
            {
                MenuItem = menuItem.MenuItem
            };
            return View("_EditModelItemModal", model);
        }
    }
}