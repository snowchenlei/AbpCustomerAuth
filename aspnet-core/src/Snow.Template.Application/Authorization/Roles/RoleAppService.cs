using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Snow.Template.Authorization.Users;
using Snow.Template.Authorization.Roles.Dto;

namespace Snow.Template.Authorization.Roles
{
    [AbpAuthorize(PermissionNames.Pages_Administration_Roles)]
    public class RoleAppService : TemplateAppServiceBase, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cache;

        public RoleAppService(IRepository<Role> repository, RoleManager roleManager, UserManager userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _roleRepository = repository;
        }

        public async Task<PagedResultDto<RoleListDto>> GetRoles(GetRolesInput input)
        {
            var query = GetRolesFilteredQuery(input);

            var userCount = await query.CountAsync();

            var users = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var userListDtos = _mapper.Map<List<RoleListDto>>(users);

            return new PagedResultDto<RoleListDto>(
                userCount,
                userListDtos
            );
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Create,
            PermissionNames.Pages_Administration_Roles_Edit)]
        public async Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input)
        {
            var permissions = PermissionManager.GetAllPermissions();

            RoleEditDto roleEditDto;

            List<FlatPermissionDto> flatPermissions = _mapper.Map<List<FlatPermissionDto>>(permissions);
            if (input.Id.HasValue) //Editing existing role?
            {
                var role = await _roleManager.GetRoleByIdAsync(input.Id.Value);
                string[] grantedPermissionNames = (await _roleManager.GetGrantedPermissionsAsync(role)).Select(p => p.Name).ToArray();
                foreach (FlatPermissionDto flatPermission in flatPermissions)
                {
                    if (grantedPermissionNames.Contains(flatPermission.Name))
                    {
                        flatPermission.IsSelected = true;
                    }
                }
                roleEditDto = _mapper.Map<RoleEditDto>(role);
            }
            else
            {
                roleEditDto = new RoleEditDto();
            }

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = flatPermissions.OrderBy(p => p.Name).ToList(),
            };
        }

        public async Task<ListResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input)
        {
            var roles = await _roleManager
                .Roles
                .WhereIf(
                    !input.Permission.IsNullOrWhiteSpace(),
                    r => r.Permissions.Any(rp => rp.Name == input.Permission && rp.IsGranted)
                )
                .ToListAsync();

            return new ListResultDto<RoleListDto>(_mapper.Map<List<RoleListDto>>(roles));
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Create, PermissionNames.Pages_Administration_Roles_Edit)]
        public async Task CreateOrUpdateRole(CreateOrUpdateRoleInput input)
        {
            if (input.Role.Id.HasValue)
            {
                await UpdateRoleAsync(input);
            }
            else
            {
                await CreateRoleAsync(input);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Create)]
        protected virtual async Task CreateRoleAsync(CreateOrUpdateRoleInput input)
        {
            var role = _mapper.Map<Role>(input.Role);
            role.SetNormalizedName();

            CheckErrors(await _roleManager.CreateAsync(role));
            if (input.GrantedPermissions.Any())
            {
                var grantedPermissions = PermissionManager
                    .GetAllPermissions()
                    .Where(p => input.GrantedPermissions.Contains(p.Name))
                    .ToList();

                await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Edit)]
        protected virtual async Task UpdateRoleAsync(CreateOrUpdateRoleInput input)
        {
            Debug.Assert(input.Role.Id != null, "input.Role.Id != null");
            var role = await _roleManager.GetRoleByIdAsync(input.Role.Id.Value);

            _mapper.Map(input.Role, role);

            CheckErrors(await _roleManager.UpdateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Roles_Delete)]
        public async Task DeleteRole(EntityDto<int> input)
        {
            var role = await _roleManager.FindByIdAsync(input.Id.ToString());
            var users = await _userManager.GetUsersInRoleAsync(role.NormalizedName);

            foreach (var user in users)
            {
                CheckErrors(await _userManager.RemoveFromRoleAsync(user, role.NormalizedName));
            }

            CheckErrors(await _roleManager.DeleteAsync(role));
        }

        public Task<ListResultDto<PermissionDto>> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();

            return Task.FromResult(new ListResultDto<PermissionDto>(
                ObjectMapper.Map<List<PermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList()
            ));
        }

        /// <summary>
        /// 获取用户过滤查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private IQueryable<Role> GetRolesFilteredQuery(IGetRolesInput input)
        {
            return _roleRepository.GetAll();
        }
    }
}