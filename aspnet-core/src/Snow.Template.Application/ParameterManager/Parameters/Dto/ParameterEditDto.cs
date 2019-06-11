using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Application.Services.Dto;
using Snow.Template.Localization;

namespace Snow.Template.Parameters.Dto
{
    public class ParameterEditDto : NullableIdDto<Guid>
    {
        [TemplateDisplayName("Value")]
        public string Value { get; set; }

        [TemplateDisplayName("Description")]
        public string Description { get; set; }

        [TemplateDisplayName("Sort")]
        public int Sort { get; set; }

        [TemplateDisplayName("ParameterType")]
        public Guid ParameterTypeId { get; set; }
    }
}