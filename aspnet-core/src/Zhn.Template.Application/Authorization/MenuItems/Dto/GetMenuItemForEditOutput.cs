﻿using System.Collections.Generic;

namespace Zhn.Template.Authorization.MenuItems.Dto
{
    public class GetMenuItemForEditOutput
    {
        public MenuItemEditDto MenuItem { get; set; }

        public List<MenuItemSelectListDto> MenuItems { get; set; }
    }
}

