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
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Snow.Template.Authorization;
using Snow.Template.Authorization.Roles;
using Snow.Template.Authorization.Users;
using Snow.Template.Editions;
using Microsoft.AspNetCore.Identity;
using Snow.Template.Authorization.MultiTenancy.Dto;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AutoMapper;
using Abp.UI;
using System.Diagnostics;

namespace Snow.Template.Authorization.MultiTenancy
{
    /// <summary>
    /// 租户管理
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_Administration_Tenants)]
    public class TenantAppService : TemplateAppServiceBase, ITenantAppService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Tenant, int> _tenantRepository;
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="repository"></param>
        /// <param name="tenantManager"></param>
        /// <param name="editionManager"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="abpZeroDbMigrator"></param>
        public TenantAppService(
            IMapper mapper,
            IRepository<Tenant, int> repository,
            TenantManager tenantManager,
            EditionManager editionManager,
            UserManager userManager,
            RoleManager roleManager,
            IAbpZeroDbMigrator abpZeroDbMigrator)
        {
            _mapper = mapper;
            _tenantRepository = repository;
            _tenantManager = tenantManager;
            _editionManager = editionManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _abpZeroDbMigrator = abpZeroDbMigrator;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Tenants)]
        public async Task<PagedResultDto<TenantListDto>> GetPagedAsync(GetTenantsInput input)
        {
            var query = GetTenantsFilteredQuery(input);
            int tenantCount = await query.CountAsync();
            List<Tenant> tenants = await query
                .AsNoTracking()
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            List<TenantListDto> tenantListDtos = _mapper.Map<List<TenantListDto>>(tenants);

            return new PagedResultDto<TenantListDto>(
                tenantCount,
                tenantListDtos
            );
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Tenants_Create,
            PermissionNames.Pages_Administration_Tenants_Edit)]
        public async Task<TenantEditDto> GetForEditAsync(NullableIdDto<int> input)
        {
            Tenant tenant = await _tenantRepository.FirstOrDefaultAsync(input.Id.Value);
            return _mapper.Map<TenantEditDto>(tenant);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Tenants_Create)]
        public async Task CreateAsync(CreateTenantInput input)
        {
            // Create tenant
            var tenant = ObjectMapper.Map<Tenant>(input);
            tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty()
                ? null
                : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);

            var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
            if (defaultEdition != null)
            {
                tenant.EditionId = defaultEdition.Id;
            }

            await _tenantManager.CreateAsync(tenant);
            await CurrentUnitOfWork.SaveChangesAsync(); // To get new tenant's id.

            // Create tenant database
            _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

            // We are working entities of new tenant, so changing tenant filter
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                // Create static roles for new tenant
                CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

                await CurrentUnitOfWork.SaveChangesAsync(); // To get static role ids

                // Grant all permissions to admin role
                var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                await _roleManager.GrantAllPermissionsAsync(adminRole);

                // Create admin user for the tenant
                var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress);
                await _userManager.InitializeOptionsAsync(tenant.Id);
                CheckErrors(await _userManager.CreateAsync(adminUser, User.DefaultPassword));
                await CurrentUnitOfWork.SaveChangesAsync(); // To get admin user's id

                // Assign admin user to role!
                CheckErrors(await _userManager.AddToRoleAsync(adminUser, adminRole.Name));
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Tenants_Edit)]
        public async Task UpdateAsync(TenantEditDto input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            Tenant tenant = await _tenantRepository.GetAsync(input.Id.Value)
                ?? throw new UserFriendlyException("租户不存在");
            _mapper.Map(input, tenant);
            await _tenantRepository.UpdateAsync(tenant);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Tenants_Delete)]
        public async Task DeleteAsync(EntityDto input)
        {
            var tenant = await _tenantManager.GetByIdAsync(input.Id);
            await _tenantManager.DeleteAsync(tenant);
        }

        /// <summary>
        /// 过滤条件
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns></returns>
        private IQueryable<Tenant> GetTenantsFilteredQuery(GetTenantsInput input)
        {
            return _tenantRepository.GetAll()
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive)
            ;
        }
    }
}