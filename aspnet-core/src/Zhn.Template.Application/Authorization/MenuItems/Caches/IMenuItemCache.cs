using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities.Caching;
using Zhn.Template.Authorization.MenuItems.Dto;

namespace Zhn.Template.Authorization.MenuItems.Caches
{
    public interface IMenuItemCache : IEntityCache<MenuItemCacheItem>
    {
    }
}