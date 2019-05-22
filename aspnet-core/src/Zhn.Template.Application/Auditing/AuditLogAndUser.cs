using System;
using System.Collections.Generic;
using System.Text;
using Abp.Auditing;
using Zhn.Template.Authorization.Users;

namespace Zhn.Template.Auditing
{
    /// <summary>
    /// A helper class to store an <see cref="AuditLog"/> and a <see cref="User"/> object.
    /// </summary>
    public class AuditLogAndUser
    {
        public AuditLog AuditLog { get; set; }

        public User User { get; set; }
    }
}
