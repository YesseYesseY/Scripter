using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharpCommon
{
    public static class Minhook
    {
        [DllImport("Scripter.dll", EntryPoint = "MH__Initialize")]
        public static extern int Initialize();
        [DllImport("Scripter.dll", EntryPoint = "MH__CreateHook")]
        public static unsafe extern int CreateHook(nint pTarget, nint pDetour, nint* ppOriginal);
        [DllImport("Scripter.dll", EntryPoint = "MH__EnableHook")]
        public static extern int EnableHook(nint pTarget);
    }
}
