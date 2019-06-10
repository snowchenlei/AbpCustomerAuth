using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Snow.Template.Authorization.MenuItems.Dto;

namespace Snow.Template.Web.Models.MenuItems
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

