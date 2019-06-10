using System;
using System.Collections.Generic;
using System.Text;
using Abp.Dependency;
using Castle.Core;
using Castle.MicroKernel;

namespace Snow.Template.Interceptor
{
    public static class ValidationInterceptorRegister
    {
        public static void Initialize(IIocManager iocManager)
        {
            iocManager.IocContainer.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        private static void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            var name = handler.ComponentModel.Implementation.Name;

            if (name == "MvcActionInvocationValidator")
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(MvcActionInvocationValidatorInterceptor)));
            }
        }
    }
}
