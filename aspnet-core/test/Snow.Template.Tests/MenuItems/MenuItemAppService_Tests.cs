using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Snow.Template.Authorization.MenuItems;
using Snow.Template.Authorization.MenuItems.Dto;

namespace Snow.Template.Tests.MenuItems
{
    public class MenuItemAppService_Tests : TemplateTestBase
    {
        private readonly IMenuItemAppService _menuItemAppService;

        public MenuItemAppService_Tests()
        {
            _menuItemAppService = Resolve<IMenuItemAppService>();
        }

        [Fact]
        public async Task GetMenuItems_Test()
        {
            // Act
            var output = await _menuItemAppService.GetPagedAsync(new GetMenuItemsInput() { MaxResultCount = 20, SkipCount = 0 });

            // Assert
            output.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetMenuItem_Test()
        {
            // Arrange
            var menuItem = await UsingDbContextAsync(async context => await context.MenuItem.FirstOrDefaultAsync());
            // Act
            var output = await _menuItemAppService.GetMenuItemForEdit(new NullableIdDto(menuItem.Id));
            // Assert
            output.MenuItem.Name.ShouldBeSameAs(menuItem.Name);
        }

        [Fact]
        public async Task CreateMenuItem_Test()
        {
            // Act
            await _menuItemAppService.CreateOrEditAsync(
                new CreateOrUpdateMenuItemInput()
                {
                    MenuItem = new MenuItemEditDto()
                    {
                        Name = "系统管理",
                        PermissionName = "Pages.Admin.System",
                        Route = "/Sys"
                    }
                });
            // Assert
            await UsingDbContextAsync(async context =>
            {
                var menuItem = await context.MenuItem.FirstOrDefaultAsync(u => u.Name == "系统管理");
                menuItem.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task UpdateMenuItem_Test()
        {
            // Arrange
            MenuItem menuItem = await UsingDbContextAsync(async context => await context.MenuItem.FirstOrDefaultAsync());
            string newName = menuItem.Name + "_new";

            // Act
            await _menuItemAppService.CreateOrEditAsync(
                new CreateOrUpdateMenuItemInput()
                {
                    MenuItem = new MenuItemEditDto()
                    {
                        Id = menuItem.Id,
                        Name = newName,
                        PermissionName = menuItem.PermissionName,
                        Sort = menuItem.Sort,
                        Icon = menuItem.Icon
                    }
                });

            // Assert
            await UsingDbContextAsync(async context =>
            {
                var johnNashRole = await context.MenuItem.FirstOrDefaultAsync(u => u.Id == menuItem.Id);
                johnNashRole.Name.ShouldBeSameAs(newName);
            });
        }

        [Fact]
        public async Task DeleteMenuItem_Test()
        {
            // Arrange
            MenuItem menuItem = await UsingDbContextAsync(async context => await context.MenuItem.FirstOrDefaultAsync());
            // Act
            await _menuItemAppService.DeleteAsync(new EntityDto(menuItem.Id));
            // Assert
            await UsingDbContextAsync(async context =>
            {
                var newMenuItem = await context.MenuItem.FirstOrDefaultAsync(r => r.Id == menuItem.Id);
                newMenuItem.ShouldBeNull();
            });
        }
    }
}