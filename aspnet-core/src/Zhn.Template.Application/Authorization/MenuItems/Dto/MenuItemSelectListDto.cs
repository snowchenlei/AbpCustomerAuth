﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Zhn.Template.Authorization.MenuItems.Dto
{
    public class MenuItemSelectListDto : MenuItemListDto
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
