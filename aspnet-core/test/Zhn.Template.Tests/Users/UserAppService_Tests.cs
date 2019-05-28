﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
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
            var output = await _userAppService.GetUsers(new GetUsersInput() {MaxResultCount = 20, SkipCount = 0});

            // Assert
            output.Items.Count.ShouldBeGreaterThan(0);
        }
        [Fact]
        public async Task GetUser_Test()
        {
            // Act
            var output = await _userAppService.GetUserForEdit(new NullableIdDto<long>(1));

            // Assert
            output.ShouldNotBeNull();
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

            await UsingDbContextAsync(async context =>
            {
                var johnNashUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "john.nash");
                johnNashUser.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task UpdateUser_Test()
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

            await UsingDbContextAsync(async context =>
            {
                var johnNashUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "john.nash");
                johnNashUser.ShouldNotBeNull();
            });
        }
    }
}


