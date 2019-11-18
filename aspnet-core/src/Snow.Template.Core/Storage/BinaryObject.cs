using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp;
using Abp.Domain.Entities;

namespace Snow.Template.Storage
{
    [Table("AppBinaryObjects")]
    public class BinaryObject : Entity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        [Required]
        [MaxLength(TemplateConsts.MaxUrlLength)]
        public virtual string FilePath { get; set; }

        public BinaryObject()
        {
            Id = SequentialGuidGenerator.Instance.Create();
        }

        public BinaryObject(int? tenantId, string filePath)
            : this()
        {
            TenantId = tenantId;
            FilePath = filePath;
        }
    }
}