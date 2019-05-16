using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Navigation;
using Abp.Runtime.Session;
using Zhn.Template.Authorization.MenuItems;

namespace Zhn.Template.Web.Views.Shared.Components.SideBarNav
{
    public class SideBarNavViewComponent : TemplateViewComponent
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IAbpSession _abpSession;
        private readonly MenuItemManager _menuItemManager;

        public SideBarNavViewComponent(
            IUserNavigationManager userNavigationManager,
            IAbpSession abpSession, MenuItemManager menuItemManager)
        {
            _userNavigationManager = userNavigationManager;
            _abpSession = abpSession;
            _menuItemManager = menuItemManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string activeMenu = "")
        {
            var model = new SideBarNavViewModel
            {
                MainMenu = await _menuItemManager.GetMenuItems(),//await _userNavigationManager.GetMenuAsync("MainMenu", _abpSession.ToUserIdentifier()),
                ActiveMenuItemName = activeMenu
            };

            return View(model);
        }
    }
}


