
using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Services;

namespace Zhn.Template
{
    public abstract class TemplateDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected TemplateDomainServiceBase()
        {
            LocalizationSourceName = TemplateConsts.LocalizationSourceName;
        }
    }
}
