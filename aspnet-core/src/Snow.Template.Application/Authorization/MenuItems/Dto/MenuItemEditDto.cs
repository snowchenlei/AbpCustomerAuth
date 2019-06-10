using Abp.Application.Services.Dto;
using Snow.Template.Localization;

namespace Snow.Template.Authorization.MenuItems.Dto
{
    public class MenuItemEditDto : NullableIdDto
    {
        /// <summary>
        /// 父菜单
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [TemplateDisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        [TemplateDisplayName("PermissionName")]
        public string PermissionName { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [TemplateDisplayName("Icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        [TemplateDisplayName("Route")]
        public string Route { get; set; }

        [TemplateDisplayName("Sort")]
        public int Sort { get; set; }
    }
}
