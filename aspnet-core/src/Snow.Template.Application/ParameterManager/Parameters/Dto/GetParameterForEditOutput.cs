using System;
using System.Collections.Generic;
using System.Text;
using Snow.Template.ParameterManager.ParameterTypes.Dto;

namespace Snow.Template.Parameters.Dto
{
    public class GetParameterForEditOutput
    {
        public ParameterEditDto Parameter { get; set; }

        public List<ParameterTypeSelectListDto> ParameterTypes { get; set; }
    }
}