using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities.Caching;
using Snow.Template.Authorization.MenuItems.Dto;

namespace Snow.Template.Authorization.MenuItems.Caches
{
    public interface IMenuItemCache : IEntityCache<MenuItemCacheItem>
    {
    }
}
