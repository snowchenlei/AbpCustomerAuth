using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhn.Template.Authorization.MenuItems.Dto;

namespace Zhn.Template.Web.Models.MenuItems
{
    public class MenuItemListViewModel
    {
        public IReadOnlyList<MenuItemListDto> MenuItems { get; set; }
    }
}
