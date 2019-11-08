using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace Snow.Template.Authorization.MultiTenancy.Dto
{
    public class GetTenantsInput : PagedAndSortedResultRequestDto, IShouldNormalize
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