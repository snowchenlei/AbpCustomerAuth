using System;
using System.Collections.Generic;
using System.Text;

namespace Zhn.Template.Auditing
{
    public interface INamespaceStripper
    {
        string StripNameSpace(string serviceName);
    }
}
