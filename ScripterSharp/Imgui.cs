using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharp
{
    public static class ImGui
    {
        [DllImport("Scripter.dll", CharSet = CharSet.Unicode)]
        public static extern void InitGui(Action guifunc);

        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowDemoWindow@ImGui@@YAXPEA_N@Z")]
        public static extern void ShowDemoWindow(ref bool show);
    }
}
