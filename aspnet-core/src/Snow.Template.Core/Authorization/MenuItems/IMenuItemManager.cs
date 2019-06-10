using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Navigation;
using Abp.Domain.Services;

namespace Snow.Template.Authorization.MenuItems
{
    public interface IMenuItemManager : IDomainService
    {
        Task<MenuItem> GetMenuItemAsync(int id);

        Task<List<MenuItem>> GetMenuItemsAsync();

        Task<UserMenu> GetMenuItemTree(UserIdentifier user);
    }
}
