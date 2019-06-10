using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.Template.Auditing
{
    public interface INamespaceStripper
    {
        string StripNameSpace(string serviceName);
    }
}

