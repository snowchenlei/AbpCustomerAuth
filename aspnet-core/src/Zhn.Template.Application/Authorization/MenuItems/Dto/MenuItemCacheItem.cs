using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Zhn.Template.Authorization.MenuItems.Dto
{
    public class MenuItemCacheItem : EntityDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        public int? ParentId { get; set; }
    }
}