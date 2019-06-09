using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Application.Services.Dto;

namespace Zhn.Template.Parameters.Dto
{
    public class ParameterEditDto : NullableIdDto<Guid>
    {
        [StringLength(50)]
        public string Value { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int Sort { get; set; }

        public Guid ParameterTypeId { get; set; }
    }
}