using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;

namespace Zhn.Template.Web.Views
{
    public abstract class TemplateRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected TemplateRazorPage()
        {
            LocalizationSourceName = TemplateConsts.LocalizationSourceName;
        }
    }
}


