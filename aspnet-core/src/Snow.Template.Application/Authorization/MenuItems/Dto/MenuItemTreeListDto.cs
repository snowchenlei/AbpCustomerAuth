using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.Template.Authorization.MenuItems.Dto
{
    public class MenuItemTreeListDto
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
    }
}
