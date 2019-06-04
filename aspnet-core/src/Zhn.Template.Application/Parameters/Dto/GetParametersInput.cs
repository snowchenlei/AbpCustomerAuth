using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Validation;
using Zhn.Template.Dto;

namespace Zhn.Template.Parameters.Dto
{
    public class GetParametersInput : PagedAndSortedInputDto, IShouldNormalize
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