using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Zhn.Template.Auditing.Dto
{
    public class EntityPropertyChangeDto : EntityDto<long>
    {
        public long EntityChangeId { get; set; }

        public string NewValue { get; set; }

        public string OriginalValue { get; set; }

        public string PropertyName { get; set; }

        public string PropertyTypeFullName { get; set; }

        public int? TenantId { get; set; }
    }
}
