using System.Collections.Generic;
using System.Linq;
using Abp.Application.Navigation;
using Abp.Domain.Repositories;
using Abp.Localization;
using Zhn.Template.Authorization;
using Zhn.Template.Authorization.MenuItems;

namespace Zhn.Template.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class TemplateNavigationProvider : NavigationProvider
    {
        private readonly IRepository<MenuItem> _menuItemRepository;

        public TemplateNavigationProvider(IRepository<MenuItem> menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        private void SetNavigation(MenuItemDefinition menuItemDefinition, List<MenuItem> menuItems, int parentId)
        {
            foreach (MenuItem menuItem in menuItems.Where(m => m.Parent?.Id == parentId))
            {
                menuItemDefinition.AddItem(new MenuItemDefinition(
                    menuItem.Name,
                    L(menuItem.Name),
                    url: menuItem.Route,
                    icon: menuItem.Icon,
                    target: "tab_" + menuItem.Id,
                    requiredPermissionName: menuItem.PermissionName,
                    order: menuItem.Sort
                ));
            }
        }

        public override void SetNavigation(INavigationProviderContext context)
        {
            //TODO:仅在项目启动时创建，未提供方法动态更新。
            //List<MenuItem> menuItems = _menuItemRepository.GetAllList();
            //foreach (MenuItem menuItem in menuItems.Where(m=>m.Parent == null))
            //{
            //    MenuItemDefinition currentDefinition = new MenuItemDefinition(
            //        menuItem.Name,
            //        L(menuItem.Name),
            //        order: menuItem.Sort
            //    );
            //    SetNavigation(currentDefinition, menuItems, menuItem.Id);
            //    context.Manager.MainMenu.AddItem(currentDefinition);
            //}

            //context.Manager.MainMenu
            //    .AddItem(
            //        new MenuItemDefinition(
            //            "SystemMenu",
            //            L("SystemMenu"),
            //            icon: "menu"
            //        ).AddItem(
            //            new MenuItemDefinition(
            //                PageNames.Tenants,
            //                L("Tenants"),
            //                url: "Tenants",
            //                icon: "business",
            //                target: "tab_1",
            //                requiredPermissionName: PermissionNames.Pages_Tenants
            //            )
            //        ).AddItem(
            //            new MenuItemDefinition(
            //                PageNames.MenuItems,
            //                L("MenuItems"),
            //                url: "MenuItems",
            //                icon: "people",
            //                target: "tab_2",
            //                requiredPermissionName: PermissionNames.Pages_Administration_MenuItems
            //            )
            //        ).AddItem(
            //            new MenuItemDefinition(
            //                PageNames.Users,
            //                L("Users"),
            //                url: "Users",
            //                icon: "people",
            //                target: "tab_3",
            //                requiredPermissionName: PermissionNames.Pages_Administration_Users
            //            )
            //        ).AddItem(
            //            new MenuItemDefinition(
            //                PageNames.Roles,
            //                L("Roles"),
            //                url: "Roles",
            //                icon: "local_offer",
            //                target: "tab_4",
            //                requiredPermissionName: PermissionNames.Pages_Administration_Roles
            //            )
            //        )
            //    );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TemplateConsts.LocalizationSourceName);
        }
    }
}