using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Localization;
using Microsoft.EntityFrameworkCore;

namespace Zhn.Template.Authorization.MenuItems
{
    public class MenuItemManager: IDomainService
    {
        private readonly IRepository<MenuItem> _menuItemRepository;
        private readonly ILocalizationContext _localizationContext;
        public MenuItemManager(IRepository<MenuItem> menuItemRepository, ILocalizationContext localizationContext)
        {
            _menuItemRepository = menuItemRepository;
            _localizationContext = localizationContext;
        }

        public async Task<UserMenu> GetMenuItems()
        {
            //TODO:权限过滤
            UserMenu root = new UserMenu()
            {
                Items = new List<UserMenuItem>()
            };
            List<MenuItem> menus = await _menuItemRepository.GetAllListAsync();//GetAll().OrderBy(a=>a.Sort).ToListAsync();
            //CreateChild(root, menus, menus.Where(m => m.Parent == null));
            foreach (MenuItem menuItem in menus.Where(m => m.Parent == null))
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
                CreateChild(currentUserMenuIte, menus.Where(m => m.Parent?.Id == menuItem.Id));
                root.Items.Add(currentUserMenuIte);
            }
            return root;
        }

        private void CreateChild(UserMenuItem currentUserMenuIte, IEnumerable<MenuItem> childs)
        {
            foreach (MenuItem menuItem in childs)
            {
                currentUserMenuIte.Items.Add(new UserMenuItem(new MenuItemDefinition(
                    menuItem.Name,
                    L(menuItem.Name),
                    url: menuItem.Route,
                    icon: menuItem.Icon,
                    target: "tab_" + menuItem.Id,
                    requiredPermissionName: menuItem.PermissionName,
                    order: menuItem.Sort
                ), _localizationContext));
            }
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TemplateConsts.LocalizationSourceName);
        }
    }
}
