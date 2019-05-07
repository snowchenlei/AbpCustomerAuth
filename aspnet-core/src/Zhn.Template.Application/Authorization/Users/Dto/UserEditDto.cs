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

        [CustomerDisplayName("Name")]
        public string Name { get; set; }

        [CustomerDisplayName("Surname")]
        public string Surname { get; set; }

        [CustomerDisplayName("UserName")]
        public string UserName { get; set; }

        [CustomerDisplayName("EmailAddress")]
        public string EmailAddress { get; set; }

        [CustomerDisplayName("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [CustomerDisplayName("Password")]

        // Not used "Required" attribute since empty value is used to 'not change password'
        //[DisableAuditing]
        public string Password { get; set; }

        [CustomerDisplayName("IsActive")]
        public bool IsActive { get; set; }

        [CustomerDisplayName("ShouldChangePasswordOnNextLogin")]
        public bool ShouldChangePasswordOnNextLogin { get; set; }

        [CustomerDisplayName("IsTwoFactorEnabled")]
        public virtual bool IsTwoFactorEnabled { get; set; }

        [CustomerDisplayName("IsLockoutEnabled")]
        public virtual bool IsLockoutEnabled { get; set; }
    }
}