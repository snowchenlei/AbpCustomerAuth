using System;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace Zhn.Template.Authorization.MenuItems.Dto
{
    public class GetMenuItemsInput: PagedAndSortedResultRequestDto,IShouldNormalize
    {
        public void Normalize()
        {
            if (String.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}

