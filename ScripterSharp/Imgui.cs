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
        public static extern void InitGui(Action guifunc, Win32.WndProc proc);

        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowDemoWindow@ImGui@@YAXPEA_N@Z")]
        public static extern void ShowDemoWindow(ref bool show);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplDX9_NewFrame@@YAXXZ")]
        public static extern void ImplDX9_NewFrame();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplWin32_NewFrame@@YAXXZ")]
        public static extern void ImplWin32_NewFrame();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EndFrame@ImGui@@YAXXZ")]
        public static extern void EndFrame();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?NewFrame@ImGui@@YAXXZ")]
        public static extern void NewFrame();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplWin32_WndProcHandler@@YA_JPEAUHWND__@@I_K_J@Z")]
        public static extern nint ImplWin32_WndProcHandler(nint hWnd, uint msg, nint wParam, nint lParam);

    }
}
