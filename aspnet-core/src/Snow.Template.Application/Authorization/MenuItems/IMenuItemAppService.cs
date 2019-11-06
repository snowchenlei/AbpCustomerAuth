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

        Task<PagedResultDto<MenuItemListDto>> GetPagedAsync(GetMenuItemsInput input);

        Task<GetMenuItemForEditOutput> GetForEditAsync(NullableIdDto<int> input);

        Task CreateOrEditAsync(CreateOrUpdateMenuItemInput input);

        Task DeleteAsync(EntityDto input);
    }
}
