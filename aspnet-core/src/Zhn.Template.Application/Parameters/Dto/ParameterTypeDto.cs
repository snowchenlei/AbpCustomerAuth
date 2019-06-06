using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace Zhn.Template.Parameters.Dto
{
    public class ParameterTypeDto : Entity<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}