using System;
using System.Collections.Generic;
using System.Text;
using Abp.Dependency;

namespace Zhn.Template.Storage
{
    public interface ITempFileCacheManager : ITransientDependency
    {
        void SetFile(string token, byte[] content);

        byte[] GetFile(string token);
    }
}
