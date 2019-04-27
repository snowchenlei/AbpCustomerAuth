using Abp.AspNetCore.Mvc.ViewComponents;

namespace Zhn.Template.Web.Views
{
    public abstract class TemplateViewComponent : AbpViewComponent
    {
        protected TemplateViewComponent()
        {
            LocalizationSourceName = TemplateConsts.LocalizationSourceName;
        }
    }
}


