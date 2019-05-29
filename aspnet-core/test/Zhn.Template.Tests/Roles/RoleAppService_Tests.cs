using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Zhn.Template.Authorization.Roles;
using Zhn.Template.Authorization.Roles.Dto;
using Zhn.Template.Authorization.Users;

namespace Zhn.Template.Tests.Roles
{
    public class RoleAppService_Tests : TemplateTestBase
    {
        private readonly IRoleAppService _roleAppService;

        public RoleAppService_Tests()
        {
            _roleAppService = Resolve<IRoleAppService>();
        }

        [Fact]
        public async Task GetRoles_Test()
        {
            // Act
            var output = await _roleAppService.GetRoles(new GetRolesInput() { MaxResultCount = 20, SkipCount = 0 });

            // Assert
            output.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetRole_Test()
        {
            // Arrange
            var defaultRole = await UsingDbContextAsync(async context => await context.Roles.FirstOrDefaultAsync(u => u.TenantId == AbpSession.TenantId));
            // Act
            var output = await _roleAppService.GetRoleForEdit(new NullableIdDto(defaultRole.Id));
            // Assert
            output.Role.DisplayName.ShouldBeSameAs(defaultRole.DisplayName);
        }

        [Fact]
        public async Task CreateRole_Test()
        {
            // Act
            await _roleAppService.CreateOrUpdateRole(
                new CreateOrUpdateRoleInput()
                {
                    Role = new RoleEditDto()
                    {
                        DisplayName = "test",
                        IsDefault = false
                    },
                    Permissions = new List<string>()
                });
            // Assert
            await UsingDbContextAsync(async context =>
            {
                var johnNashRole = await context.Roles.FirstOrDefaultAsync(u => u.DisplayName == "test");
                johnNashRole.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task UpdateRole_Test()
        {
            // Arrange
            Role defaultRole = await InitDateAsync();
            string newDisplayName = defaultRole.DisplayName + "_new";

            // Act
            await _roleAppService.CreateOrUpdateRole(
                new CreateOrUpdateRoleInput()
                {
                    Role = new RoleEditDto()
                    {
                        Id = defaultRole.Id,
                        DisplayName = newDisplayName
                    },
                    Permissions = new List<string>()
                });
            // Assert
            await UsingDbContextAsync(async context =>
            {
                var johnNashRole = await context.Roles.FirstOrDefaultAsync(u => u.Id == defaultRole.Id);
                johnNashRole.DisplayName.ShouldBeSameAs(newDisplayName);
            });
        }

        [Fact]
        public async Task DeleteRole_Test()
        {
            // Arrange
            Role defaultRole = await InitDateAsync();
            // Act
            await _roleAppService.DeleteRole(new EntityDto<int>(defaultRole.Id));
            // Assert
            await UsingDbContextAsync(async context =>
            {
                var johnNashUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == defaultRole.Id && r.TenantId == AbpSession.TenantId && r.IsDeleted == false);
                johnNashUser.ShouldBeNull();
            });
        }

        private async Task<Role> InitDateAsync()
        {
            var defaultRole = await UsingDbContextAsync(async context =>
            {
                UserRole userRole = await context.UserRoles.FirstOrDefaultAsync(u => u.UserId == AbpSession.UserId);
                Role role = await context.Roles.FirstOrDefaultAsync(u =>
                    u.Id != userRole.UserId && u.IsDeleted == false && u.TenantId == AbpSession.TenantId);
                if (role == null)
                {
                    role = new Role()
                    {
                        TenantId = AbpSession.TenantId,
                        Name = "John",
                        DisplayName = "John",
                    };
                    context.Add(role);
                    await context.SaveChangesAsync();
                }
                return role;
            });
            return defaultRole;
        }
    }
}