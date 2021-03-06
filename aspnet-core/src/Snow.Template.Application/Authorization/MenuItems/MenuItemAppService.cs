﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.UI;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Snow.Template.Authorization;
using Snow.Template.Authorization.MenuItems;
using Snow.Template.Authorization.MenuItems.Caches;
using Snow.Template.Authorization.MenuItems.Dto;
using Snow.Template.Authorization.Users;

namespace Snow.Template.Authorization.MenuItems
{
    [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems)]
    public class MenuItemAppService : TemplateAppServiceBase, IMenuItemAppService
    {
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;
        private readonly IMenuItemCache _menuItemCache;

        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;

        private readonly IMenuItemManager _menuItemManager;
        private readonly IRepository<MenuItem> _menuItemRepository;

        public MenuItemAppService(IRepository<MenuItem> menuItemRepository,
                                  IMapper mapper,
                                  IAbpSession abpSession,
                                  UserManager userManager,
                                  ICacheManager cacheManager,
                                  IMenuItemCache menuItemCache,
                                  IMenuItemManager menuItemManager)
        {
            _menuItemRepository = menuItemRepository;
            _mapper = mapper;
            _abpSession = abpSession;
            _userManager = userManager;
            _cacheManager = cacheManager;
            _menuItemCache = menuItemCache;
            _menuItemManager = menuItemManager;
        }

        //[AbpAuthorize(PermissionNames.Pages_Administration_MenuItems)]
        //public async Task<List<MenuItemListDto>> GetList()
        //{
        //    //var query = _menuItemRepository.GetAll();
        //    //var menuItems = await query
        //    //    .AsNoTracking()
        //    //    .ToListAsync();
        //    List<MenuItem> menuItems = await _menuItemManager.GetMenuItemsAsync();

        // var menuItemListDtos = ObjectMapper.Map<List<MenuItemListDto>>(menuItems);

        //    return new List<MenuItemListDto>(menuItemListDtos);
        //}

        [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems)]
        public async Task<PagedResultDto<MenuItemListDto>> GetPagedAsync(GetMenuItemsInput input)
        {
            // TODO:过滤不需要的菜单，如未开启多租户属于Host的菜单
            var query = GetMenuItemsFilteredQuery(input);
            int menuItemCount = await query.CountAsync();
            List<MenuItem> menuItems = await query
                .Include(m => m.Parent)
                .AsNoTracking()
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            List<MenuItemListDto> menuItemListDtos = _mapper.Map<List<MenuItemListDto>>(menuItems);

            return new PagedResultDto<MenuItemListDto>(
                menuItemCount,
                menuItemListDtos
            );
        }

        public async Task<List<MenuItemTreeListDto>> GetMenuItemTreeAsync()
        {
            List<MenuItem> menuItems =
                await _menuItemRepository.GetAllIncluding(a => a.Parent).AsNoTracking().ToListAsync();
            return menuItems.Select(m => new MenuItemTreeListDto
            {
                Id = m.Id,
                ParentId = m.Parent == null ? 0 : m.Parent.Id,
                Name = m.Name
            }).ToList();
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems_Create,
            PermissionNames.Pages_Administration_MenuItems_Edit)]
        public async Task<GetMenuItemForEditOutput> GetForEditAsync(NullableIdDto<int> input, int? parentId)
        {
            GetMenuItemForEditOutput output = new GetMenuItemForEditOutput();
            List<MenuItem> menuItems = await _menuItemManager.GetMenuItemsAsync();
            output.MenuItems = _mapper.Map<List<MenuItemSelectListDto>>(menuItems);
            if (input.Id.HasValue)
            {
                MenuItemCacheItem menuItem = await _menuItemCache.GetAsync(input.Id.Value) ??
                                             throw new UserFriendlyException("菜单不存在");
                foreach (MenuItemSelectListDto item in output.MenuItems)
                {
                    if (item.Id == menuItem.ParentId)
                    {
                        item.IsSelected = true;
                        break;
                    }
                }
                output.MenuItem = _mapper.Map<MenuItemEditDto>(menuItem);
            }
            else
            {
                output.MenuItem = new MenuItemEditDto();
                if (parentId.HasValue)
                {
                    output.MenuItem.ParentId = parentId;
                }
            }

            return output;
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems_Create,
            PermissionNames.Pages_Administration_MenuItems_Edit)]
        public async Task CreateOrEditAsync(CreateOrUpdateMenuItemInput input)
        {
            if (input.MenuItem.Id.HasValue)
            {
                await UpdateAsync(input);
            }
            else
            {
                await CreateAsync(input);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems_Create)]
        protected virtual async Task CreateAsync(CreateOrUpdateMenuItemInput input)
        {
            MenuItem menuItem = _mapper.Map<MenuItem>(input.MenuItem);
            if (input.MenuItem.ParentId.HasValue)
            {
                menuItem.Parent = _menuItemRepository.FirstOrDefault(m => m.Id == input.MenuItem.ParentId);
            }
            await _menuItemRepository.InsertAsync(menuItem);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems_Edit)]
        private async Task UpdateAsync(CreateOrUpdateMenuItemInput input)
        {
            Debug.Assert(input.MenuItem.Id != null, "input.MenuItem.Id != null");
            MenuItem menuItem = await _menuItemRepository.GetAsync(input.MenuItem.Id.Value);
            menuItem = _mapper.Map(input.MenuItem, menuItem);
            if (input.MenuItem.ParentId.HasValue && menuItem.Parent?.Id != input.MenuItem.ParentId)
            {
                menuItem.Parent = _menuItemRepository.FirstOrDefault(m => m.Id == input.MenuItem.ParentId.Value);
            }

            await _menuItemRepository.UpdateAsync(menuItem);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems_Delete)]
        public async Task DeleteAsync(EntityDto input)
        {
            await _menuItemRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems_BatchDelete)]
        public async Task BatchDeleteAsync(List<int> ids)
        {
            await _menuItemRepository.DeleteAsync(m => ids.Contains(m.Id));
        }

        /// <summary>
        /// 获取菜单过滤查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private IQueryable<MenuItem> GetMenuItemsFilteredQuery(IGetMenuItemsInput input)
        {
            return _menuItemRepository.GetAll()
                .WhereIf(input.ParentId.HasValue, m => m.Parent.Id == input.ParentId.Value);
        }
    }
}