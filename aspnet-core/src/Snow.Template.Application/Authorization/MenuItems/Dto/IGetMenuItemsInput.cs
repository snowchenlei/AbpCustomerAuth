using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.Template.Authorization.MenuItems.Dto
{
    public interface IGetMenuItemsInput
    {
        int? ParentId { get; set; }

    }
}
