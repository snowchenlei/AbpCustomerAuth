using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Navigation;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Snow.Template.Authorization.MenuItems;

namespace Snow.Template.Web.Views.Shared.Components.SideBarNav
{
    public class SideBarNavViewComponent : TemplateViewComponent
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IMenuItemManager _menuItemManager;
        private readonly IUserNavigationManager _userNavigationManager;

        public SideBarNavViewComponent(
            IUserNavigationManager userNavigationManager,
            IAbpSession abpSession, MenuItemManager menuItemManager, ICacheManager cacheManager)
        {
            _userNavigationManager = userNavigationManager;
            _abpSession = abpSession;
            _menuItemManager = menuItemManager;
            _cacheManager = cacheManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string activeMenu = "")
        {
            var model = new SideBarNavViewModel
            {
                MainMenu = await _cacheManager.GetCache("ComponentCache").GetAsync("MainMenu",
                    async () =>
                        await _menuItemManager.GetMenuItemTree(_abpSession
                            .ToUserIdentifier())), //await _userNavigationManager.GetMenuAsync("MainMenu", _abpSession.ToUserIdentifier()),
                ActiveMenuItemName = activeMenu
            };

            return View(model);
        }
    }
}