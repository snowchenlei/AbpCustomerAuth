﻿using System;
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
using Snow.Template.Authorization.Accounts;
using Snow.Template.Authorization.Roles;
using Snow.Template.Authorization.Roles.Dto;
using Snow.Template.Authorization.Users.Dto;
using Snow.Template.Authorization.Users.Exporting;
using Snow.Template.Dto;
using Abp.Domain.Uow;
using Snow.Template.Storage;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Snow.Template.Authorization.Users
{
    [AbpAuthorize(PermissionNames.Pages_Administration_Users)]
    public class UserAppService : TemplateAppServiceBase, IUserAppService
    {
        private const int MaxProfilPictureBytes = 5242880; //5MB
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IMapper _mapper;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IUserListExcelExporter _userListExcelExporter;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;

        public UserAppService(
            IAbpSession abpSession,
            UserManager userManager,
            RoleManager roleManager,
            IWebHostEnvironment environment,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
             IBinaryObjectManager binaryObjectManager,
            ITempFileCacheManager tempFileCacheManager,
            LogInManager logInManager, IRepository<User, long> userRepository
            , IMapper mapper
            , IRepository<UserRole, long> userRoleRepository
            , IUserListExcelExporter userListExcelExporter)
        {
            _binaryObjectManager = binaryObjectManager;
            _tempFileCacheManager = tempFileCacheManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _environment = environment;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _userRepository = userRepository;
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
            _userListExcelExporter = userListExcelExporter;
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users)]
        public async Task<PagedResultDto<UserListDto>> GetPagedAsync(GetUsersInput input)
        {
            var query = GetUsersFilteredQuery(input);

            var userCount = await query.CountAsync();

            var users = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var userListDtos = _mapper.Map<List<UserListDto>>(users);
            await FillRoleNamesAsync(userListDtos);

            return new PagedResultDto<UserListDto>(
                userCount,
                userListDtos
            );
        }

        public async Task<FileDto> GetToExcelAsync(GetUsersToExcelInput input)
        {
            var query = GetUsersFilteredQuery(input);

            var users = await query
                .OrderBy(input.Sorting)
                .ToListAsync();

            var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
            await FillRoleNamesAsync(userListDtos);

            return _userListExcelExporter.ExportToFile(userListDtos);
        }

        /// <summary>
        /// 获取用户修改信息
        /// </summary>
        /// <param name="input">id</param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Create, PermissionNames.Pages_Administration_Users_Edit)]
        public async Task<GetUserForEditOutput> GetForEditAsync(NullableIdDto<long> input)
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
                User user = await _userRepository.GetAsync(input.Id.Value);
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
        public async Task CreateOrUpdateAsync(CreateOrUpdateUserInput input)
        {
            if (input.User.Id.HasValue)
            {
                await UpdateAsync(input);
            }
            else
            {
                await CreateAsync(input);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Create)]
        protected virtual async Task CreateAsync(CreateOrUpdateUserInput input)
        {
            var user = ObjectMapper.Map<User>(input.User);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.User.Password));

            if (input.AssignedRoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.AssignedRoleNames));
            }

            CurrentUnitOfWork.SaveChanges();
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Edit)]
        protected virtual async Task UpdateAsync(CreateOrUpdateUserInput input)
        {
            Debug.Assert(input.User.Id != null, "input.User.Id != null");
            var user = await _userManager.GetUserByIdAsync(input.User.Id.Value);

            user = _mapper.Map(input.User, user);
            user.SetNormalizedNames();

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.AssignedRoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.AssignedRoleNames));
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Delete)]
        public async Task DeleteAsync(EntityDto<long> input)
        {
            if (input.Id == AbpSession.GetUserId())
            {
                throw new UserFriendlyException(L("YouCanNotDeleteOwnAccount"));
            }

            var user = await UserManager.GetUserByIdAsync(input.Id);
            CheckErrors(await UserManager.DeleteAsync(user));
        }

        public async Task<ListResultDto<RoleDto>> GetRolesAsync()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguageAsync(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto input)
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

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto input)
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
        private async Task FillRoleNamesAsync(IReadOnlyCollection<UserListDto> userListDtos)
        {
            /* This method is optimized to fill role names to given list. */
            var userIds = userListDtos.Select(u => u.Id);
            var userRoles = await _userRoleRepository.GetAll()
                .Where(userRole => userIds.Contains(userRole.UserId))
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

        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateHeadImage(UpdateHeadImageInput input)
        {
            string fixedFilePath = String.Empty, absoluteFilePath = String.Empty;
            string absolutePath = "/AppData/FileUpload/Image/HeadImage/";
            string fixedPath = _environment.ContentRootPath + absolutePath;
            var imageBytes = _tempFileCacheManager.GetFile(input.FileToken);

            if (imageBytes == null)
            {
                throw new UserFriendlyException("There is no such image file with the token: " + input.FileToken);
            }
            using (var bmpImage = new Bitmap(new MemoryStream(imageBytes)))
            {
                var width = (input.Width == 0 || input.Width > bmpImage.Width) ? bmpImage.Width : input.Width;
                var height = (input.Height == 0 || input.Height > bmpImage.Height) ? bmpImage.Height : input.Height;
                var bmCrop = bmpImage.Clone(new Rectangle(input.X, input.Y, width, height), bmpImage.PixelFormat);
                // TODO:限制裁剪后图片大小 1、先将图片保存成流 2、判断大小 3、大了结束。小了再将流保存成图片
                if (!Directory.Exists(fixedPath))
                {
                    Directory.CreateDirectory(fixedPath);
                }
                absoluteFilePath = absolutePath + input.FileToken + ".jpg";
                fixedFilePath = fixedPath + input.FileToken + ".jpg";
                bmCrop.Save(fixedFilePath, bmpImage.RawFormat);
            }

            User user = await UserManager.GetUserByIdAsync(AbpSession.GetUserId());

            if (user.ProfilePictureId.HasValue)
            {
                await _binaryObjectManager.DeleteAsync(user.ProfilePictureId.Value);
            }

            var storedFile = new BinaryObject(AbpSession.TenantId, absoluteFilePath);
            await _binaryObjectManager.SaveAsync(storedFile);

            user.ProfilePictureId = storedFile.Id;
        }

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <returns></returns>
        public async Task<GetHeadImageOutput> GetHeadImageAsync()
        {
            var user = await UserManager.GetUserByIdAsync(AbpSession.GetUserId());
            if (user.ProfilePictureId == null)
            {
                return new GetHeadImageOutput(string.Empty);
            }

            return await GetHeadImageByIdAsync(user.ProfilePictureId.Value);
        }

        public async Task<GetHeadImageOutput> GetHeadImageByIdAsync(Guid profilePictureId)
        {
            return await GetHeadImageByIdInternal(profilePictureId);
        }

        private async Task<GetHeadImageOutput> GetHeadImageByIdInternal(Guid profilePictureId)
        {
            string path = await GetHeadImageByIdOrNull(profilePictureId);
            if (path == null)
            {
                return new GetHeadImageOutput(string.Empty);
            }

            return new GetHeadImageOutput(path);
        }

        private async Task<string> GetHeadImageByIdOrNull(Guid profilePictureId)
        {
            var file = await _binaryObjectManager.GetOrNullAsync(profilePictureId);
            if (file == null)
            {
                return null;
            }

            return file.FilePath;
        }
    }
}