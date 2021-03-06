﻿using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Roles;
using Snow.Template.Authorization.Users;

namespace Snow.Template.Authorization.Roles
{
    public class Role : AbpRole<User>
    {
        public const int MaxDescriptionLength = 500;
        public Role()
        {
        }

        public Role(int? tenantId, string displayName)
            : base(tenantId, displayName)
        {
        }

        public Role(int? tenantId, string name, string displayName)
            : base(tenantId, name, displayName)
        {
        }

        public string Description { get; set; }
    }
}



