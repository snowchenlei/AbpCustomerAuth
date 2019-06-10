using System;
using System.Collections.Generic;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using FluentValidation;

namespace Snow.Template
{
    public class TemplateValidator<T> :AbstractValidator<T>
    {
        protected string L(string name, params object[] args)
        {
            return LocalizationHelper.GetSource(TemplateConsts.LocalizationSourceName).GetString(name, args);
        }
    }
}

