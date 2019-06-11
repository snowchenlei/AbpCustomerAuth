using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Application.Services.Dto;
using Snow.Template.Localization;

namespace Snow.Template.ParameterManager.ParameterTypes.Dto
{
    public class ParameterTypeEditDto : NullableIdDto<Guid>
    {
        [TemplateDisplayName("Code")]
        public string Code { get; set; }

        [TemplateDisplayName("Name")]
        public string Name { get; set; }
    }
}