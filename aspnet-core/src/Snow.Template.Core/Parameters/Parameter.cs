using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Domain.Entities;

namespace Snow.Template.Parameters
{
    public class Parameter : Entity<Guid>
    {
        [StringLength(50)]
        public string Value { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int Sort { get; set; }

        public ParameterType ParameterType { get; set; }
    }
}
