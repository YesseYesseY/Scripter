using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static ScripterSharpCommon.ImGui;

namespace ScripterSharpCommon
{
    public static unsafe class ImGui
    {
        // NOTE: If you're adding to this, use byte instead of bool.
        // If you're returning ImVec2 use void for return and do "void Example(ImVec2* ret);", probably same for ImVec4

        #region Backend Stuff
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplDX9_NewFrame@@YAXXZ")]
        public static extern void ImplDX9_NewFrame();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplWin32_NewFrame@@YAXXZ")]
        public static extern void ImplWin32_NewFrame();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplWin32_WndProcHandler@@YA_JPEAUHWND__@@I_K_J@Z")]
        public static extern nint ImplWin32_WndProcHandler(nint hWnd, uint msg, nint wParam, nint lParam);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplWin32_Init@@YA_NPEAX@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ImplWin32_Init(nint hwnd);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplDX9_Init@@YA_NPEAUIDirect3DDevice9@@@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ImplDX9_Init(DXD9.LPDIRECT3DDEVICE9* device);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplDX9_RenderDrawData@@YAXPEAUImDrawData@@@Z")]
        public static extern void ImplDX9_RenderDrawData(nint draw_data);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplDX9_Shutdown@@YAXXZ")]
        public static extern void ImplDX9_Shutdown();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplWin32_Shutdown@@YAXXZ")]
        public static extern void ImplWin32_Shutdown();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplDX9_InvalidateDeviceObjects@@YAXXZ")]
        public static extern void ImplDX9_InvalidateDeviceObjects();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplDX9_CreateDeviceObjects@@YA_NXZ")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ImplDX9_CreateDeviceObjects();
        #endregion

        #region Context creation and access
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateContext@ImGui@@YAPEAUImGuiContext@@PEAUImFontAtlas@@@Z")]
        public static extern nint CreateContext(nint shared_font_atlas = 0);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?DestroyContext@ImGui@@YAXPEAUImGuiContext@@@Z")]
        public static extern void DestroyContext(nint ctx = 0);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetCurrentContext@ImGui@@YAPEAUImGuiContext@@XZ")]
        public static extern nint GetCurrentContext();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetCurrentContext@ImGui@@YAXPEAUImGuiContext@@@Z")]
        public static extern void SetCurrentContext(nint ctx);
        #endregion

        #region Main
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetIO@ImGui@@YAAEAUImGuiIO@@XZ")]
        public static extern ImGuiIO* GetIO();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetPlatformIO@ImGui@@YAAEAUImGuiPlatformIO@@XZ")]
        public static extern nint GetPlatformIO();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetStyle@ImGui@@YAAEAUImGuiStyle@@XZ")]
        public static extern ImGuiStyle* GetStyle();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?NewFrame@ImGui@@YAXXZ")]
        public static extern void NewFrame();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EndFrame@ImGui@@YAXXZ")]
        public static extern void EndFrame();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Render@ImGui@@YAXXZ")]
        public static extern void Render();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetDrawData@ImGui@@YAPEAUImDrawData@@XZ")]
        public static extern ImDrawData* GetDrawData();

        #endregion

