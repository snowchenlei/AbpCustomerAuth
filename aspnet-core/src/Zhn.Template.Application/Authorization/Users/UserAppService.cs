using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Configuration;
using Abp.Zero.Configuration;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zhn.Template.Authorization.Accounts;
using Zhn.Template.Authorization.Roles;
using Zhn.Template.Authorization.Roles.Dto;
using Zhn.Template.Authorization.Users.Dto;

namespace Zhn.Template.Authorization.Users
{
    [AbpAuthorize(PermissionNames.Pages_Administration_Users)]
    public class UserAppService : TemplateAppServiceBase, IUserAppService
    {
        private readonly IMapper _mapper;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;

        public UserAppService(
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager, IRepository<User, long> userRepository, IMapper mapper, IRepository<UserRole, long> userRoleRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _userRepository = userRepository;
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users)]
        public async Task<PagedResultDto<UserListDto>> GetUsers(GetUsersInput input)
        {
            var query = GetUsersFilteredQuery(input);

            var userCount = await query.CountAsync();

            var users = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var userListDtos = _mapper.Map<List<UserListDto>>(users);
            await FillRoleNames(userListDtos);

            return new PagedResultDto<UserListDto>(
                userCount,
                userListDtos
            );
        }

        /// <summary>
        /// 获取用户修改信息
        /// </summary>
        /// <param name="input">id</param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Create, PermissionNames.Pages_Administration_Users_Edit)]
        public async Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input)
        {
            var userRoleDtos = await _roleManager.Roles
                .OrderBy(r => r.DisplayName)
                .Select(r => new UserRoleDto
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    RoleDisplayName = r.DisplayName
                })
                .ToArrayAsync();
            var output = new GetUserForEditOutput()
            {
                Roles = userRoleDtos
            };
            if (input.Id.HasValue)
            {   //修改
                User user = await _userRepository.FirstOrDefaultAsync(input.Id.Value);
                if (user == null)
                {
                    throw new UserFriendlyException("用户不存在");
                }
                output.User = _mapper.Map<UserEditDto>(user);
                foreach (var userRoleDto in userRoleDtos)
                {
                    userRoleDto.IsAssigned = await UserManager.IsInRoleAsync(user, userRoleDto.RoleName);
                }
            }
            else
            {
                output.User = new UserEditDto()
                {
                    IsActive = true,
                    //ShouldChangePasswordOnNextLogin = true,
                    //IsTwoFactorEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled),
                    //IsLockoutEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled)
                };
                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    var defaultUserRole = userRoleDtos.FirstOrDefault(ur => ur.RoleName == defaultRole.Name);
                    if (defaultUserRole != null)
                    {
                        defaultUserRole.IsAssigned = true;
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Create, PermissionNames.Pages_Administration_Users_Edit)]
        public async Task CreateOrUpdateUser(CreateOrUpdateUserInput input)
        {
            if (input.User.Id.HasValue)
            {
                await UpdateUserAsync(input);
            }
            else
            {
                await CreateUserAsync(input);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Create)]
        protected virtual async Task CreateUserAsync(CreateOrUpdateUserInput input)
        {
            var user = ObjectMapper.Map<User>(input.User);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.User.Password));

            if (input.AssignedRoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.AssignedRoleNames));
            }

            CurrentUnitOfWork.SaveChanges();
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Edit)]
        protected virtual async Task UpdateUserAsync(CreateOrUpdateUserInput input)
        {
            Debug.Assert(input.User.Id != null, "input.User.Id != null");
            var user = await _userManager.GetUserByIdAsync(input.User.Id.Value);

            user = _mapper.Map(input.User, user);
            user.SetNormalizedNames();

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.AssignedRoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.AssignedRoleNames));
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Delete)]
        public async Task DeleteUser(EntityDto<long> input)
        {
            if (input.Id == AbpSession.GetUserId())
            {
                throw new UserFriendlyException(L("YouCanNotDeleteOwnAccount"));
            }

            var user = await UserManager.GetUserByIdAsync(input.Id);
            CheckErrors(await UserManager.DeleteAsync(user));
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to change password.");
            }
            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);
            var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Existing Password' did not match the one on record.  Please try again or contact an administrator for assistance in resetting your password.");
            }
            if (!new Regex(AccountAppService.PasswordRegex).IsMatch(input.NewPassword))
            {
                throw new UserFriendlyException("Passwords must be at least 8 characters, contain a lowercase, uppercase, and number.");
            }
            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to reset password.");
            }
            long currentUserId = _abpSession.UserId.Value;
            var currentUser = await _userManager.GetUserByIdAsync(currentUserId);
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }
            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                CurrentUnitOfWork.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// 获取用户过滤查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private IQueryable<User> GetUsersFilteredQuery(IGetUsersInput input)
        {
            return _userRepository.GetAll();
        }

        /// <summary>
        /// 添加角色信息
        /// </summary>
        /// <param name="userListDtos"></param>
        /// <returns></returns>
        private async Task FillRoleNames(IReadOnlyCollection<UserListDto> userListDtos)
        {
            /* This method is optimized to fill role names to given list. */

            var userRoles = await _userRoleRepository.GetAll()
                .Where(userRole => userListDtos.Any(user => user.Id == userRole.UserId))
                .Select(userRole => userRole).ToListAsync();

            var distinctRoleIds = userRoles.Select(userRole => userRole.RoleId).Distinct();

            foreach (var user in userListDtos)
            {
                var rolesOfUser = userRoles.Where(userRole => userRole.UserId == user.Id).ToList();
                user.Roles = _mapper.Map<List<UserListRoleDto>>(rolesOfUser);
            }

            var roleNames = new Dictionary<int, string>();
            foreach (var roleId in distinctRoleIds)
            {
                var role = await _roleManager.FindByIdAsync(roleId.ToString());
                if (role != null)
                {
                    roleNames[roleId] = role.DisplayName;
                }
            }

            foreach (var userListDto in userListDtos)
            {
                foreach (var userListRoleDto in userListDto.Roles)
                {
                    if (roleNames.ContainsKey(userListRoleDto.RoleId))
                    {
                        userListRoleDto.RoleName = roleNames[userListRoleDto.RoleId];
                    }
                }

                userListDto.Roles = userListDto.Roles.Where(r => r.RoleName != null).OrderBy(r => r.RoleName).ToList();
            }
        }
    }
}