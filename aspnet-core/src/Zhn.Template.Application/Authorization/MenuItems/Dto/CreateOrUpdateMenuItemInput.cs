using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Zhn.Template.Authorization.MenuItems.Dto
{
   public  class CreateOrUpdateMenuItemInput
    {
        [Required]
        public MenuItemEditDto MenuItem{ get; set; }
    }
}

