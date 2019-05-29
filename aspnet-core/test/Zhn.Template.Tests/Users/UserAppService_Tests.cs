using System;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Zhn.Template.Authorization.Users;
using Zhn.Template.Authorization.Users.Dto;

namespace Zhn.Template.Tests.Users
{
    public class UserAppService_Tests : TemplateTestBase
    {
        private readonly IUserAppService _userAppService;

        public UserAppService_Tests()
        {
            _userAppService = Resolve<IUserAppService>();
        }

        [Fact]
        public async Task GetUsers_Test()
        {
            // Act
            var output = await _userAppService.GetUsers(new GetUsersInput() { MaxResultCount = 20, SkipCount = 0 });

            // Assert
            output.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetUser_Test()
        {
            // Arrange
            var defaultUser = await UsingDbContextAsync(async context => await context.Users.FirstOrDefaultAsync(u => u.TenantId == AbpSession.TenantId));
            // Act
            var output = await _userAppService.GetUserForEdit(new NullableIdDto<long>(defaultUser.Id));
            // Assert
            output.User.Name.ShouldBeSameAs(defaultUser.Name);
        }

        [Fact]
        public async Task CreateUser_Test()
        {
            // Act
            await _userAppService.CreateOrUpdateUser(
                new CreateOrUpdateUserInput()
                {
                    User = new UserEditDto()
                    {
                        EmailAddress = "john@volosoft.com",
                        IsActive = true,
                        Name = "John",
                        Surname = "Nash",
                        Password = "123qwe",
                        UserName = "john.nash"
                    },
                    AssignedRoleNames = new string[0]
                });
            // Assert
            await UsingDbContextAsync(async context =>
            {
                var johnNashUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "john.nash");
                johnNashUser.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task UpdateUser_Test()
        {
            User defaultUser = await InitDateAsync();
            // Act
            await _userAppService.CreateOrUpdateUser(
                new CreateOrUpdateUserInput()
                {
                    User = new UserEditDto()
                    {
                        Id = defaultUser.Id,
                        EmailAddress = "john.up@volosoft.com",
                        IsActive = true,
                        Name = "John",
                        Surname = "Nash",
                        Password = "123qwe",
                        UserName = "john.nash"
                    },
                    AssignedRoleNames = new string[0]
                });
            // Assert
            await UsingDbContextAsync(async context =>
            {
                var johnNashUser = await context.Users.FirstOrDefaultAsync(u => u.Id == defaultUser.Id);
                johnNashUser.EmailAddress.ShouldBeSameAs("john.up@volosoft.com");
            });
        }

        [Fact]
        public async Task DeleteUser_Test()
        {
            // Arrange
            User defaultUser = await InitDateAsync();
            // Act
            await _userAppService.DeleteUser(new EntityDto<long>(defaultUser.Id));
            // Assert
            await UsingDbContextAsync(async context =>
            {
                var johnNashUser = await context.Users.FirstOrDefaultAsync(u => u.Id == defaultUser.Id && u.TenantId == AbpSession.TenantId && u.IsDeleted == false);
                johnNashUser.ShouldBeNull();
            });
        }

        [Fact]
        public async Task DeleteUser_LoginUser_Test()
        {
            // Arrange
            User defaultUser = await UsingDbContextAsync(async context =>
                await context.Users.FirstOrDefaultAsync(u => u.Id == AbpSession.UserId));
            // Act Assert
            await Assert.ThrowsAsync<UserFriendlyException>(() => _userAppService.DeleteUser(new EntityDto<long>(defaultUser.Id)));
        }

        private async Task<User> InitDateAsync()
        {
            var defaultUser = await UsingDbContextAsync(async context =>
            {
                User user = await context.Users.FirstOrDefaultAsync(u =>
                    u.TenantId == AbpSession.TenantId && u.Id != AbpSession.UserId && u.IsDeleted == false);
                if (user == null)
                {
                    user = new User()
                    {
                        TenantId = AbpSession.TenantId,
                        EmailAddress = "john@volosoft.com",
                        IsActive = true,
                        Name = "John",
                        Surname = "Test",
                        Password = "123qwe",
                        UserName = "john.test",
                    };
                    context.Add(user);
                    await context.SaveChangesAsync();
                }
                return user;
            });
            return defaultUser;
        }
    }
}