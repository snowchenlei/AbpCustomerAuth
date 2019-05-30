using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Entities.Caching;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Microsoft.EntityFrameworkCore;
using Zhn.Template.Authorization.MenuItems.Dto;

namespace Zhn.Template.Authorization.MenuItems.Caches
{
    public class MenuItemCache : EntityCache<MenuItem, MenuItemCacheItem>, IMenuItemCache, ITransientDependency
    {
        public MenuItemCache(ICacheManager cacheManager, IRepository<MenuItem, int> repository, string cacheName = null) : base(cacheManager, repository, cacheName)
        {
        }

        protected override MenuItem GetEntityFromDataSource(int id)
        {
            return Repository.GetAllIncluding(m => m.Parent).FirstOrDefault(m => m.Id == id);
        }

        protected override Task<MenuItem> GetEntityFromDataSourceAsync(int id)
        {
            return Repository.GetAllIncluding(m => m.Parent).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}