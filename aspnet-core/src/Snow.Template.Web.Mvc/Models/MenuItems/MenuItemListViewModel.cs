using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snow.Template.Authorization.MenuItems.Dto;

namespace Snow.Template.Web.Models.MenuItems
{
    public class MenuItemListViewModel
    {
        public IReadOnlyList<MenuItemListDto> MenuItems { get; set; }
    }
}

