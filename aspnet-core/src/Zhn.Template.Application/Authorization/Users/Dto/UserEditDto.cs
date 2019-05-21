using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Localization;
using Zhn.Template.Localization;

namespace Zhn.Template.Authorization.Users.Dto
{
    public partial class UserEditDto
    {
        /// <summary>
        /// Set null to create a new user. Set user's Id to update a user
        /// </summary>
        public long? Id { get; set; }

        [TemplateDisplayName("Name")]
        public string Name { get; set; }

        [TemplateDisplayName("Surname")]
        public string Surname { get; set; }

        [TemplateDisplayName("UserName")]
        public string UserName { get; set; }

        [TemplateDisplayName("EmailAddress")]
        public string EmailAddress { get; set; }

        [TemplateDisplayName("PhoneNumber")]
        public string PhoneNumber { get; set; }

        
        [TemplateDisplayName("Password")]
        [DisableAuditing]
        public string Password { get; set; }

        [TemplateDisplayName("IsActive")]
        public bool IsActive { get; set; }

        [TemplateDisplayName("ShouldChangePasswordOnNextLogin")]
        public bool ShouldChangePasswordOnNextLogin { get; set; }

        [TemplateDisplayName("IsTwoFactorEnabled")]
        public virtual bool IsTwoFactorEnabled { get; set; }

        [TemplateDisplayName("IsLockoutEnabled")]
        public virtual bool IsLockoutEnabled { get; set; }
    }
}