        #region Demo, Debug, Information
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowDemoWindow@ImGui@@YAXPEA_N@Z")]
        private static extern void _ShowDemoWindow(byte* show = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowMetricsWindow@ImGui@@YAXPEA_N@Z")]
        private static extern void _ShowMetricsWindow(byte* p_open = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowDebugLogWindow@ImGui@@YAXPEA_N@Z")]
        private static extern void _ShowDebugLogWindow(byte* p_open = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowIDStackToolWindow@ImGui@@YAXPEA_N@Z")]
        private static extern void _ShowIDStackToolWindow(byte* p_open = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowAboutWindow@ImGui@@YAXPEA_N@Z")]
        private static extern void _ShowAboutWindow(byte* p_open = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowStyleEditor@ImGui@@YAXPEAUImGuiStyle@@@Z")]
        private static extern byte _ShowStyleEditor(ImGuiStyle* _ref = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowStyleSelector@ImGui@@YA_NPEBD@Z")]
        public static extern void ShowStyleSelector(string label);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowFontSelector@ImGui@@YAXPEBD@Z")]
        public static extern void ShowFontSelector(string label);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowUserGuide@ImGui@@YAXXZ")]
        public static extern void ShowUserGuide();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetVersion@ImGui@@YAPEBDXZ")]
        private static extern nint _GetVersion();

        private static string ImGuiVersionStr = "";
        public static string GetVersion()
        {
            if (string.IsNullOrEmpty(ImGuiVersionStr))
            {
                var _str = Marshal.PtrToStringAnsi(_GetVersion());
                if (_str is not null)
                    ImGuiVersionStr = _str;
            };

            return ImGuiVersionStr;
        }

        public static bool ShowStyleEditor(ImGuiStyle* _ref = null) => _ShowStyleEditor(_ref) != 0;

        public static void ShowDemoWindow(bool* show = null)
        {
            byte val = 1;
            if (show is not null && *show is false)
                val = 0;
            _ShowDemoWindow(show is null ? null : &val);
            if (show is not null)
                *show = val != 0;
        }

        public static void ShowMetricsWindow(bool* p_open = null)
        {
            byte val = 1;
            if (p_open is not null && *p_open is false)
                val = 0;
            _ShowMetricsWindow(p_open is null ? null : &val);
            if (p_open is not null)
                *p_open = val != 0;
        }
        public static void ShowDebugLogWindow(bool* p_open = null)
        {
            byte val = 1;
            if (p_open is not null && *p_open is false)
                val = 0;
            _ShowDebugLogWindow(p_open is null ? null : &val);
            if (p_open is not null)
                *p_open = val != 0;
        }
        public static void ShowIDStackToolWindow(bool* p_open = null)
        {
            byte val = 1;
            if (p_open is not null && *p_open is false)
                val = 0;
            _ShowIDStackToolWindow(p_open is null ? null : &val);
            if (p_open is not null)
                *p_open = val != 0;
        }
        public static void ShowAboutWindow(bool* p_open = null)
        {
            byte val = 1;
            if (p_open is not null && *p_open is false)
                val = 0;
            _ShowAboutWindow(p_open is null ? null : &val);
            if (p_open is not null)
                *p_open = val != 0;
        }

        #endregion

        #region Styles
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?StyleColorsDark@ImGui@@YAXPEAUImGuiStyle@@@Z")]
        public static extern void StyleColorsDark(ImGuiStyle* dst = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?StyleColorsLight@ImGui@@YAXPEAUImGuiStyle@@@Z")]
        public static extern void StyleColorsLight(ImGuiStyle* dst = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?StyleColorsClassic@ImGui@@YAXPEAUImGuiStyle@@@Z")]
        public static extern void StyleColorsClassic(ImGuiStyle* dst = null);
        #endregion

        #region Windows
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Begin@ImGui@@YA_NPEBDPEA_NH@Z")]
        private static extern byte _Begin(string name, byte* p_open = null, WindowFlags flags = WindowFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?End@ImGui@@YAXXZ")]
        public static extern void End();

        public static bool Begin(string name, bool* p_open = null, WindowFlags flags = WindowFlags.None)
        {
            byte v = 1;
            if (p_open is not null && *p_open == false)
                v = 0;
            var ret = _Begin(name, &v, flags) != 0;
            if (p_open is not null)
                *p_open = v != 0;
            return ret;
        }
        #endregion

        #region Child Windows
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?BeginChild@ImGui@@YA_NPEBDAEBUImVec2@@HH@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool BeginChild(string str_id, in ImVec2 size = default, ChildFlags child_flags = ChildFlags.None, WindowFlags window_flags = WindowFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?BeginChild@ImGui@@YA_NIAEBUImVec2@@HH@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool BeginChild(uint id, in ImVec2 size = default, ChildFlags child_flags = ChildFlags.None, WindowFlags window_flags = WindowFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EndChild@ImGui@@YAXXZ")]
        public static extern void EndChild();
        #endregion

        // TODO: GetWindowDrawList
        #region Windows Utilities
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?IsWindowAppearing@ImGui@@YA_NXZ")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool IsWindowAppearing();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?IsWindowCollapsed@ImGui@@YA_NXZ")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool IsWindowCollapsed();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?IsWindowFocused@ImGui@@YA_NH@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool IsWindowFocused(FocusedFlags flags = FocusedFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?IsWindowHovered@ImGui@@YA_NH@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool IsWindowHovered(HoveredFlags flags = HoveredFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetWindowPos@ImGui@@YA?AUImVec2@@XZ")]
        public static extern void GetWindowPos(ImVec2* ret);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetWindowSize@ImGui@@YA?AUImVec2@@XZ")]
        public static extern void GetWindowSize(ImVec2* ret);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetWindowWidth@ImGui@@YAMXZ")]
        public static extern float GetWindowWidth();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetWindowHeight@ImGui@@YAMXZ")]
        public static extern float GetWindowHeight();

        public static ImVec2 GetWindowPos()
        {
            ImVec2 ret;
            GetWindowPos(&ret);
            return ret;
        }

        public static ImVec2 GetWindowSize()
        {
            ImVec2 ret;
            GetWindowSize(&ret);
            return ret;
        }
        #endregion

        // TODO: SetNextWindowSizeConstraints
        #region Window manipulation
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetNextWindowPos@ImGui@@YAXAEBUImVec2@@H0@Z")]
        public static extern void SetNextWindowPos(in ImVec2 pos, Cond cond = Cond.None, in ImVec2 pivot = default);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetNextWindowSize@ImGui@@YAXAEBUImVec2@@H@Z")]
        public static extern void SetNextWindowSize(in ImVec2 size, Cond cond = Cond.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetNextWindowContentSize@ImGui@@YAXAEBUImVec2@@@Z")]
        public static extern void SetNextWindowContentSize(in ImVec2 size);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetNextWindowCollapsed@ImGui@@YAX_NH@Z")]
        public static extern void SetNextWindowCollapsed([MarshalAs(UnmanagedType.I1)] bool collapsed, Cond cond = Cond.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetNextWindowFocus@ImGui@@YAXXZ")]
        public static extern void SetNextWindowFocus();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetNextWindowScroll@ImGui@@YAXAEBUImVec2@@@Z")]
        public static extern void SetNextWindowScroll(in ImVec2 scroll);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetNextWindowBgAlpha@ImGui@@YAXM@Z")]
        public static extern void SetNextWindowBgAlpha(float alpha);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetWindowPos@ImGui@@YAXAEBUImVec2@@H@Z")]
        public static extern void SetWindowPos(in ImVec2 pos, Cond cond = Cond.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetWindowSize@ImGui@@YAXAEBUImVec2@@H@Z")]
        public static extern void SetWindowSize(in ImVec2 size, Cond cond = Cond.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetWindowCollapsed@ImGui@@YAX_NH@Z")]
        public static extern void SetWindowCollapsed([MarshalAs(UnmanagedType.I1)] bool collapsed, Cond cond = Cond.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetWindowFocus@ImGui@@YAXXZ")]
        public static extern void SetWindowFocus();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetWindowFontScale@ImGui@@YAXM@Z")]
        public static extern void SetWindowFontScale(float scale);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetWindowPos@ImGui@@YAXPEBDAEBUImVec2@@H@Z")]
        public static extern void SetWindowPos(string name, in ImVec2 pos, Cond cond = Cond.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetWindowSize@ImGui@@YAXPEBDAEBUImVec2@@H@Z")]
        public static extern void SetWindowSize(string name, in ImVec2 size, Cond cond = Cond.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetWindowCollapsed@ImGui@@YAXPEBD_NH@Z")]
        public static extern void SetWindowCollapsed(string name, [MarshalAs(UnmanagedType.I1)] bool collapsed, Cond cond = Cond.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetWindowFocus@ImGui@@YAXPEBD@Z")]
        public static extern void SetWindowFocus(string name);
        #endregion

        #region Windows Scrolling
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetScrollX@ImGui@@YAMXZ")]
        public static extern float GetScrollX();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetScrollY@ImGui@@YAMXZ")]
        public static extern float GetScrollY();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetScrollX@ImGui@@YAXM@Z")]
        public static extern void SetScrollX(float scroll_x);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetScrollY@ImGui@@YAXM@Z")]
        public static extern void SetScrollY(float scroll_y);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetScrollMaxX@ImGui@@YAMXZ")]
        public static extern float GetScrollMaxX();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetScrollMaxY@ImGui@@YAMXZ")]
        public static extern float GetScrollMaxY();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetScrollHereX@ImGui@@YAXM@Z")]
        public static extern void SetScrollHereX(float center_x_ratio = 0.5f);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetScrollHereY@ImGui@@YAXM@Z")]
        public static extern void SetScrollHereY(float center_y_ratio = 0.5f);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetScrollFromPosX@ImGui@@YAXMM@Z")]
        public static extern void SetScrollFromPosX(float local_x, float center_x_ratio = 0.5f);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetScrollFromPosY@ImGui@@YAXMM@Z")]
        public static extern void SetScrollFromPosY(float local_y, float center_y_ratio = 0.5f);
        #endregion

        // TODO: region Parameters stacks (shared)
        // TODO: region Parameters stacks (current window)
        // TODO: region Style read access

        // TODO: Like everything
        #region Layout cursor positioning
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetCursorScreenPos@ImGui@@YA?AUImVec2@@XZ")]
        public static extern void GetCursorScreenPos(ImVec2* ret); // This is stupid
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetCursorScreenPos@ImGui@@YAXAEBUImVec2@@@Z")]
        public static extern void SetCursorScreenPos(in ImVec2 pos);

        public static ImVec2 GetCursorScreenPos()
        {
            ImVec2 ret;
            GetCursorScreenPos(&ret);
            return ret;
        }
        #endregion

        #region Other layout functions
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Separator@ImGui@@YAXXZ")]
        public static extern void Separator();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SameLine@ImGui@@YAXMM@Z")]
        public static extern void SameLine(float offset_from_start_x = 0.0f, float spacing = -1.0f);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?NewLine@ImGui@@YAXXZ")]
        public static extern void NewLine();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Spacing@ImGui@@YAXXZ")]
        public static extern void Spacing();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Dummy@ImGui@@YAXAEBUImVec2@@@Z")]
        public static extern void Dummy(in ImVec2 size);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Indent@ImGui@@YAXM@Z")]
        public static extern void Indent(float indent_w = 0.0f);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Unindent@ImGui@@YAXM@Z")]
        public static extern void Unindent(float indent_w = 0.0f);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?BeginGroup@ImGui@@YAXXZ")]
        public static extern void BeginGroup();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EndGroup@ImGui@@YAXXZ")]
        public static extern void EndGroup();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?AlignTextToFramePadding@ImGui@@YAXXZ")]
        public static extern void AlignTextToFramePadding();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetTextLineHeight@ImGui@@YAMXZ")]
        public static extern float GetTextLineHeight();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetTextLineHeightWithSpacing@ImGui@@YAMXZ")]
        public static extern float GetTextLineHeightWithSpacing();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetFrameHeight@ImGui@@YAMXZ")]
        public static extern float GetFrameHeight();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetFrameHeightWithSpacing@ImGui@@YAMXZ")]
        public static extern float GetFrameHeightWithSpacing();
        #endregion

        #region ID stack/scopes
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?PushID@ImGui@@YAXPEBD@Z")]
        public static extern void PushID(string str_id);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?PushID@ImGui@@YAXPEBD0@Z")]
        public static extern void PushID(nint str_id_begin, nint std_id_end);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?PushID@ImGui@@YAXPEBX@Z")]
        public static extern void PushID(nint ptr_id);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?PushID@ImGui@@YAXH@Z")]
        public static extern void PushID(int int_id);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?PopID@ImGui@@YAXXZ")]
        public static extern void PopID();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetID@ImGui@@YAIPEBD@Z")]
        public static extern uint GetID(string str_id);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetID@ImGui@@YAIPEBD0@Z")]
        public static extern uint GetID(nint str_id_begin, nint str_id_end);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetID@ImGui@@YAIPEBX@Z")]
        public static extern uint GetID(nint ptr_id);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetID@ImGui@@YAIH@Z")]
        public static extern uint GetID(int int_id);
        #endregion

        // TODO: Formating??? probably not needed tbh
        #region Widgets: Text
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?TextUnformatted@ImGui@@YAXPEBD0@Z")]
        public static extern void TextUnformatted(string text, nint text_end= 0);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Text@ImGui@@YAXPEBDZZ")]
        public static extern void Text(string fmt);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?TextColored@ImGui@@YAXAEBUImVec4@@PEBDZZ")]
        public static extern void TextColored(in ImVec4 col, string fmt);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?TextDisabled@ImGui@@YAXPEBDZZ")]
        public static extern void TextDisabled(string fmt);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?TextWrapped@ImGui@@YAXPEBDZZ")]
        public static extern void TextWrapped(string fmt);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?LabelText@ImGui@@YAXPEBD0ZZ")]
        public static extern void LabelText(string label, string fmt);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?BulletText@ImGui@@YAXPEBDZZ")]
        public static extern void BulletText(string fmt);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SeparatorText@ImGui@@YAXPEBD@Z")]
        public static extern void SeparatorText(string label);

        #endregion

        // TODO: CheckboxFlags
        #region Widgets: Main
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Button@ImGui@@YA_NPEBDAEBUImVec2@@@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool Button(string label, in ImVec2 size = default);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SmallButton@ImGui@@YA_NPEBD@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SmallButton(string label);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InvisibleButton@ImGui@@YA_NPEBDAEBUImVec2@@H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool InvisibleButton(string std_id, in ImVec2 size, ButtonFlags flags = ButtonFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ArrowButton@ImGui@@YA_NPEBDW4ImGuiDir@@@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ArrowButton(string std_id, Dir dir);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Checkbox@ImGui@@YA_NPEBDPEA_N@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool Checkbox(string label, [MarshalAs(UnmanagedType.I1)] ref bool v);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?RadioButton@ImGui@@YA_NPEBD_N@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool RadioButton(string label, [MarshalAs(UnmanagedType.I1)] bool active);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?RadioButton@ImGui@@YA_NPEBDPEAHH@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool RadioButton(string label, ref int v, int v_button);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ProgressBar@ImGui@@YAXMAEBUImVec2@@PEBD@Z")]
        public static extern void ProgressBar(float fraction, in ImVec2 size_arg = default, string? overlay = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Bullet@ImGui@@YAXXZ")]
        public static extern void Bullet();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?TextLink@ImGui@@YA_NPEBD@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool TextLink(string label);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?TextLinkOpenURL@ImGui@@YAXPEBD0@Z")]
        public static extern void TextLinkOpenURL(string label, string? url = null);
        #endregion

        // TODO: region Widgets: Images

        // TODO: 2 Combo()
        #region Widgets: Combo Box (Dropdown)
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?BeginCombo@ImGui@@YA_NPEBD0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool BeginCombo(string label, string preview_value, ComboFlags flags = ComboFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EndCombo@ImGui@@YAXXZ")]
        public static extern void EndCombo();
        // TODO
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Combo@ImGui@@YA_NPEBDPEAH0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool Combo(string label, ref int current_item, string items_separated_by_zeros, int popup_max_height_in_items = -1);
        // TODO
        #endregion

        // TODO: Scalar inputs
        #region Widgets: Drag Sliders
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?DragFloat@ImGui@@YA_NPEBDPEAMMMM0H@Z")]
        private static extern byte _DragFloat(string label, float* v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?DragFloat2@ImGui@@YA_NPEBDQEAMMMM0H@Z")]
        private static extern byte _DragFloat2(string label, float* v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?DragFloat3@ImGui@@YA_NPEBDQEAMMMM0H@Z")]
        private static extern byte _DragFloat3(string label, float* v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?DragFloat4@ImGui@@YA_NPEBDQEAMMMM0H@Z")]
        private static extern byte _DragFloat4(string label, float* v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?DragFloatRange2@ImGui@@YA_NPEBDPEAM1MMM00H@Z")]
        private static extern byte _DragFloatRange2(string label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, string format = "%.3f", string? format_max = null, SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?DragInt@ImGui@@YA_NPEBDPEAHMHH0H@Z")]
        private static extern byte _DragInt(string label, int* v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, string format = "%d", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?DragInt2@ImGui@@YA_NPEBDQEAHMHH0H@Z")]
        private static extern byte _DragInt2(string label, int* v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, string format = "%d", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?DragInt3@ImGui@@YA_NPEBDQEAHMHH0H@Z")]
        private static extern byte _DragInt3(string label, int* v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, string format = "%d", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?DragInt4@ImGui@@YA_NPEBDQEAHMHH0H@Z")]
        private static extern byte _DragInt4(string label, int* v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, string format = "%d", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?DragIntRange2@ImGui@@YA_NPEBDPEAH1MHH00H@Z")]
        private static extern byte _DragIntRange2(string label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, string format = "%d", string? format_max = null, SliderFlags flags = SliderFlags.None);

        public static bool DragFloat(string label, float* v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None) => 
            _DragFloat(label, v, v_speed, v_min, v_max, format, flags) != 0;
        public static bool DragFloat2(string label, float* v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None) => 
            _DragFloat2(label, v, v_speed, v_min, v_max, format, flags) != 0;
        public static bool DragFloat3(string label, float* v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None) => 
            _DragFloat3(label, v, v_speed, v_min, v_max, format, flags) != 0;
        public static bool DragFloat4(string label, float* v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None) => 
            _DragFloat4(label, v, v_speed, v_min, v_max, format, flags) != 0;
        public static bool DragFloat(string label, ref float v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
        {
            fixed (float* vptr = &v)
            {
                return _DragFloat(label, vptr, v_speed, v_min, v_max, format, flags) != 0;
            }
        }
        public static bool DragFloat2(string label, ref float[] v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
        {
            fixed (float* vptr = v)
            {
                return _DragFloat2(label, vptr, v_speed, v_min, v_max, format, flags) != 0;
            }
        }
        public static bool DragFloat3(string label, ref float[] v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
        {
            fixed (float* vptr = v)
            {
                return _DragFloat3(label, vptr, v_speed, v_min, v_max, format, flags) != 0;
            }
        }
        public static bool DragFloat4(string label, ref float[] v, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
        {
            fixed (float* vptr = v)
            {
                return _DragFloat4(label, vptr, v_speed, v_min, v_max, format, flags) != 0;
            }
        }

        public static bool DragFloatRange2(string label, float* v_current_min, float* v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, string format = "%.3f", string? format_max = null, SliderFlags flags = SliderFlags.None)
            => _DragFloatRange2(label, v_current_min, v_current_max, v_speed, v_min, v_max, format, format_max, flags) != 0;
        public static bool DragFloatRange2(string label, ref float v_current_min, ref float v_current_max, float v_speed = 1.0f, float v_min = 0.0f, float v_max = 0.0f, string format = "%.3f", string? format_max = null, SliderFlags flags = SliderFlags.None)
        {
            fixed (float* curminptr = &v_current_min)
            {
                fixed (float* curmaxptr = &v_current_max)
                {
                    return _DragFloatRange2(label, curminptr, curmaxptr, v_speed, v_min, v_max, format, format_max, flags) != 0;
                }
            }
        }
        public static bool DragInt(string label, int* v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, string format = "%d", SliderFlags flags = SliderFlags.None) => _DragInt(label, v, v_speed, v_min, v_max, format, flags) != 0;
        public static bool DragInt2(string label, int* v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, string format = "%d", SliderFlags flags = SliderFlags.None) => _DragInt2(label, v, v_speed, v_min, v_max, format, flags) != 0;
        public static bool DragInt3(string label, int* v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, string format = "%d", SliderFlags flags = SliderFlags.None) => _DragInt3(label, v, v_speed, v_min, v_max, format, flags) != 0;
        public static bool DragInt4(string label, int* v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, string format = "%d", SliderFlags flags = SliderFlags.None) => _DragInt4(label, v, v_speed, v_min, v_max, format, flags) != 0;
        public static bool DragInt(string label, ref int v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, string format = "%d", SliderFlags flags = SliderFlags.None)
        {
            fixed (int* vptr = &v)
            {
                return DragInt(label, vptr, v_speed, v_min, v_max, format, flags);
            }
        }
        public static bool DragInt2(string label, ref int[] v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, string format = "%d", SliderFlags flags = SliderFlags.None)
        {
            fixed (int* vptr = v)
            {
                return DragInt2(label, vptr, v_speed, v_min, v_max, format, flags);
            }
        }
        public static bool DragInt3(string label, ref int[] v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, string format = "%d", SliderFlags flags = SliderFlags.None)
        {
            fixed (int* vptr = v)
            {
                return DragInt3(label, vptr, v_speed, v_min, v_max, format, flags);
            }
        }
        public static bool DragInt4(string label, ref int[] v, float v_speed = 1.0f, int v_min = 0, int v_max = 0, string format = "%d", SliderFlags flags = SliderFlags.None)
        {
            fixed (int* vptr = v)
            {
                return DragInt4(label, vptr, v_speed, v_min, v_max, format, flags);
            }
        }
        public static bool DragIntRange2(string label, int* v_current_min, int* v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, string format = "%d", string? format_max = null, SliderFlags flags = SliderFlags.None)
            => _DragIntRange2(label, v_current_min, v_current_max, v_speed, v_min, v_max, format, format_max, flags) != 0;

        public static bool DragIntRange2(string label, ref int v_current_min, ref int v_current_max, float v_speed = 1.0f, int v_min = 0, int v_max = 0, string format = "%d", string? format_max = null, SliderFlags flags = SliderFlags.None)
        {
            fixed (int* curminptr = &v_current_min)
            {
                fixed (int* curmaxptr = &v_current_max)
                {
                    return DragIntRange2(label, curminptr, curmaxptr, v_speed, v_min, v_max, format, format_max, flags);
                }
            }
        }
        #endregion

        // TODO: Scalar inputs
        #region Widgets: Regular Sliders
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SliderFloat@ImGui@@YA_NPEBDPEAMMM0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SliderFloat(string label, float* v, float v_min, float v_max, string format = "%.3f", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SliderFloat2@ImGui@@YA_NPEBDQEAMMM0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SliderFloat2(string label, float* v, float v_min, float v_max, string format = "%.3f", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SliderFloat3@ImGui@@YA_NPEBDQEAMMM0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SliderFloat3(string label, float* v, float v_min, float v_max, string format = "%.3f", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SliderFloat4@ImGui@@YA_NPEBDQEAMMM0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SliderFloat4(string label, float* v, float v_min, float v_max, string format = "%.3f", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SliderAngle@ImGui@@YA_NPEBDPEAMMM0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SliderAngle(string label, float* v_rad, float v_degrees_min = -360.0f, float v_degrees_max = +360.0f, string format = "%.0f deg", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SliderInt@ImGui@@YA_NPEBDPEAHHH0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SliderInt(string label, int* v, int v_min, int v_max, string format = "%d", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SliderInt2@ImGui@@YA_NPEBDQEAHHH0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SliderInt2(string label, int* v, int v_min, int v_max, string format = "%d", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SliderInt3@ImGui@@YA_NPEBDQEAHHH0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SliderInt3(string label, int* v, int v_min, int v_max, string format = "%d", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SliderInt4@ImGui@@YA_NPEBDQEAHHH0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SliderInt4(string label, int* v, int v_min, int v_max, string format = "%d", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?VSliderFloat@ImGui@@YA_NPEBDAEBUImVec2@@PEAMMM0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool VSliderFloat(string label, in ImVec2 size, float* v, float v_min, float v_max, string format = "%.3f", SliderFlags flags = SliderFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?VSliderInt@ImGui@@YA_NPEBDAEBUImVec2@@PEAHHH0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool VSliderInt(string label, in ImVec2 size, int* v, int v_min, int v_max, string format = "%d", SliderFlags flags = SliderFlags.None);

        public static bool SliderAngle(string label, ref float v_rad, float v_degrees_min = -360.0f, float v_degrees_max = +360.0f, string format = "%.0f deg", SliderFlags flags = SliderFlags.None)
        {
            fixed (float* vptr = &v_rad)
                return SliderAngle(label, vptr, v_degrees_min, v_degrees_max, format, flags);
        }
        public static bool SliderFloat(string label, ref float v, float v_min, float v_max, string format = "%.3f", SliderFlags flags = SliderFlags.None)
        {
            fixed (float* vptr = &v)
                return SliderFloat(label, vptr, v_min, v_max, format, flags);
        }
        public static bool SliderFloat2(string label, ref float[] v, float v_min, float v_max, string format = "%.3f", SliderFlags flags = SliderFlags.None)
        {
            fixed (float* vptr = v)
                return SliderFloat2(label, vptr, v_min, v_max, format, flags);
        }
        public static bool SliderFloat3(string label, ref float[] v, float v_min, float v_max, string format = "%.3f", SliderFlags flags = SliderFlags.None)
        {
            fixed (float* vptr = v)
                return SliderFloat3(label, vptr, v_min, v_max, format, flags);
        }
        public static bool SliderFloat4(string label, ref float[] v, float v_min, float v_max, string format = "%.3f", SliderFlags flags = SliderFlags.None)
        {
            fixed (float* vptr = v)
                return SliderFloat4(label, vptr, v_min, v_max, format, flags);
        }
        public static bool SliderInt(string label, ref int v, int v_min, int v_max, string format = "%d", SliderFlags flags = SliderFlags.None)
        {
            fixed (int* vptr = &v)
                return SliderInt(label, vptr, v_min, v_max, format, flags);
        }
        public static bool SliderInt2(string label, ref int[] v, int v_min, int v_max, string format = "%d", SliderFlags flags = SliderFlags.None)
        {
            fixed (int* vptr = v)
                return SliderInt2(label, vptr, v_min, v_max, format, flags);
        }
        public static bool SliderInt3(string label, ref int[] v, int v_min, int v_max, string format = "%d", SliderFlags flags = SliderFlags.None)
        {
            fixed (int* vptr = v)
                return SliderInt3(label, vptr, v_min, v_max, format, flags);
        }
        public static bool SliderInt4(string label, ref int[] v, int v_min, int v_max, string format = "%d", SliderFlags flags = SliderFlags.None)
        {
            fixed (int* vptr = v)
                return SliderInt4(label, vptr, v_min, v_max, format, flags);
        }
        public static bool VSliderInt(string label, in ImVec2 size, ref int v, int v_min, int v_max, string format = "%d", SliderFlags flags = SliderFlags.None)
        {
            fixed (int* vptr = &v)
                return VSliderInt(label, size, vptr, v_min, v_max, format, flags);
        }
        public static bool VSliderFloat(string label, in ImVec2 size, ref float v, float v_min, float v_max, string format = "%.3f", SliderFlags flags = SliderFlags.None)
        {
            fixed (float* vptr = &v)
                return VSliderFloat(label, size, vptr, v_min, v_max, format, flags);
        }

        #endregion

        // TODO: Better InputText
        // TODO: Scalar inputs
        #region Widgets: Input with Keyboard
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InputText@ImGui@@YA_NPEBDPEAD_KHP6AHPEAUImGuiInputTextCallbackData@@@ZPEAX@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool InputText(string label, byte* buf, ulong buf_size, InputTextFlags flags = InputTextFlags.None, delegate*<InputTextCallbackData*, int> callback = null, void* user_data = null);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InputTextMultiline@ImGui@@YA_NPEBDPEAD_KAEBUImVec2@@HP6AHPEAUImGuiInputTextCallbackData@@@ZPEAX@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool InputTextMultiline(string label, byte* buf, ulong buf_size, in ImVec2 size = default, InputTextFlags flags = InputTextFlags.None, delegate*<InputTextCallbackData*, int> callback = null, void* user_data = null);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InputTextWithHint@ImGui@@YA_NPEBD0PEAD_KHP6AHPEAUImGuiInputTextCallbackData@@@ZPEAX@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool InputTextWithHint(string label, string hint, byte* buf, ulong buf_size, InputTextFlags flags = InputTextFlags.None, delegate*<InputTextCallbackData*, int> callback = null, void* user_data = null);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InputFloat@ImGui@@YA_NPEBDPEAMMM0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool InputFloat(string label, float* v, float step = 0.0f, float step_fast = 0.0f, string format = "%.3f", InputTextFlags flags = InputTextFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InputFloat2@ImGui@@YA_NPEBDQEAM0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool InputFloat2(string label, float* v, string format = "%.3f", InputTextFlags flags = InputTextFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InputFloat3@ImGui@@YA_NPEBDQEAM0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool InputFloat3(string label, float* v, string format = "%.3f", InputTextFlags flags = InputTextFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InputFloat4@ImGui@@YA_NPEBDQEAM0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool InputFloat4(string label, float* v, string format = "%.3f", InputTextFlags flags = InputTextFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InputInt@ImGui@@YA_NPEBDPEAHHHH@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool InputInt(string label, int* v, int step = 1, int step_fast = 100, InputTextFlags flags = InputTextFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InputInt2@ImGui@@YA_NPEBDQEAHH@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool InputInt2(string label, int* v, InputTextFlags flags = InputTextFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InputInt3@ImGui@@YA_NPEBDQEAHH@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool InputInt3(string label, int* v, InputTextFlags flags = InputTextFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InputInt4@ImGui@@YA_NPEBDQEAHH@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool InputInt4(string label, int* v, InputTextFlags flags = InputTextFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?InputDouble@ImGui@@YA_NPEBDPEANNN0H@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool InputDouble(string label, double* v, double step = 0.0, double step_fast = 0.0, string format = "%.6f", InputTextFlags flags = InputTextFlags.None);

        public static bool InputText(string label, ref byte[] buf, InputTextFlags flags = InputTextFlags.None, delegate*<InputTextCallbackData*, int> callback = null, void* user_data = null)
        {
            fixed (byte* bufptr = buf)
                return InputText(label, bufptr, (ulong)buf.Length, flags, callback, user_data);
        }
        public static bool InputTextMultiline(string label, ref byte[] buf, in ImVec2 size = default, InputTextFlags flags = InputTextFlags.None, delegate*<InputTextCallbackData*, int> callback = null, void* user_data = null)
        {
            fixed (byte* bufptr = buf)
                return InputTextMultiline(label, bufptr, (ulong)buf.Length, size, flags, callback, user_data);
        }
        public static bool InputTextWithHint(string label, string hint, ref byte[] buf, InputTextFlags flags = InputTextFlags.None, delegate*<InputTextCallbackData*, int> callback = null, void* user_data = null)
        {
            fixed (byte* bufptr = buf)
                return InputTextWithHint(label, hint, bufptr, (ulong)buf.Length, flags, callback, user_data);
        }
        public static bool InputFloat(string label, ref float v, float step = 0.0f, float step_fast = 0.0f, string format = "%.3f", InputTextFlags flags = InputTextFlags.None)
        {
            fixed (float* vptr = &v)
                return InputFloat(label, vptr, step, step_fast, format, flags);
        }
        public static bool InputFloat2(string label, ref float[] v, string format = "%.3f", InputTextFlags flags = InputTextFlags.None)
        {
            fixed (float* vptr = v)
                return InputFloat2(label, vptr, format, flags);
        }
        public static bool InputFloat3(string label, ref float[] v, string format = "%.3f", InputTextFlags flags = InputTextFlags.None)
        {
            fixed (float* vptr = v)
                return InputFloat3(label, vptr, format, flags);
        }
        public static bool InputFloat4(string label, ref float[] v, string format = "%.3f", InputTextFlags flags = InputTextFlags.None)
        {
            fixed (float* vptr = v)
                return InputFloat4(label, vptr, format, flags);
        }
        public static bool InputInt(string label, ref int v, int step = 1, int step_fast = 100, InputTextFlags flags = InputTextFlags.None)
        {
            fixed (int* vptr = &v)
                return InputInt(label, vptr, step, step_fast, flags);
        }
        public static bool InputInt2(string label, ref int[] v, InputTextFlags flags = InputTextFlags.None)
        {
            fixed (int* vptr = v)
                return InputInt2(label, vptr, flags);
        }
        public static bool InputInt3(string label, ref int[] v, InputTextFlags flags = InputTextFlags.None)
        {
            fixed (int* vptr = v)
                return InputInt3(label, vptr, flags);
        }
        public static bool InputInt4(string label, ref int[] v, InputTextFlags flags = InputTextFlags.None)
        {
            fixed (int* vptr = v)
                return InputInt4(label, vptr, flags);
        }
        public static bool InputDouble(string label, ref double v, double step = 0.0, double step_fast = 0.0, string format = "%.6f", InputTextFlags flags = InputTextFlags.None)
        {
            fixed (double* vptr = &v)
                return InputDouble(label, vptr, step, step_fast, format, flags);
        }
        #endregion

        #region Widgets: Color Editor/Picker
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ColorEdit3@ImGui@@YA_NPEBDQEAMH@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ColorEdit3(string label, float* col, ColorEditFlags flags = ColorEditFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ColorEdit4@ImGui@@YA_NPEBDQEAMH@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ColorEdit4(string label, float* col, ColorEditFlags flags = ColorEditFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ColorPicker3@ImGui@@YA_NPEBDQEAMH@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ColorPicker3(string label, float* col, ColorEditFlags flags = ColorEditFlags.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ColorPicker4@ImGui@@YA_NPEBDQEAMHPEBM@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ColorPicker4(string label, float* col, ColorEditFlags flags = ColorEditFlags.None, float* ref_col = null);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ColorButton@ImGui@@YA_NPEBDAEBUImVec4@@HAEBUImVec2@@@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ColorButton(string desc_id, ref ImVec4 col, ColorEditFlags flags = ColorEditFlags.None, in ImVec4 size = default);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetColorEditOptions@ImGui@@YAXH@Z")]
        public static extern void SetColorEditOptions(ColorEditFlags flags);

        public static bool ColorEdit3(string label, ref float[] col, ColorEditFlags flags = ColorEditFlags.None)
        {
            fixed (float* colptr = col)
                return ColorEdit3(label, colptr, flags);
        }
        public static bool ColorEdit4(string label, ref float[] col, ColorEditFlags flags = ColorEditFlags.None)
        {
            fixed (float* colptr = col)
                return ColorEdit4(label, colptr, flags);
        }
        public static bool ColorPicker3(string label, ref float[] col, ColorEditFlags flags = ColorEditFlags.None)
        {
            fixed (float* colptr = col)
                return ColorPicker3(label, colptr, flags);
        }
        public static bool ColorPicker4(string label, ref float[] col, ColorEditFlags flags = ColorEditFlags.None, float* ref_col = null)
        {
            fixed (float* colptr = col)
                return ColorPicker4(label, colptr, flags, ref_col);
        }

        public static bool ColorEdit3(string label, ref ImVec4 col, ColorEditFlags flags = ColorEditFlags.None)
        {
            fixed (ImVec4* colptr = &col)
                return ColorEdit3(label, (float*)colptr, flags);
        }
        public static bool ColorEdit4(string label, ref ImVec4 col, ColorEditFlags flags = ColorEditFlags.None)
        {
            fixed (ImVec4* colptr = &col)
                return ColorEdit4(label, (float*)colptr, flags);
        }
        public static bool ColorPicker3(string label, ref ImVec4 col, ColorEditFlags flags = ColorEditFlags.None)
        {
            fixed (ImVec4* colptr = &col)
                return ColorPicker3(label, (float*)colptr, flags);
        }
        public static bool ColorPicker4(string label, ref ImVec4 col, ColorEditFlags flags = ColorEditFlags.None, float* ref_col = null)
        {
            fixed (ImVec4* colptr = &col)
                return ColorPicker4(label, (float*)colptr, flags, ref_col);
        }
        #endregion

        // TODO: Rest of this region
        #region Widgets: Trees
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?TreeNode@ImGui@@YA_NPEBD@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool TreeNode(string label);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?TreePop@ImGui@@YAXXZ")]
        public static extern void TreePop();

        #endregion

        #region Widgets: Selectables
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Selectable@ImGui@@YA_NPEBD_NHAEBUImVec2@@@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool Selectable(string label, bool selected = false, SelectableFlags flags = SelectableFlags.None, in ImVec2 size = default);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Selectable@ImGui@@YA_NPEBDPEA_NHAEBUImVec2@@@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool Selectable(string label, bool* p_selected, SelectableFlags flags = SelectableFlags.None, in ImVec2 size = default);
        #endregion

        // TODO: region Multi-selection system

        // TODO: 2 ListBox()
        #region Widgets: List Boxes
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?BeginListBox@ImGui@@YA_NPEBDAEBUImVec2@@@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool BeginListBox(string label, in ImVec2 size = default);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EndListBox@ImGui@@YAXXZ")]
        public static extern void EndListBox();
        #endregion

        // TODO: 2 missing
        #region Widgets: Data Plotting
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?PlotLines@ImGui@@YAXPEBDPEBMHH0MMUImVec2@@H@Z")]
        public static extern void PlotLines(string label, float* values, int values_count, int values_offset = 0, string? overlay_text = null, float scale_min = float.MaxValue, float scale_max = float.MaxValue, ImVec2 graph_size = default, int stride = 4);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?PlotHistogram@ImGui@@YAXPEBDPEBMHH0MMUImVec2@@H@Z")]
        public static extern void PlotHistogram(string label, float* values, int values_count, int values_offset = 0, string? overlay_text = null, float scale_min = float.MaxValue, float scale_max = float.MaxValue, ImVec2 graph_size = default, int stride = 4);

        public static void PlotLines(string label, ref float[] values, int values_offset = 0, string? overlay_text = null, float scale_min = float.MaxValue, float scale_max = float.MaxValue, ImVec2 graph_size = default, int stride = 4)
        {
            fixed (float* valuesptr = values)
                PlotLines(label, valuesptr, values.Length, values_offset, overlay_text, scale_min, scale_max, graph_size, stride);
        }
        public static void PlotHistogram(string label, ref float[] values, int values_offset = 0, string? overlay_text = null, float scale_min = float.MaxValue, float scale_max = float.MaxValue, ImVec2 graph_size = default, int stride = 4)
        {
            fixed (float* valuesptr = values)
                PlotHistogram(label, valuesptr, values.Length, values_offset, overlay_text, scale_min, scale_max, graph_size, stride);
        }
        #endregion

        #region Tab Bars, Tabs
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?BeginTabBar@ImGui@@YA_NPEBDH@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool BeginTabBar(string str_id, TabBarFlags flags = TabBarFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EndTabBar@ImGui@@YAXXZ")]
        public static extern void EndTabBar();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?BeginTabItem@ImGui@@YA_NPEBDPEA_NH@Z")]
        private static extern byte _BeginTabItem(string label, byte* p_open = null, TabItemFlags flags = TabItemFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EndTabItem@ImGui@@YAXXZ")]
        public static extern void EndTabItem();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?TabItemButton@ImGui@@YA_NPEBDH@Z")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool TabItemButton(string label, TabItemFlags flags = TabItemFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetTabItemClosed@ImGui@@YAXPEBD@Z")]
        public static extern void SetTabItemClosed(string tab_or_docked_window_label);


        public static bool BeginTabItem(string label, bool* p_open = null, TabItemFlags flags = TabItemFlags.None)
        {
            byte val = 1;
            if (p_open is not null && *p_open is false)
                val = 0;
            var ret = _BeginTabItem(label, p_open is null ? null : &val, flags) != 0;
            if (p_open is not null)
                *p_open = val != 0;
            return ret;
        }
        #endregion


        #region Structs
        [StructLayout(LayoutKind.Sequential)] public struct ImVec2
        {
            public float x;
            public float y;

            public ImVec2(float _x, float _y)
            {
                x = _x;
                y = _y;
            }

            public override string ToString()
            {
                return $"({x}, {y})";
            }
        }
        [StructLayout(LayoutKind.Sequential)] public struct ImVec4
        {
            public float x, y, z, w;

            public ImVec4(float _x, float _y, float _z, float _w)
            {
                x = _x;
                y = _y;
                z = _z;
                w = _w;
            }

            public override string ToString()
            {
                return $"({x}, {y}, {z}, {w})";
            }
        }
        [StructLayout(LayoutKind.Sequential)] public unsafe struct KeyData
        {
            [MarshalAs(UnmanagedType.I1)] public bool Down;
            public float DownDuration;
            public float DownDurationPrev;
            public float AnalogValue;
        }
        [StructLayout(LayoutKind.Sequential)] public struct ImGuiStyle
        {
            public float Alpha;
            public float DisabledAlpha;
            public ImVec2 WindowPadding;
            public float WindowRounding;
            public float WindowBorderSize;
            public ImVec2 WindowMinSize;
            public ImVec2 WindowTitleAlign;
            public Dir WindowMenuButtonPosition;
            public float ChildRounding;
            public float ChildBorderSize;
            public float PopupRounding;
            public float PopupBorderSize;
            public ImVec2 FramePadding;
            public float FrameRounding;
            public float FrameBorderSize;
            public ImVec2 ItemSpacing;
            public ImVec2 ItemInnerSpacing;
            public ImVec2 CellPadding;
            public ImVec2 TouchExtraPadding;
            public float IndentSpacing;
            public float ColumnsMinSpacing;
            public float ScrollbarSize;
            public float ScrollbarRounding;
            public float GrabMinSize;
            public float GrabRounding;
            public float LogSliderDeadzone;
            public float TabRounding;
            public float TabBorderSize;
            public float TabMinWidthForCloseButton;
            public float TabBarBorderSize;
            public float TabBarOverlineSize;
            public float TableAngledHeadersAngle;
            public ImVec2 TableAngledHeadersTextAlign;
            public Dir ColorButtonPosition;
            public ImVec2 ButtonTextAlign;
            public ImVec2 SelectableTextAlign;
            public float SeparatorTextBorderSize;
            public ImVec2 SeparatorTextAlign;
            public ImVec2 SeparatorTextPadding;
            public ImVec2 DisplayWindowPadding;
            public ImVec2 DisplaySafeAreaPadding;
            public float MouseCursorScale;
            [MarshalAs(UnmanagedType.I1)] public bool AntiAliasedLines;
            [MarshalAs(UnmanagedType.I1)] public bool AntiAliasedLinesUseTex;
            [MarshalAs(UnmanagedType.I1)] public bool AntiAliasedFill;
            public float CurveTessellationTol;
            public float CircleTessellationMaxError;
            public fixed byte Colors[56 * 16]; // TODO

            public float HoverStationaryDelay;
            public float HoverDelayShort;
            public float HoverDelayNormal;
            public HoveredFlags HoverFlagsForTooltipMouse;
            public HoveredFlags HoverFlagsForTooltipNav;
        }
        [StructLayout(LayoutKind.Sequential)] public unsafe struct ImGuiIO
        {
            public ConfigFlags ConfigFlags;
            public BackendFlags BackendFlags;
            public ImVec2 DisplaySize;
            public float DeltaTime;
            public float IniSavingRate;
            private nint _IniFilename;
            private nint _LogFileName;
            public nint UserData;

            public nint Fonts; // TODO: ImFontAtlas
            public float FontGlobalScale;
            [MarshalAs(UnmanagedType.I1)] public bool FontAllowUserScaling;
            public nint FontDefault; // TODO: ImFont
            public ImVec2 DisplayFramebufferScale;

            [MarshalAs(UnmanagedType.I1)] public bool ConfigNavSwapGamepadButtons;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigNavMoveSetMousePos;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigNavCaptureKeyboard;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigNavEscapeClearFocusItem;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigNavEscapeClearFocusWindow;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigNavCursorVisibleAuto;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigNavCursorVisibleAlways;

            [MarshalAs(UnmanagedType.I1)] public bool MouseDrawCursor;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigMacOSXBehaviors;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigInputTrickleEventQueue;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigInputTextCursorBlink;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigInputTextEnterKeepActive;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigDragClickToInputText;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigWindowsResizeFromEdges;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigWindowsMoveFromTitleBarOnly;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigWindowsCopyContentsWithCtrlC;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigScrollbarScrollByPage;
            public float ConfigMemoryCompactTimer;

            public float MouseDoubleClickTime;
            public float MouseDoubleClickMaxDist;
            public float MouseDragThreshold;
            public float KeyRepeatDelay;
            public float KeyRepeatRate;

            [MarshalAs(UnmanagedType.I1)] public bool ConfigErrorRecovery;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigErrorRecoveryEnableAssert;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigErrorRecoveryEnableDebugLog;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigErrorRecoveryEnableTooltip;

            [MarshalAs(UnmanagedType.I1)] public bool ConfigDebugIsDebuggerPresent;

            [MarshalAs(UnmanagedType.I1)] public bool ConfigDebugHighlightIdConflicts;

            [MarshalAs(UnmanagedType.I1)] public bool ConfigDebugBeginReturnValueOnce;
            [MarshalAs(UnmanagedType.I1)] public bool ConfigDebugBeginReturnValueLoop;

            [MarshalAs(UnmanagedType.I1)] public bool ConfigDebugIgnoreFocusLoss;

            [MarshalAs(UnmanagedType.I1)] public bool ConfigDebugIniSettings;

            public nint BackendPlatformName;
            public nint BackendRendererName;
            public nint BackendPlatformUserData;
            public nint BackendRendererUserData;
            public nint BackendLanguageUserData;

            [MarshalAs(UnmanagedType.I1)] public bool WantCaptureMouse;
            [MarshalAs(UnmanagedType.I1)] public bool WantCaptureKeyboard;
            [MarshalAs(UnmanagedType.I1)] public bool WantTextInput;
            [MarshalAs(UnmanagedType.I1)] public bool WantSetMousePos;
            [MarshalAs(UnmanagedType.I1)] public bool WantSaveIniSettings;
            [MarshalAs(UnmanagedType.I1)] public bool NavActive;
            [MarshalAs(UnmanagedType.I1)] public bool NavVisible;
            public float Framerate;
            public int MetricsRenderVertices;
            public int MetricsRenderIndices;
            public int MetricsRenderWindows;
            public int MetricsActiveWindows;
            public ImVec2 MouseDelta;

            public nint Ctx;

            public ImVec2 MousePos;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)] public fixed bool MouseDown[5];
            public float MouseWheel;
            public float MouseWheelH;
            public ImGuiMouseSource MouseSource;
            [MarshalAs(UnmanagedType.I1)] public bool KeyCtrl;
            [MarshalAs(UnmanagedType.I1)] public bool KeyShift;
            [MarshalAs(UnmanagedType.I1)] public bool KeyAlt;
            [MarshalAs(UnmanagedType.I1)] public bool KeySuper;

            // Honestly everything below should be private, why would this ever need to be accessed
            public int KeyMods;
            private fixed byte _KeysData[154 * 16]; // Being unable to access this should not be a problem
            [MarshalAs(UnmanagedType.I1)] public bool WantCaptureMouseUnlessPopupClose;
            public ImVec2 MousePosPrev;
            public fixed ulong MouseClickedPos[5]; // Really a ImVec2 array
            public fixed double MouseClickedTime[5];
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)] public fixed bool MouseClicked[5];
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)] public fixed bool MouseDoubleClicked[5];
            public fixed ushort MouseClickedCount[5];
            public fixed ushort MouseClickedLastCount[5];
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)] public fixed bool MouseReleased[5];
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)] public fixed bool MouseDownOwned[5];
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)] public fixed bool MouseDownOwnedUnlessPopupClose[5];
            [MarshalAs(UnmanagedType.I1)] public bool MouseWheelRequestAxisSwap;
            [MarshalAs(UnmanagedType.I1)] public bool MouseCtrlLeftAsRightClick;
            public fixed float MouseDownDuration[5];
            public fixed float MouseDownDurationPrev[5];
            public fixed float MouseDragMaxDistanceSqr[5];
            public float PenPressure;
            [MarshalAs(UnmanagedType.I1)] public bool AppFocusLost;
            [MarshalAs(UnmanagedType.I1)] public bool AppAcceptingEvents;
            public char InputQueueSurrogate;

            public string? IniFilename => Marshal.PtrToStringAnsi(_IniFilename);
            public string? LogFileName => Marshal.PtrToStringAnsi(_LogFileName);
        }
        [StructLayout(LayoutKind.Sequential)] public unsafe struct ImVector<T> where T : unmanaged
        {
            public int Size;
            public int Capacity;
            public T* Data;
        }
        [StructLayout(LayoutKind.Sequential)] public struct ImDrawData
        {
            [MarshalAs(UnmanagedType.I1)] public bool Valid;
            public int CmdListsCount;
            public int TotalIdxCount;
            public int TotalVtxCount;
            public ImVector<nint> CmdLists;
            public ImVec2 DisplayPos;
            public ImVec2 DisplaySize;
            public ImVec2 FramebufferScale;
            public nint OwnerViewport;
        }
        [StructLayout(LayoutKind.Sequential)] public struct InputTextCallbackData
        {
            public nint Ctx;
            public InputTextFlags EventFlag;
            public InputTextFlags Flags;
            public nint UserData;

            public ushort EventChar;
            public int/*ImGuiKey*/ EventKey;
            public char* Buf;
            public int BufTextLen;
            public int BufSize;
            [MarshalAs(UnmanagedType.I1)] public bool BufDirty;
            public int CursorPos;
            public int SelectionStart;
            public int SelectionEnd;
        }
        #endregion

        #region Enums
        [Flags] public enum WindowFlags : int
        {
            None = 0,
            NoTitleBar = 1 << 0,   // Disable title-bar
            NoResize = 1 << 1,   // Disable user resizing with the lower-right grip
            NoMove = 1 << 2,   // Disable user moving the window
            NoScrollbar = 1 << 3,   // Disable scrollbars (window can still scroll with mouse or programmatically)
            NoScrollWithMouse = 1 << 4,   // Disable user vertically scrolling with mouse wheel. On child window, mouse wheel will be forwarded to the parent unless NoScrollbar is also set.
            NoCollapse = 1 << 5,   // Disable user collapsing window by double-clicking on it. Also referred to as Window Menu Button (e.g. within a docking node).
            AlwaysAutoResize = 1 << 6,   // Resize every window to its content every frame
            NoBackground = 1 << 7,   // Disable drawing background color (WindowBg, etc.) and outside border. Similar as using SetNextWindowBgAlpha(0.0f).
            NoSavedSettings = 1 << 8,   // Never load/save settings in .ini file
            NoMouseInputs = 1 << 9,   // Disable catching mouse, hovering test with pass through.
            MenuBar = 1 << 10,  // Has a menu-bar
            HorizontalScrollbar = 1 << 11,  // Allow horizontal scrollbar to appear (off by default). You may use SetNextWindowContentSize(ImVec2(width,0.0f)); prior to calling Begin() to specify width. Read code in imgui_demo in the "Horizontal Scrolling" section.
            NoFocusOnAppearing = 1 << 12,  // Disable taking focus when transitioning from hidden to visible state
            NoBringToFrontOnFocus = 1 << 13,  // Disable bringing window to front when taking focus (e.g. clicking on it or programmatically giving it focus)
            AlwaysVerticalScrollbar = 1 << 14,  // Always show vertical scrollbar (even if ContentSize.y < Size.y)
            AlwaysHorizontalScrollbar = 1 << 15,  // Always show horizontal scrollbar (even if ContentSize.x < Size.x)
            NoNavInputs = 1 << 16,  // No keyboard/gamepad navigation within the window
            NoNavFocus = 1 << 17,  // No focusing toward this window with keyboard/gamepad navigation (e.g. skipped by CTRL+TAB)
            UnsavedDocument = 1 << 18,  // Display a dot next to the title. When used in a tab/docking context, tab is selected when clicking the X + closure is not assumed (will wait for user to stop submitting the tab). Otherwise closure is assumed when pressing the X, so if you keep submitting the tab may reappear at end of tab bar.
            NoDocking = 1 << 19,  // Disable docking of this window
            NoNav = NoNavInputs | NoNavFocus,
            NoDecoration = NoTitleBar | NoResize | NoScrollbar | NoCollapse,
            NoInputs = NoMouseInputs | NoNavInputs | NoNavFocus,
        }
        [Flags] public enum ChildFlags : int
        {
            None = 0,
            Borders = 1 << 0,   // Show an outer border and enable WindowPadding. (IMPORTANT: this is always == 1 == true for legacy reason)
            AlwaysUseWindowPadding = 1 << 1,   // Pad with style.WindowPadding even if no border are drawn (no padding by default for non-bordered child windows because it makes more sense)
            ResizeX = 1 << 2,   // Allow resize from right border (layout direction). Enable .ini saving (unless ImGuiWindowFlags_NoSavedSettings passed to window flags)
            ResizeY = 1 << 3,   // Allow resize from bottom border (layout direction). "
            AutoResizeX = 1 << 4,   // Enable auto-resizing width. Read "IMPORTANT: Size measurement" details above.
            AutoResizeY = 1 << 5,   // Enable auto-resizing height. Read "IMPORTANT: Size measurement" details above.
            AlwaysAutoResize = 1 << 6,   // Combined with AutoResizeX/AutoResizeY. Always measure size even when child is hidden, always return true, always disable clipping optimization! NOT RECOMMENDED.
            FrameStyle = 1 << 7,   // Style the child window like a framed item: use FrameBg, FrameRounding, FrameBorderSize, FramePadding instead of ChildBg, ChildRounding, ChildBorderSize, WindowPadding.
            NavFlattened = 1 << 8,   // [BETA] Share focus scope, allow keyboard/gamepad navigation to cross over parent border to this child or between sibling child windows.
        }
        [Flags] public enum FocusedFlags : int
        {
            None = 0,
            ChildWindows = 1 << 0,   // Return true if any children of the window is focused
            RootWindow = 1 << 1,   // Test from root window (top most parent of the current hierarchy)
            AnyWindow = 1 << 2,   // Return true if any window is focused. Important: If you are trying to tell how to dispatch your low-level inputs, do NOT use this. Use 'io.WantCaptureMouse' instead! Please read the FAQ!
            NoPopupHierarchy = 1 << 3,   // Do not consider popup hierarchy (do not treat popup emitter as parent of popup) (when used with _ChildWindows or _RootWindow)
            DockHierarchy = 1 << 4,   // Consider docking hierarchy (treat dockspace host as parent of docked window) (when used with _ChildWindows or _RootWindow)
            RootAndChildWindows = RootWindow | ChildWindows,
        };
        [Flags] public enum HoveredFlags : int
        {
            None = 0,        // Return true if directly over the item/window, not obstructed by another window, not obstructed by an active popup or modal blocking inputs under them.
            ChildWindows = 1 << 0,   // IsWindowHovered() only: Return true if any children of the window is hovered
            RootWindow = 1 << 1,   // IsWindowHovered() only: Test from root window (top most parent of the current hierarchy)
            AnyWindow = 1 << 2,   // IsWindowHovered() only: Return true if any window is hovered
            NoPopupHierarchy = 1 << 3,   // IsWindowHovered() only: Do not consider popup hierarchy (do not treat popup emitter as parent of popup) (when used with _ChildWindows or _RootWindow)
            DockHierarchy = 1 << 4,   // IsWindowHovered() only: Consider docking hierarchy (treat dockspace host as parent of docked window) (when used with _ChildWindows or _RootWindow)
            AllowWhenBlockedByPopup = 1 << 5,   // Return true even if a popup window is normally blocking access to this item/window
                                                                  //AllowWhenBlockedByModal     = 1 << 6,   // Return true even if a modal popup window is normally blocking access to this item/window. FIXME-TODO: Unavailable yet.
            AllowWhenBlockedByActiveItem = 1 << 7,   // Return true even if an active item is blocking access to this item/window. Useful for Drag and Drop patterns.
            AllowWhenOverlappedByItem = 1 << 8,   // IsItemHovered() only: Return true even if the item uses AllowOverlap mode and is overlapped by another hoverable item.
            AllowWhenOverlappedByWindow = 1 << 9,   // IsItemHovered() only: Return true even if the position is obstructed or overlapped by another window.
            AllowWhenDisabled = 1 << 10,  // IsItemHovered() only: Return true even if the item is disabled
            NoNavOverride = 1 << 11,  // IsItemHovered() only: Disable using keyboard/gamepad navigation state when active, always query mouse
            AllowWhenOverlapped = AllowWhenOverlappedByItem | AllowWhenOverlappedByWindow,
            RectOnly = AllowWhenBlockedByPopup | AllowWhenBlockedByActiveItem | AllowWhenOverlapped,
            RootAndChildWindows = RootWindow | ChildWindows,

            // Tooltips mode
            // - typically used in IsItemHovered() + SetTooltip() sequence.
            // - this is a shortcut to pull flags from 'style.HoverFlagsForTooltipMouse' or 'style.HoverFlagsForTooltipNav' where you can reconfigure desired behavior.
            //   e.g. 'TooltipHoveredFlagsForMouse' defaults to 'Stationary | DelayShort'.
            // - for frequently actioned or hovered items providing a tooltip, you want may to use ForTooltip (stationary + delay) so the tooltip doesn't show too often.
            // - for items which main purpose is to be hovered, or items with low affordance, or in less consistent apps, prefer no delay or shorter delay.
            ForTooltip = 1 << 12,  // Shortcut for standard flags when using IsItemHovered() + SetTooltip() sequence.

            // (Advanced) Mouse Hovering delays.
            // - generally you can use ForTooltip to use application-standardized flags.
            // - use those if you need specific overrides.
            Stationary = 1 << 13,  // Require mouse to be stationary for style.HoverStationaryDelay (~0.15 sec) _at least one time_. After this, can move on same item/window. Using the stationary test tends to reduces the need for a long delay.
            DelayNone = 1 << 14,  // IsItemHovered() only: Return true immediately (default). As this is the default you generally ignore this.
            DelayShort = 1 << 15,  // IsItemHovered() only: Return true after style.HoverDelayShort elapsed (~0.15 sec) (shared between items) + requires mouse to be stationary for style.HoverStationaryDelay (once per item).
            DelayNormal = 1 << 16,  // IsItemHovered() only: Return true after style.HoverDelayNormal elapsed (~0.40 sec) (shared between items) + requires mouse to be stationary for style.HoverStationaryDelay (once per item).
            NoSharedDelay = 1 << 17,  // IsItemHovered() only: Disable shared delay system where moving from one item to the next keeps the previous timer for a short time (standard for tooltips with long delays)
        };
        [Flags] public enum TabBarFlags : int
        {
            None = 0,
            Reorderable = 1 << 0,   // Allow manually dragging tabs to re-order them + New tabs are appended at the end of list
            AutoSelectNewTabs = 1 << 1,   // Automatically select new tabs when they appear
            TabListPopupButton = 1 << 2,   // Disable buttons to open the tab list popup
            NoCloseWithMiddleMouseButton = 1 << 3,   // Disable behavior of closing tabs (that are submitted with p_open != NULL) with middle mouse button. You may handle this behavior manually on user's side with if (IsItemHovered() && IsMouseClicked(2)) *p_open = false.
            NoTabListScrollingButtons = 1 << 4,   // Disable scrolling buttons (apply when fitting policy is ImGuiTabBarFlags_FittingPolicyScroll)
            NoTooltip = 1 << 5,   // Disable tooltips when hovering a tab
            DrawSelectedOverline = 1 << 6,   // Draw selected overline markers over selected tab
            FittingPolicyResizeDown = 1 << 7,   // Resize tabs when they don't fit
            FittingPolicyScroll = 1 << 8,   // Add scroll buttons when tabs don't fit
            FittingPolicyMask_ = FittingPolicyResizeDown | FittingPolicyScroll,
            FittingPolicyDefault_ = FittingPolicyResizeDown,
        };
        [Flags] public enum TabItemFlags
        {
            None = 0,
            UnsavedDocument = 1 << 0,   // Display a dot next to the title + set NoAssumedClosure.
            SetSelected = 1 << 1,   // Trigger flag to programmatically make the tab selected when calling BeginTabItem()
            NoCloseWithMiddleMouseButton = 1 << 2,   // Disable behavior of closing tabs (that are submitted with p_open != NULL) with middle mouse button. You may handle this behavior manually on user's side with if (IsItemHovered() && IsMouseClicked(2)) *p_open = false.
            NoPushId = 1 << 3,   // Don't call PushID()/PopID() on BeginTabItem()/EndTabItem()
            NoTooltip = 1 << 4,   // Disable tooltip for the given tab
            NoReorder = 1 << 5,   // Disable reordering this tab or having another tab cross over this tab
            Leading = 1 << 6,   // Enforce the tab position to the left of the tab bar (after the tab list popup button)
            Trailing = 1 << 7,   // Enforce the tab position to the right of the tab bar (before the scrolling buttons)
            NoAssumedClosure = 1 << 8,   // Tab is selected when trying to close + closure is not immediately assumed (will wait for user to stop submitting the tab). Otherwise closure is assumed when pressing the X, so if you keep submitting the tab may reappear at end of tab bar.
        };
        [Flags] public enum Cond : int
        {
            None = 0,        // No condition (always set the variable), same as _Always
            Always = 1 << 0,   // No condition (always set the variable), same as _None
            Once = 1 << 1,   // Set the variable once per runtime session (only the first call will succeed)
            FirstUseEver = 1 << 2,   // Set the variable if the object/window has no persistently saved data (no entry in .ini file)
            Appearing = 1 << 3,   // Set the variable if the object/window is appearing after being hidden/inactive (or the first time)
        };
        [Flags] public enum SliderFlags : int
        {
            None = 0,
            Logarithmic = 1 << 5,       // Make the widget logarithmic (linear otherwise). Consider using ImGuiSliderFlags_NoRoundToFormat with this if using a format-string with small amount of digits.
            NoRoundToFormat = 1 << 6,       // Disable rounding underlying value to match precision of the display format string (e.g. %.3f values are rounded to those 3 digits).
            NoInput = 1 << 7,       // Disable CTRL+Click or Enter key allowing to input text directly into the widget.
            WrapAround = 1 << 8,       // Enable wrapping around from max to min and from min to max. Only supported by DragXXX() functions for now.
            ClampOnInput = 1 << 9,       // Clamp value to min/max bounds when input manually with CTRL+Click. By default CTRL+Click allows going out of bounds.
            ClampZeroRange = 1 << 10,      // Clamp even if min==max==0.0f. Otherwise due to legacy reason DragXXX functions don't clamp with those values. When your clamping limits are dynamic you almost always want to use it.
            NoSpeedTweaks = 1 << 11,      // Disable keyboard modifiers altering tweak speed. Useful if you want to alter tweak speed yourself based on your own logic.
            AlwaysClamp = ClampOnInput | ClampZeroRange,
            InvalidMask_ = 0x7000000F,   // [Internal] We treat using those bits as being potentially a 'float power' argument from the previous API that has got miscast to this enum, and will trigger an assert if needed.
        };
        [Flags] public enum ButtonFlags : int
        {
            None = 0,
            MouseButtonLeft = 1 << 0,   // React on left mouse button (default)
            MouseButtonRight = 1 << 1,   // React on right mouse button
            MouseButtonMiddle = 1 << 2,   // React on center mouse button
            MouseButtonMask_ = MouseButtonLeft | MouseButtonRight | MouseButtonMiddle, // [Internal]
            EnableNav = 1 << 3,   // InvisibleButton(): do not disable navigation/tabbing. Otherwise disabled by default.
        };
        [Flags] public enum ComboFlags : int
        {
            None = 0,
            PopupAlignLeft = 1 << 0,   // Align the popup toward the left by default
            HeightSmall = 1 << 1,   // Max ~4 items visible. Tip: If you want your combo popup to be a specific size you can use SetNextWindowSizeConstraints() prior to calling BeginCombo()
            HeightRegular = 1 << 2,   // Max ~8 items visible (default)
            HeightLarge = 1 << 3,   // Max ~20 items visible
            HeightLargest = 1 << 4,   // As many fitting items as possible
            NoArrowButton = 1 << 5,   // Display on the preview box without the square arrow button
            NoPreview = 1 << 6,   // Display only a square arrow button
            WidthFitPreview = 1 << 7,   // Width dynamically calculated from preview contents
            HeightMask_ = HeightSmall | HeightRegular | HeightLarge | HeightLargest,
        };
        [Flags] public enum ConfigFlags : int
        {
            None = 0,
            NavEnableKeyboard = 1 << 0,   // Master keyboard navigation enable flag. Enable full Tabbing + directional arrows + space/enter to activate.
            NavEnableGamepad = 1 << 1,   // Master gamepad navigation enable flag. Backend also needs to set ImGuiBackendFlags_HasGamepad.
            NoMouse = 1 << 4,   // Instruct dear imgui to disable mouse inputs and interactions.
            NoMouseCursorChange = 1 << 5,   // Instruct backend to not alter mouse cursor shape and visibility. Use if the backend cursor changes are interfering with yours and you don't want to use SetMouseCursor() to change mouse cursor. You may want to honor requests from imgui by reading GetMouseCursor() yourself instead.
            NoKeyboard = 1 << 6,   // Instruct dear imgui to disable keyboard inputs and interactions. This is done by ignoring keyboard events and clearing existing states.

            // User storage (to allow your backend/engine to communicate to code that may be shared between multiple projects. Those flags are NOT used by core Dear ImGui)
            IsSRGB = 1 << 20,  // Application is SRGB-aware.
            IsTouchScreen = 1 << 21,  // Application is using a touch screen instead of a mouse.
        };
        [Flags] public enum BackendFlags : int
        {
            None = 0,
            HasGamepad = 1 << 0,   // Backend Platform supports gamepad and currently has one connected.
            HasMouseCursors = 1 << 1,   // Backend Platform supports honoring GetMouseCursor() value to change the OS cursor shape.
            HasSetMousePos = 1 << 2,   // Backend Platform supports io.WantSetMousePos requests to reposition the OS mouse position (only used if io.ConfigNavMoveSetMousePos is set).
            RendererHasVtxOffset = 1 << 3,   // Backend Renderer supports ImDrawCmd::VtxOffset. This enables output of large meshes (64K+ vertices) while still using 16-bit indices.
        };
        [Flags] public enum InputTextFlags : int
        {
            // Basic filters (also see ImGuiInputTextFlags_CallbackCharFilter)
            None = 0,
            CharsDecimal = 1 << 0,   // Allow 0123456789.+-*/
            CharsHexadecimal = 1 << 1,   // Allow 0123456789ABCDEFabcdef
            CharsScientific = 1 << 2,   // Allow 0123456789.+-*/eE (Scientific notation input)
            CharsUppercase = 1 << 3,   // Turn a..z into A..Z
            CharsNoBlank = 1 << 4,   // Filter out spaces, tabs

            // Inputs
            AllowTabInput = 1 << 5,   // Pressing TAB input a '\t' character into the text field
            EnterReturnsTrue = 1 << 6,   // Return 'true' when Enter is pressed (as opposed to every time the value was modified). Consider using IsItemDeactivatedAfterEdit() instead!
            EscapeClearsAll = 1 << 7,   // Escape key clears content if not empty, and deactivate otherwise (contrast to default behavior of Escape to revert)
            CtrlEnterForNewLine = 1 << 8,   // In multi-line mode, validate with Enter, add new line with Ctrl+Enter (default is opposite: validate with Ctrl+Enter, add line with Enter).

            // Other options
            ReadOnly = 1 << 9,   // Read-only mode
            Password = 1 << 10,  // Password mode, display all characters as '*', disable copy
            AlwaysOverwrite = 1 << 11,  // Overwrite mode
            AutoSelectAll = 1 << 12,  // Select entire text when first taking mouse focus
            ParseEmptyRefVal = 1 << 13,  // InputFloat(), InputInt(), InputScalar() etc. only: parse empty string as zero value.
            DisplayEmptyRefVal = 1 << 14,  // InputFloat(), InputInt(), InputScalar() etc. only: when value is zero, do not display it. Generally used with ParseEmptyRefVal.
            NoHorizontalScroll = 1 << 15,  // Disable following the cursor horizontally
            NoUndoRedo = 1 << 16,  // Disable undo/redo. Note that input text owns the text data while active, if you want to provide your own undo/redo stack you need e.g. to call ClearActiveID().

            // Elide display / Alignment
            ElideLeft = 1 << 17,    // When text doesn't fit, elide left side to ensure right side stays visible. Useful for path/filenames. Single-line only!

            // Callback features
            CallbackCompletion = 1 << 18,  // Callback on pressing TAB (for completion handling)
            CallbackHistory = 1 << 19,  // Callback on pressing Up/Down arrows (for history handling)
            CallbackAlways = 1 << 20,  // Callback on each iteration. User code may query cursor position, modify text buffer.
            CallbackCharFilter = 1 << 21,  // Callback on character inputs to replace or discard them. Modify 'EventChar' to replace or discard, or return 1 in callback to discard.
            CallbackResize = 1 << 22,  // Callback on buffer capacity changes request (beyond 'buf_size' parameter value), allowing the string to grow. Notify when the string wants to be resized (for string types which hold a cache of their Size). You will be provided a new BufSize in the callback and NEED to honor it. (see misc/cpp/imgui_stdlib.h for an example of using this)
            CallbackEdit = 1 << 23,  // Callback on any edit. Note that InputText() already returns true on edit + you can always use IsItemEdited(). The callback is useful to manipulate the underlying buffer while focus is active.
        };
        [Flags] public enum ColorEditFlags : int
        {
            None = 0,
            NoAlpha = 1 << 1,   //              // ColorEdit, ColorPicker, ColorButton: ignore Alpha component (will only read 3 components from the input pointer).
            NoPicker = 1 << 2,   //              // ColorEdit: disable picker when clicking on color square.
            NoOptions = 1 << 3,   //              // ColorEdit: disable toggling options menu when right-clicking on inputs/small preview.
            NoSmallPreview = 1 << 4,   //              // ColorEdit, ColorPicker: disable color square preview next to the inputs. (e.g. to show only the inputs)
            NoInputs = 1 << 5,   //              // ColorEdit, ColorPicker: disable inputs sliders/text widgets (e.g. to show only the small preview color square).
            NoTooltip = 1 << 6,   //              // ColorEdit, ColorPicker, ColorButton: disable tooltip when hovering the preview.
            NoLabel = 1 << 7,   //              // ColorEdit, ColorPicker: disable display of inline text label (the label is still forwarded to the tooltip and picker).
            NoSidePreview = 1 << 8,   //              // ColorPicker: disable bigger color preview on right side of the picker, use small color square preview instead.
            NoDragDrop = 1 << 9,   //              // ColorEdit: disable drag and drop target. ColorButton: disable drag and drop source.
            NoBorder = 1 << 10,  //              // ColorButton: disable border (which is enforced by default)

            // User Options (right-click on widget to change some of them).
            AlphaBar = 1 << 16,  //              // ColorEdit, ColorPicker: show vertical alpha bar/gradient in picker.
            AlphaPreview = 1 << 17,  //              // ColorEdit, ColorPicker, ColorButton: display preview as a transparent color over a checkerboard, instead of opaque.
            AlphaPreviewHalf = 1 << 18,  //              // ColorEdit, ColorPicker, ColorButton: display half opaque / half checkerboard, instead of opaque.
            HDR = 1 << 19,  //              // (WIP) ColorEdit: Currently only disable 0.0f..1.0f limits in RGBA edition (note: you probably want to use Float flag as well).
            DisplayRGB = 1 << 20,  // [Display]    // ColorEdit: override _display_ type among RGB/HSV/Hex. ColorPicker: select any combination using one or more of RGB/HSV/Hex.
            DisplayHSV = 1 << 21,  // [Display]    // "
            DisplayHex = 1 << 22,  // [Display]    // "
            Uint8 = 1 << 23,  // [DataType]   // ColorEdit, ColorPicker, ColorButton: _display_ values formatted as 0..255.
            Float = 1 << 24,  // [DataType]   // ColorEdit, ColorPicker, ColorButton: _display_ values formatted as 0.0f..1.0f floats instead of 0..255 integers. No round-trip of value via integers.
            PickerHueBar = 1 << 25,  // [Picker]     // ColorPicker: bar for Hue, rectangle for Sat/Value.
            PickerHueWheel = 1 << 26,  // [Picker]     // ColorPicker: wheel for Hue, triangle for Sat/Value.
            InputRGB = 1 << 27,  // [Input]      // ColorEdit, ColorPicker: input and output data in RGB format.
            InputHSV = 1 << 28,  // [Input]      // ColorEdit, ColorPicker: input and output data in HSV format.

            // Defaults Options. You can set application defaults using SetColorEditOptions(). The intent is that you probably don't want to
            // override them in most of your calls. Let the user choose via the option menu and/or call SetColorEditOptions() once during startup.
            DefaultOptions_ = Uint8 | DisplayRGB | InputRGB | PickerHueBar,

            // [Internal] Masks
            DisplayMask_ = DisplayRGB | DisplayHSV | DisplayHex,
            DataTypeMask_ = Uint8 | Float,
            PickerMask_ = PickerHueWheel | PickerHueBar,
            InputMask_ = InputRGB | InputHSV,

            // Obsolete names
            //RGB = DisplayRGB, HSV = DisplayHSV, HEX = DisplayHex  // [renamed in 1.69]
        };
        [Flags] public enum SelectableFlags : int
        {
            None = 0,
            NoAutoClosePopups = 1 << 0,   // Clicking this doesn't close parent popup window (overrides ImGuiItemFlags_AutoClosePopups)
            SpanAllColumns = 1 << 1,   // Frame will span all columns of its container table (text will still fit in current column)
            AllowDoubleClick = 1 << 2,   // Generate press events on double clicks too
            Disabled = 1 << 3,   // Cannot be selected, display grayed out text
            AllowOverlap = 1 << 4,   // (WIP) Hit testing to allow subsequent widgets to overlap this one
            Highlight = 1 << 5,   // Make the item be displayed as if it is hovered
        };


        public enum Dir : int
        {
            None = -1,
            Left = 0,
            Right = 1,
            Up = 2,
            Down = 3,
            COUNT
        };
        public enum ImGuiMouseSource : int
        {
            ImGuiMouseSource_Mouse = 0,         // Input is coming from an actual mouse.
            ImGuiMouseSource_TouchScreen,       // Input is coming from a touch screen (no hovering prior to initial press, less precise initial press aiming, dual-axis wheeling possible).
            ImGuiMouseSource_Pen,               // Input is coming from a pressure/magnetic pen (often used in conjunction with high-sampling rates).
            ImGuiMouseSource_COUNT
        };
        #endregion
    }
}
