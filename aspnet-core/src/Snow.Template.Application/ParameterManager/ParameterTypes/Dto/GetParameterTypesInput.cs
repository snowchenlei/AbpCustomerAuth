using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Validation;
using Snow.Template.Dto;

namespace Snow.Template.ParameterManager.ParameterTypes.Dto
{
    public class GetParameterTypesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}
