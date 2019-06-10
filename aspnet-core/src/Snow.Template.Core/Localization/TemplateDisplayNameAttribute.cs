using System;
using System.Collections.Generic;
using System.Text;
using Abp.Localization;

namespace Snow.Template.Localization
{
    public class TemplateDisplayNameAttribute : AbpDisplayNameAttribute
    {
        public TemplateDisplayNameAttribute(string key) : base(TemplateConsts.LocalizationSourceName, key)
        {
        }
    }
}
