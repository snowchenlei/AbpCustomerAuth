using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Snow.Template.ParameterManager.ParameterTypes.Dto
{
    public class ParameterTypeListDto : EntityDto<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}