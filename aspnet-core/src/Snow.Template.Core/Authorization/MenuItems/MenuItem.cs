using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;

namespace Snow.Template.Authorization.MenuItems
{
    public class MenuItem : CreationAuditedEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        [CanBeNull]
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

        /// <summary>
        /// 父菜单
        /// </summary>
        [CanBeNull]
        public MenuItem Parent { get; set; }
    }
}
