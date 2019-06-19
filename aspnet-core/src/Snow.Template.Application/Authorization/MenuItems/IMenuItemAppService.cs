using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Snow.Template.Authorization.MenuItems.Dto;

namespace Snow.Template.Authorization.MenuItems
{
    public interface IMenuItemAppService
    {
        //Task<List<MenuItemListDto>> GetList();

        Task<PagedResultDto<MenuItemListDto>> GetMenuItems(GetMenuItemsInput input);

        Task<List<MenuItemTreeListDto>> GetMenuItemTree();

        Task<GetMenuItemForEditOutput> GetMenuItemForEdit(NullableIdDto<int> input);

        Task CreateOrEditMenuItem(CreateOrUpdateMenuItemInput input);

        Task DeleteMenuItem(EntityDto input);
    }
}
