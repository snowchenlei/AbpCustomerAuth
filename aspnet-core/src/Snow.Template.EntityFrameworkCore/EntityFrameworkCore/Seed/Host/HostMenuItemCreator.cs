using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snow.Template.Authorization.MenuItems;
using Snow.Template.Authorization.Roles;

namespace Snow.Template.EntityFrameworkCore.Seed.Host
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
            var t = _context.MenuItem.IgnoreQueryFilters().FirstOrDefault(c => c.Name == "系统管理");
            if (t != null)
            {
                return;
            }
            MenuItem systemManager = new MenuItem()
            {
                Name = "系统管理",
                CreationTime = DateTime.Now,
                Icon = "fas fa-cog",
                Parent = null
            };
            _context.MenuItem.Add(systemManager);
            _context.MenuItem.Add(new MenuItem()
            {
                Name = "用户管理",
                CreationTime = DateTime.Now,
                Icon = "far fa-circle nav-icon",
                Route = "/Users",
                PermissionName = "Pages.Administration.Users",
                Parent = systemManager
            });
            _context.MenuItem.Add(new MenuItem()
            {
                Name = "角色管理",
                CreationTime = DateTime.Now,
                Icon = "far fa-circle nav-icon",
                Route = "/Roles",
                PermissionName = "Pages.Administration.Roles",
                Parent = systemManager
            });
            _context.MenuItem.Add(new MenuItem()
            {
                Name = "菜单管理",
                CreationTime = DateTime.Now,
                Icon = "far fa-circle nav-icon",
                Route = "/MenuItems",
                PermissionName = "Pages.Administration.MenuItems",
                Parent = systemManager
            });
            _context.MenuItem.Add(new MenuItem()
            {
                Name = "租户管理",
                CreationTime = DateTime.Now,
                Icon = "far fa-circle nav-icon",
                Route = "/Tenants",
                PermissionName = "Pages.Administration.Tenants",
                Parent = systemManager
            });
            _context.MenuItem.Add(new MenuItem()
            {
                Name = "日志管理",
                CreationTime = DateTime.Now,
                Icon = "far fa-circle nav-icon",
                Route = "/AuditLogs",
                PermissionName = "Pages.Administration.AuditLogs",
                Parent = systemManager
            });
            MenuItem dataManager = new MenuItem()
            {
                Name = "数据管理",
                CreationTime = DateTime.Now,
                Icon = "fas fa-cog",
                Parent = null
            };
            _context.MenuItem.Add(dataManager);
            _context.MenuItem.Add(new MenuItem()
            {
                Name = "参数类型管理",
                CreationTime = DateTime.Now,
                Icon = "far fa-circle nav-icon",
                Route = "/ParameterTypes",
                PermissionName = "Pages.Administration.ParameterTypes",
                Parent = dataManager
            });
            _context.MenuItem.Add(new MenuItem()
            {
                Name = "参数管理",
                CreationTime = DateTime.Now,
                Icon = "far fa-circle nav-icon",
                Route = "/Parameters",
                PermissionName = "Pages.Administration.Parameters",
                Parent = dataManager
            });
        }
    }
}