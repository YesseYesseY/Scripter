using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharp
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ProcessEventHookAttribute : Attribute
    {
        public string name;
        public ProcessEventHookAttribute(string FuncName)
        {
            name = FuncName;
        }
    }
}
