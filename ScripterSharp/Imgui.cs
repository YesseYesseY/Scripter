using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharp
{
    public static class ImGui
    {
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
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateContext@ImGui@@YAPEAUImGuiContext@@PEAUImFontAtlas@@@Z")]
        public static extern nint CreateContext(nint shared_font_atlas = 0);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?StyleColorsDark@ImGui@@YAXPEAUImGuiStyle@@@Z")]
        public static extern void StyleColorsDark(nint dst = 0);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplWin32_Init@@YA_NPEAX@Z")]
        public static extern bool ImplWin32_Init(nint hwnd);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplDX9_Init@@YA_NPEAUIDirect3DDevice9@@@Z")]
        public static extern bool ImplDX9_Init(nint device);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Render@ImGui@@YAXXZ")]
        public static extern void Render();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetDrawData@ImGui@@YAPEAUImDrawData@@XZ")]
        public static extern nint GetDrawData();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplDX9_RenderDrawData@@YAXPEAUImDrawData@@@Z")]
        public static extern void ImplDX9_RenderDrawData(nint draw_data);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplDX9_Shutdown@@YAXXZ")]
        public static extern void ImplDX9_Shutdown();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplWin32_Shutdown@@YAXXZ")]
        public static extern void ImplWin32_Shutdown();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?DestroyContext@ImGui@@YAXPEAUImGuiContext@@@Z")]
        public static extern void DestroyContext(nint ctx = 0);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplDX9_InvalidateDeviceObjects@@YAXXZ")]
        public static extern void ImplDX9_InvalidateDeviceObjects();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplDX9_CreateDeviceObjects@@YA_NXZ")]
        public static extern bool ImplDX9_CreateDeviceObjects();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Begin@ImGui@@YA_NPEBDPEA_NH@Z")]
        public static extern unsafe bool Begin([MarshalAs(UnmanagedType.LPStr)]string name, bool* p_open = null, int flags = 0);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?End@ImGui@@YAXXZ")]
        public static extern unsafe void End();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Text@ImGui@@YAXPEBDZZ")]
        public static extern unsafe void Text([MarshalAs(UnmanagedType.LPStr)] string str);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SmallButton@ImGui@@YA_NPEBD@Z")]
        public static extern unsafe bool Button([MarshalAs(UnmanagedType.LPStr)] string label, long size = 0); // TODO: Change to real button and add ImVec

    }
}
