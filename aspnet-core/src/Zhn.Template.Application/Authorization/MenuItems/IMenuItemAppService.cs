﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Zhn.Template.Authorization.MenuItems.Dto;

namespace Zhn.Template.Authorization.MenuItems
{
    public interface IMenuItemAppService
    {
        Task<List<MenuItemListDto>> GetList();

        Task<PagedResultDto<MenuItemListDto>> GetMenuItems(GetMenuItemsInput input);

        Task<GetMenuItemForEditOutput> GetMenuItemForEdit(NullableIdDto<int> input);

        Task CreateOrUpdate(CreateOrUpdateMenuItemInput input);

        Task Delete(EntityDto input);
    }
}
