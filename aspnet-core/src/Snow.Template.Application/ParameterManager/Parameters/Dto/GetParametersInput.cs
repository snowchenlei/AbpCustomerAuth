using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Validation;
using Snow.Template.Dto;

namespace Snow.Template.Parameters.Dto
{
    public class GetParametersInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Sort,Id";
            }
        }

        public Guid? ParameterTypeId { get; set; }
    }
}