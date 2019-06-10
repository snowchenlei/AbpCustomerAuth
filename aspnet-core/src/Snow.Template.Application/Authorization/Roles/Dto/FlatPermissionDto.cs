using Snow.Template.Dto;

namespace Snow.Template.Authorization.Roles.Dto
{
    public class FlatPermissionDto:ZtreeDto
    {
        public string ParentName { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        public bool IsSelected { get; set; }
    }
}
