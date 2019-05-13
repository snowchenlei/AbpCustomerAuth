using Abp.Application.Services.Dto;

namespace Zhn.Template.Authorization.MenuItems.Dto
{
    public class MenuItemEditDto : NullableIdDto
    {
        /// <summary>
        /// 父菜单
        /// </summary>
        public int ParentId { get; set; }

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

        public int Sort { get; set; }
    }
}
