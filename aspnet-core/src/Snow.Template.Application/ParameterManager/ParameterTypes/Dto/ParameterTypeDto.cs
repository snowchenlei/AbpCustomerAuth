using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace Snow.Template.ParameterManager.ParameterTypes.Dto
{
    public class ParameterTypeDto : Entity<Guid>
    {
        public string Text { get; set; }
    }
}