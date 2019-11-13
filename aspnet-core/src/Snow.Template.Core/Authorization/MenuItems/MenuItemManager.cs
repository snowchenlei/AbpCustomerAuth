using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Features;
using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using Snow.Template.Authorization.Users;

namespace Snow.Template.Authorization.MenuItems
{
    public class MenuItemManager : TemplateDomainServiceBase, IMenuItemManager
    {
        public IAbpSession AbpSession { get; set; }
        private readonly IIocResolver _iocResolver;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<MenuItem> _menuItemRepository;
        private readonly UserManager _userManager;
        private readonly ILocalizationContext _localizationContext;

        public MenuItemManager(IRepository<MenuItem> menuItemRepository
            , UserManager userManager
            , ILocalizationContext localizationContext, IIocResolver iocResolver, ICacheManager cacheManager)
        {
            _menuItemRepository = menuItemRepository;
            this._userManager = userManager;
            _localizationContext = localizationContext;
            _iocResolver = iocResolver;
            _cacheManager = cacheManager;
        }

        public async Task<List<MenuItem>> GetMenuItemsAsync()
        {
            return await _cacheManager.GetCache("CoreCache")
                 .GetAsync("MenuItemAllList", async () =>
                 {
                     return await _menuItemRepository.GetAll()
                         .AsNoTracking()
                         .ToListAsync();
                 });
        }

        public async Task<MenuItem> GetMenuItemAsync(int id)
        {
            return await _cacheManager.GetCache("CoreCache")
                .GetAsync("MenuItem", async () =>
                {
                    return await _menuItemRepository.GetAll()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(a => a.Id == id);
                });
        }

        public async Task<UserMenu> GetMenuItemTree(UserIdentifier user)
        {
            //TODO:权限过滤
            UserMenu root = new UserMenu()
            {
                Items = new List<UserMenuItem>()
            };
            //List<MenuItem> menus = await GetMenuItemsAsync();
            User currentUser = _userManager.GetUserById(AbpSession.UserId.Value);
            var grantedPermissions = await _userManager.GetGrantedPermissionsAsync(currentUser);
            List<MenuItem> menus = await _menuItemRepository.GetAllListAsync();
            menus = menus.Where(m => grantedPermissions.Any(a => a.Name == m.PermissionName) || m.PermissionName == null).ToList();
            foreach (MenuItem menuItem in menus.Where(m => m.Parent == null).OrderBy(m => m.Sort))
            {
                UserMenuItem currentUserMenuIte = new UserMenuItem(new MenuItemDefinition(
                    menuItem.Name,
                    L(menuItem.Name),
                    url: menuItem.Route,
                    icon: menuItem.Icon,
                    target: "tab_" + menuItem.Id,
                    requiredPermissionName: menuItem.PermissionName,
                    order: menuItem.Sort
                ), _localizationContext);
                await CreateChild(user, currentUserMenuIte, menus.Where(m => m.Parent?.Id == menuItem.Id).OrderBy(m => m.Sort));
                root.Items.Add(currentUserMenuIte);
            }
            return root;
        }

        private async Task CreateChild(UserIdentifier user, UserMenuItem currentUserMenuIte, IEnumerable<MenuItem> childs)
        {
            using (var scope = _iocResolver.CreateScope())
            {
                var permissionDependencyContext = scope.Resolve<PermissionDependencyContext>();
                permissionDependencyContext.User = user;

                var featureDependencyContext = scope.Resolve<FeatureDependencyContext>();
                featureDependencyContext.TenantId = user == null ? null : user.TenantId;

                foreach (MenuItem menuItem in childs)
                {
                    MenuItemDefinition menuItemDefinition = new MenuItemDefinition(
                        menuItem.Name,
                        L(menuItem.Name),
                        url: menuItem.Route,
                        icon: menuItem.Icon,
                        target: "tab_" + menuItem.Id,
                        requiredPermissionName: menuItem.PermissionName,
                        order: menuItem.Sort
                    );
                    if (menuItemDefinition.RequiresAuthentication && user == null)
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(menuItemDefinition.RequiredPermissionName))
                    {
                        var permissionDependency = new SimplePermissionDependency(menuItemDefinition.RequiredPermissionName);
                        try
                        {
                            if (user == null ||
                                !(await permissionDependency.IsSatisfiedAsync(permissionDependencyContext)))
                            {
                                continue;
                            }
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                    }

                    if (menuItemDefinition.PermissionDependency != null &&
                        (user == null || !(await menuItemDefinition.PermissionDependency.IsSatisfiedAsync(permissionDependencyContext))))
                    {
                        continue;
                    }

                    if (menuItemDefinition.FeatureDependency != null &&
                        (AbpSession.MultiTenancySide == MultiTenancySides.Tenant || (user != null && user.TenantId != null)) &&
                        !(await menuItemDefinition.FeatureDependency.IsSatisfiedAsync(featureDependencyContext)))
                    {
                        continue;
                    }
                    currentUserMenuIte.Items.Add(new UserMenuItem(menuItemDefinition, _localizationContext));
                }
            }

            //foreach (MenuItem menuItem in childs)
            //{
            //    MenuItemDefinition menuItemDefinition = new MenuItemDefinition(
            //        menuItem.Name,
            //        L(menuItem.Name),
            //        url: menuItem.Route,
            //        icon: menuItem.Icon,
            //        target: "tab_" + menuItem.Id,
            //        requiredPermissionName: menuItem.PermissionName,
            //        order: menuItem.Sort
            //    );
            //    var permissionDependency = new SimplePermissionDependency(menuItemDefinition.RequiredPermissionName);
            //    if (user == null || !(await permissionDependency.IsSatisfiedAsync(permissionDependencyContext)))
            //    {
            //        continue;
            //    }
            //    currentUserMenuIte.Items.Add(new UserMenuItem(menuItemDefinition, _localizationContext));
            //}
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TemplateConsts.LocalizationSourceName);
        }
    }
}