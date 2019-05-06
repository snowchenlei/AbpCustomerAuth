using Abp.Localization;
using Abp.Runtime.Validation;
using Castle.DynamicProxy;

namespace Zhn.Template.Interceptor
{
    public class MvcActionInvocationValidatorInterceptor : IInterceptor
    {
        private readonly ILocalizationManager _localizationManager;

        public MvcActionInvocationValidatorInterceptor(ILocalizationManager localizationManager)
        {
            this._localizationManager = localizationManager;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.Method.Name;

            if (method != "ThrowValidationError")
            {
                invocation.Proceed();
                return;
            }

            try
            {
                invocation.Proceed();
            }
            catch (AbpValidationException e)
            {
                foreach (var validationResult in e.ValidationErrors)
                {
                    if (!validationResult.ErrorMessage.Contains("#"))
                    {
                        continue;
                    }

                    var errorStrings = validationResult.ErrorMessage.Split("#");
                    if (errorStrings.Length < 2)
                    {
                        continue;
                    }

                    if (errorStrings[0] != "ABP")
                    {
                        continue;
                    }

                    var key = errorStrings[1];
                    validationResult.ErrorMessage = this._localizationManager.GetString(TemplateConsts.LocalizationSourceName, key);
                }
                throw;
            }
        }
    }
}