using Abp.Application.Navigation;
using Abp.Localization;
using Zhn.Template.Authorization;

namespace Zhn.Template.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class TemplateNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        "SystemMenu",
                        L("SystemMenu"),
                        icon: "menu"
                    ).AddItem(
                        new MenuItemDefinition(
                            PageNames.Tenants,
                            L("Tenants"),
                            url: "Tenants",
                            icon: "business",
                            target: "tab_1",
                            requiredPermissionName: PermissionNames.Pages_Tenants
                        )
                    ).AddItem(
                        new MenuItemDefinition(
                            PageNames.MenuItems,
                            L("MenuItems"),
                            url: "MenuItems",
                            icon: "people",
                            target: "tab_2",
                            requiredPermissionName: PermissionNames.Pages_Administration_MenuItems
                        )
                    ).AddItem(
                        new MenuItemDefinition(
                            PageNames.Users,
                            L("Users"),
                            url: "Users",
                            icon: "people",
                            target: "tab_3",
                            requiredPermissionName: PermissionNames.Pages_Administration_Users
                        )
                    ).AddItem(
                        new MenuItemDefinition(
                            PageNames.Roles,
                            L("Roles"),
                            url: "Roles",
                            icon: "local_offer",
                            target: "tab_4",
                            requiredPermissionName: PermissionNames.Pages_Administration_Roles
                        )
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TemplateConsts.LocalizationSourceName);
        }
    }
}