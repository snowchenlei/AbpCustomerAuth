using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Zhn.Template.Authorization;
using Zhn.Template.Authorization.MenuItems;
using Zhn.Template.Authorization.MenuItems.Dto;

namespace Zhn.Template.Authorization.MenuItems
{
    [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems)]
    public class MenuItemAppService : TemplateAppServiceBase, IMenuItemAppService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<MenuItem> _menuItemRepository;

        public MenuItemAppService(IRepository<MenuItem> menuItemRepository, IMapper mapper)
        {
            _menuItemRepository = menuItemRepository;
            _mapper = mapper;
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems)]
        public async Task<List<MenuItemListDto>> GetList()
        {
            var query = _menuItemRepository.GetAll();
            var menuItems = await query
                .AsNoTracking()
                .ToListAsync();

            var menuItemListDtos = ObjectMapper.Map<List<MenuItemListDto>>(menuItems);

            return new List<MenuItemListDto>(menuItemListDtos);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems)]
        public async Task<PagedResultDto<MenuItemListDto>> GetMenuItems(GetMenuItemsInput input)
        {
            var query = _menuItemRepository.GetAll();
            int menuItemCount = await query.CountAsync();
            List<MenuItem> menuItems = await query
                .Include(m=>m.Parent)
                .AsNoTracking()
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            List<MenuItemListDto> menuItemListDtos = ObjectMapper.Map<List<MenuItemListDto>>(menuItems);

            return new PagedResultDto<MenuItemListDto>(
                menuItemCount,
                menuItemListDtos
            );
        }


        [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems_Create,
            PermissionNames.Pages_Administration_MenuItems_Edit)]
        public async Task<GetMenuItemForEditOutput> GetMenuItemForEdit(NullableIdDto<int> input)
        {
            GetMenuItemForEditOutput output = new GetMenuItemForEditOutput();
            List<MenuItem> menuItems = await _menuItemRepository.GetAllListAsync();
            output.MenuItems = _mapper.Map<List<MenuItemSelectListDto>>(menuItems);
            if (input.Id.HasValue)
            {

                MenuItem menuItem = await _menuItemRepository.FirstOrDefaultAsync(m => m.Id == input.Id.Value);
                if (menuItem == null)
                {
                    throw new UserFriendlyException("菜单不存在");
                }
                foreach (MenuItemSelectListDto item in output.MenuItems)
                {
                    if (item.Id == menuItem.Parent?.Id)
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
            }

            return output;
        }
        [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems_Create,
            PermissionNames.Pages_Administration_MenuItems_Edit)]
        public async Task CreateOrEditMenuItem(CreateOrUpdateMenuItemInput input)
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
            MenuItem menuItem = await GetAsync(input.MenuItem.Id.Value);
            menuItem = _mapper.Map(input.MenuItem, menuItem);
            if (input.MenuItem.ParentId.HasValue && menuItem.Parent?.Id != input.MenuItem.ParentId)
            {
                menuItem.Parent = _menuItemRepository.FirstOrDefault(m => m.Id == input.MenuItem.ParentId.Value);
            }

            await _menuItemRepository.UpdateAsync(menuItem);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_MenuItems_Delete)]
        public async Task DeleteMenuItem(EntityDto input)
        {
            MenuItem menuItem = await GetAsync(input.Id);
            await _menuItemRepository.DeleteAsync(menuItem);
        }

        private async Task<MenuItem> GetAsync(int id)
        {
            MenuItem menuItem = await _menuItemRepository.FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null)
            {
                throw new UserFriendlyException(L("MenuItemNotFound"));
            }

            return menuItem;
        }
    }
}
