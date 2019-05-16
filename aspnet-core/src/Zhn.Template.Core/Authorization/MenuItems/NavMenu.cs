using System.Collections.Generic;

namespace Zhn.Template.Authorization.MenuItems
{
    public class NavMenu
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

        public List<NavMenu> Items { get; set; }
    }
}
