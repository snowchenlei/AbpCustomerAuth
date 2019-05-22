using System;
using System.Collections.Generic;
using System.Text;
using Zhn.Template.Authorization.MenuItems;

namespace Zhn.Template.EntityFrameworkCore.Seed.Host
{
    public class HostMenuItemCreator
    {
        private readonly TemplateDbContext _context;

        public HostMenuItemCreator(TemplateDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateHostMenuItems();
        }

        private void CreateHostMenuItems()
        {
            MenuItem parent = new MenuItem()
            {
                Name = "系统管理",
                CreationTime = DateTime.Now,
                Icon = "fas fa-cog",
                Parent = null
            };
            _context.MenuItem.Add(parent);
            _context.MenuItem.Add(new MenuItem()
            {
                Name = "用户管理",
                CreationTime = DateTime.Now,
                Icon = "far fa-circle nav-icon",
                Route = "/Users",
                PermissionName = "Pages.Administration.Users",
                Parent = null
            });
            _context.MenuItem.Add(new MenuItem()
            {
                Name = "角色管理",
                CreationTime = DateTime.Now,
                Icon = "far fa-circle nav-icon",
                Route = "/Roles",
                PermissionName = "Pages.Administration.Roles",
                Parent = null
            });
            _context.MenuItem.Add(new MenuItem()
            {
                Name = "菜单管理",
                CreationTime = DateTime.Now,
                Icon = "far fa-circle nav-icon",
                Route = "/MenuItems",
                PermissionName = "Pages.Administration.MenuItems",
                Parent = null
            });
            _context.MenuItem.Add(new MenuItem()
            {
                Name = "租户管理",
                CreationTime = DateTime.Now,
                Icon = "far fa-circle nav-icon",
                Route = "/Tenants",
                PermissionName = "Pages.Administration.Tenants",
                Parent = null
            });
        }
    }
}
