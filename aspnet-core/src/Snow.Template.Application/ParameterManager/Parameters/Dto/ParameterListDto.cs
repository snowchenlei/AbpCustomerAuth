using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Snow.Template.Parameters.Dto
{
    public class ParameterListDto : EntityDto<Guid>
    {
        public string Value { get; set; }

        public string TypeName { get; set; }
    }
}
