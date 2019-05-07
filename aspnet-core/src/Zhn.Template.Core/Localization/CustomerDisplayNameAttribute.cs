using System;
using System.Collections.Generic;
using System.Text;
using Abp.Localization;

namespace Zhn.Template.Localization
{
    public class CustomerDisplayNameAttribute : AbpDisplayNameAttribute
    {
        public CustomerDisplayNameAttribute(string key) : base(TemplateConsts.LocalizationSourceName, key)
        {
        }
    }
}