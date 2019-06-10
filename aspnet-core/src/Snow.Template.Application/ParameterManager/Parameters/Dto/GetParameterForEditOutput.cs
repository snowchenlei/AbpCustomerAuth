using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.Template.Parameters.Dto
{
    public class GetParameterForEditOutput
    {
        public ParameterEditDto Parameter { get; set; }

        public List<ParameterTypeSelectListDto> ParameterTypes { get; set; }

    }
}
