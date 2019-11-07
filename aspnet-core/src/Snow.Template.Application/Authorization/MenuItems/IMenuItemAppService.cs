using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Snow.Template.Authorization.MenuItems.Dto;

namespace Snow.Template.Authorization.MenuItems
{
    public interface IMenuItemAppService : IApplicationService
    {
        //Task<List<MenuItemListDto>> GetList();

        Task<PagedResultDto<MenuItemListDto>> GetPagedAsync(GetMenuItemsInput input);

        Task<List<MenuItemTreeListDto>> GetMenuItemTreeAsync();

        Task<GetMenuItemForEditOutput> GetForEditAsync(NullableIdDto<int> input, int? parentId);

        Task CreateOrEditAsync(CreateOrUpdateMenuItemInput input);

        Task DeleteAsync(EntityDto input);
    }
}