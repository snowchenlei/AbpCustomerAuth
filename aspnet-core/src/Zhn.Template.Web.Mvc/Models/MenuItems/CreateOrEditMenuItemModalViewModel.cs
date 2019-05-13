using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Zhn.Template.Authorization.MenuItems.Dto;

namespace Zhn.Template.Web.Models.MenuItems
{
    [AutoMapFrom(typeof(GetMenuItemForEditOutput))]
    public class CreateOrEditMenuItemModalViewModel:GetMenuItemForEditOutput
    {
        public bool IsEditMode
        {
            get { return MenuItem.Id.HasValue; }
        }

        public CreateOrEditMenuItemModalViewModel(GetMenuItemForEditOutput output)
        {
            output.MapTo(this);
        }
    }
}
