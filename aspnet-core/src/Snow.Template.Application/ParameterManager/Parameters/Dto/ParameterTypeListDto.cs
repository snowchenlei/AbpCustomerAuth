using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace Snow.Template.Parameters.Dto
{
    public class ParameterTypeListDto : Entity<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
