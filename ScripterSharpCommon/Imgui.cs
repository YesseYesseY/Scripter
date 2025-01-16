﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharpCommon
{
    public static unsafe class ImGui
    {
        // TODO: Drawlist, Viewport, SetNextWindowSizeConstraints
        // tab: Parameters stacks (shared)
        // tab: Parameters stacks (current window)
        // tab: Style read access
        // Cursor stuff
        // Scalar inputs

        // NOTE: If you're adding to this, use byte instead of bool.
        // If you're returning ImVec2 use void for return and do "void Example(ImVec2* ret);", probably same for ImVec4

        #region Native
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowDemoWindow@ImGui@@YAXPEA_N@Z")]
        private static extern void _ShowDemoWindow(byte* show = null);
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
        private static extern byte _ImplWin32_Init(nint hwnd);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ImGui_ImplDX9_Init@@YA_NPEAUIDirect3DDevice9@@@Z")]
        private static extern byte _ImplDX9_Init(DXD9.LPDIRECT3DDEVICE9* device);
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
        private static extern byte _ImplDX9_CreateDeviceObjects();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Begin@ImGui@@YA_NPEBDPEA_NH@Z")]
        private static extern byte _Begin( string name, byte* p_open = null, WindowFlags flags = WindowFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?End@ImGui@@YAXXZ")]
        public static extern void End();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Text@ImGui@@YAXPEBDZZ")]
        public static extern void Text( string str);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SmallButton@ImGui@@YA_NPEBD@Z")]
        private static extern byte _SmallButton(string label);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Button@ImGui@@YA_NPEBDAEBUImVec2@@@Z")]
        private static extern byte _Button(string label, in ImVec2 size = default);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowMetricsWindow@ImGui@@YAXPEA_N@Z")]
        private static extern void _ShowMetricsWindow(byte* p_open = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowDebugLogWindow@ImGui@@YAXPEA_N@Z")]
        private static extern void _ShowDebugLogWindow(byte* p_open = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowIDStackToolWindow@ImGui@@YAXPEA_N@Z")]
        private static extern void _ShowIDStackToolWindow(byte* p_open = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowAboutWindow@ImGui@@YAXPEA_N@Z")]
        private static extern void _ShowAboutWindow(byte* p_open = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowStyleEditor@ImGui@@YAXPEAUImGuiStyle@@@Z")]
        private static extern byte _ShowStyleEditor(void* _ref = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowStyleSelector@ImGui@@YA_NPEBD@Z")]
        public static extern void ShowStyleSelector( string label);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowFontSelector@ImGui@@YAXPEBD@Z")]
        public static extern void ShowFontSelector( string label);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowUserGuide@ImGui@@YAXXZ")]
        public static extern void ShowUserGuide();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetVersion@ImGui@@YAPEBDXZ")]
        private static extern nint _GetVersion();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?StyleColorsLight@ImGui@@YAXPEAUImGuiStyle@@@Z")]
        public static extern void StyleColorsLight(nint dst = 0);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?StyleColorsClassic@ImGui@@YAXPEAUImGuiStyle@@@Z")]
        public static extern void StyleColorsClassic(nint dst = 0);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?BeginChild@ImGui@@YA_NPEBDAEBUImVec2@@HH@Z")]
        private static extern byte _BeginChild(string str_id, in ImVec2 size = default, ChildFlags child_flags = ChildFlags.None, WindowFlags window_flags = WindowFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?BeginChild@ImGui@@YA_NIAEBUImVec2@@HH@Z")]
        private static extern byte _BeginChild(uint id, in ImVec2 size = default, ChildFlags child_flags = ChildFlags.None, WindowFlags window_flags = WindowFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EndChild@ImGui@@YAXXZ")]
        public static extern void EndChild();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?IsWindowAppearing@ImGui@@YA_NXZ")]
        private static extern byte _IsWindowAppearing();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?IsWindowCollapsed@ImGui@@YA_NXZ")]
        private static extern byte _IsWindowCollapsed();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?IsWindowFocused@ImGui@@YA_NH@Z")]
        private static extern byte _IsWindowFocused(FocusedFlags flags = FocusedFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?IsWindowHovered@ImGui@@YA_NH@Z")]
        private static extern byte _IsWindowHovered(HoveredFlags flags = HoveredFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetWindowDpiScale@ImGui@@YAMXZ")]
        public static extern float GetWindowDpiScale();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetWindowPos@ImGui@@YA?AUImVec2@@XZ")]
        public static extern void GetWindowPos(ImVec2* ret);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetWindowSize@ImGui@@YA?AUImVec2@@XZ")]
        public static extern void GetWindowSize(ImVec2* ret);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetWindowWidth@ImGui@@YAMXZ")]
        public static extern float GetWindowWidth();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetWindowHeight@ImGui@@YAMXZ")]
        public static extern float GetWindowHeight();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?BeginTabBar@ImGui@@YA_NPEBDH@Z")]
        private static extern byte _BeginTabBar(string str_id, TabBarFlags flags = TabBarFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EndTabBar@ImGui@@YAXXZ")]
        public static extern void EndTabBar();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?BeginTabItem@ImGui@@YA_NPEBDPEA_NH@Z")]
        private static extern byte _BeginTabItem(string label, byte* p_open = null, TabItemFlags flags = TabItemFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EndTabItem@ImGui@@YAXXZ")]
        public static extern void EndTabItem();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Checkbox@ImGui@@YA_NPEBDPEA_N@Z")]
        private static extern byte _Checkbox(string label, byte* v);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SameLine@ImGui@@YAXMM@Z")]
        public static extern void SameLine(float offset_from_start_x = 0.0f, float spacing = -1.0f);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetNextWindowPos@ImGui@@YAXAEBUImVec2@@H0@Z")]
        public static extern void SetNextWindowPos(in ImVec2 pos, Cond cond = Cond.None, in ImVec2 pivot = default);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetNextWindowSize@ImGui@@YAXAEBUImVec2@@H@Z")]
        public static extern void SetNextWindowSize(in ImVec2 size, Cond cond = Cond.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetNextWindowContentSize@ImGui@@YAXAEBUImVec2@@@Z")]
        public static extern void SetNextWindowContentSize(in ImVec2 size);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetNextWindowCollapsed@ImGui@@YAX_NH@Z")]
        private static extern void _SetNextWindowCollapsed(byte collapsed, Cond cond = Cond.None);
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
        private static extern void _SetWindowCollapsed(byte collapsed, Cond cond = Cond.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetWindowFocus@ImGui@@YAXXZ")]
        public static extern void SetWindowFocus();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetWindowFontScale@ImGui@@YAXM@Z")]
        public static extern void SetWindowFontScale(float scale);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetWindowPos@ImGui@@YAXPEBDAEBUImVec2@@H@Z")]
        public static extern void SetWindowPos(string name, in ImVec2 pos, Cond cond = Cond.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetWindowSize@ImGui@@YAXPEBDAEBUImVec2@@H@Z")]
        public static extern void SetWindowSize(string name, in ImVec2 size, Cond cond = Cond.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetWindowCollapsed@ImGui@@YAXPEBD_NH@Z")]
        private static extern void _SetWindowCollapsed(string name, byte collapsed, Cond cond = Cond.None);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetWindowFocus@ImGui@@YAXPEBD@Z")]
        public static extern void SetWindowFocus(string name);
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
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetCursorScreenPos@ImGui@@YA?AUImVec2@@XZ")]
        public static extern void GetCursorScreenPos(ImVec2* ret); // This is stupid
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SetCursorScreenPos@ImGui@@YAXAEBUImVec2@@@Z")]
        public static extern void SetCursorScreenPos(in ImVec2 pos);
        #endregion



        public static bool SmallButton(string label) => _SmallButton(label) != 0;
        public static bool Button(string label, in ImVec2 size = default) => _Button(label, size) != 0;
        public static bool ShowStyleEditor(void* _ref = null) => _ShowStyleEditor(_ref) != 0;
        public static bool ImplWin32_Init(nint hwnd) => _ImplWin32_Init(hwnd) != 0;
        public static bool ImplDX9_Init(DXD9.LPDIRECT3DDEVICE9* device) => _ImplDX9_Init(device) != 0;
        public static bool ImplDX9_CreateDeviceObjects() => _ImplDX9_CreateDeviceObjects() != 0;
        public static bool BeginChild(string str_id, in ImVec2 size = default, ChildFlags child_flags = ChildFlags.None, WindowFlags window_flags = WindowFlags.None) => _BeginChild(str_id, size, child_flags, window_flags) != 0;
        public static bool BeginChild(uint id, in ImVec2 size = default, ChildFlags child_flags = ChildFlags.None, WindowFlags window_flags = WindowFlags.None) => _BeginChild(id, size, child_flags, window_flags) != 0;
        public static bool IsWindowAppearing() => _IsWindowAppearing() != 0;
        public static bool IsWindowCollapsed() => _IsWindowCollapsed() != 0;
        public static bool IsWindowFocused(FocusedFlags flags = FocusedFlags.None) => _IsWindowFocused() != 0;
        public static bool IsWindowHovered(HoveredFlags flags = HoveredFlags.None) => _IsWindowHovered() != 0;
        public static bool BeginTabBar(string str_id, TabBarFlags flags = TabBarFlags.None) => _BeginTabBar(str_id, flags) != 0;
        public static void SetNextWindowCollapsed(bool collapsed, Cond cond = Cond.None) => _SetNextWindowCollapsed((byte)(collapsed == true ? 1 : 0), cond);
        public static void SetWindowCollapsed(bool collapsed, Cond cond = Cond.None) => _SetWindowCollapsed((byte)(collapsed == true ? 1 : 0), cond);
        public static void SetWindowCollapsed(string name, bool collapsed, Cond cond = Cond.None) => _SetWindowCollapsed(name, (byte)(collapsed == true ? 1 : 0), cond);


        #region Drag Sldiers
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
        public static ImVec2 GetCursorScreenPos()
        {
            ImVec2 ret;
            GetCursorScreenPos(&ret);
            return ret;
        }

        public static bool Begin(string name, bool* p_open = null, WindowFlags flags = WindowFlags.None)
        {
            byte val = 1;
            if (p_open is not null && *p_open is false)
                val = 0;
            var ret = _Begin(name, &val, flags) != 0;
            if (p_open is not null)
                *p_open = val != 0;
            return ret;
        }

        public static bool BeginTabItem(string label, bool* p_open = null, TabItemFlags flags = TabItemFlags.None)
        {
            byte val = 1;
            if (p_open is not null && *p_open is false)
                val = 0;
            var ret = _BeginTabItem(label, &val, flags) != 0;
            if (p_open is not null)
                *p_open = val != 0;
            return ret;
        }
        public static void ShowDemoWindow(bool* show = null)
        {
            byte val = 1;
            if (show is not null && *show is false)
                val = 0;
            _ShowDemoWindow(&val);
            if (show is not null)
                *show = val != 0;
        }

        public static bool Checkbox(string label, bool* v)
        {
            byte val = 1;
            if (v is not null && *v is false)
                val = 0;
            var ret = _Checkbox(label, &val) != 0;
            if (v is not null)
                *v = val != 0;
            return ret;
        }
        public static void ShowMetricsWindow(bool* p_open = null)
        {
            byte val = 1;
            if (p_open is not null && *p_open is false)
                val = 0;
            _ShowMetricsWindow(&val);
            if (p_open is not null)
                *p_open = val != 0;
        }
        public static void ShowDebugLogWindow(bool* p_open = null)
        {
            byte val = 1;
            if (p_open is not null && *p_open is false)
                val = 0;
            _ShowDebugLogWindow(&val);
            if (p_open is not null)
                *p_open = val != 0;
        }
        public static void ShowIDStackToolWindow(bool* p_open = null)
        {
            byte val = 1;
            if (p_open is not null && *p_open is false)
                val = 0;
            _ShowIDStackToolWindow(&val);
            if (p_open is not null)
                *p_open = val != 0;
        }
        public static void ShowAboutWindow(bool* p_open = null)
        {
            byte val = 1;
            if (p_open is not null && *p_open is false)
                val = 0;
            _ShowAboutWindow(&val);
            if (p_open is not null)
                *p_open = val != 0;
        }


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

        [StructLayout(LayoutKind.Sequential)]
        public struct ImVec2
        {
            public float x, y;

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
        [StructLayout(LayoutKind.Sequential)]
        public struct ImVec4
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

        [Flags]
        public enum WindowFlags : int
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
        [Flags]
        public enum ChildFlags : int
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
        [Flags]
        public enum FocusedFlags : int
        {
            None = 0,
            ChildWindows = 1 << 0,   // Return true if any children of the window is focused
            RootWindow = 1 << 1,   // Test from root window (top most parent of the current hierarchy)
            AnyWindow = 1 << 2,   // Return true if any window is focused. Important: If you are trying to tell how to dispatch your low-level inputs, do NOT use this. Use 'io.WantCaptureMouse' instead! Please read the FAQ!
            NoPopupHierarchy = 1 << 3,   // Do not consider popup hierarchy (do not treat popup emitter as parent of popup) (when used with _ChildWindows or _RootWindow)
            DockHierarchy = 1 << 4,   // Consider docking hierarchy (treat dockspace host as parent of docked window) (when used with _ChildWindows or _RootWindow)
            RootAndChildWindows = RootWindow | ChildWindows,
        };
        [Flags]
        public enum HoveredFlags : int
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

        [Flags]
        public enum TabBarFlags : int
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
        [Flags]
        public enum TabItemFlags
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

        [Flags]
        public enum Cond : int
        {
            None = 0,        // No condition (always set the variable), same as _Always
            Always = 1 << 0,   // No condition (always set the variable), same as _None
            Once = 1 << 1,   // Set the variable once per runtime session (only the first call will succeed)
            FirstUseEver = 1 << 2,   // Set the variable if the object/window has no persistently saved data (no entry in .ini file)
            Appearing = 1 << 3,   // Set the variable if the object/window is appearing after being hidden/inactive (or the first time)
        };
        [Flags]
        public enum SliderFlags : int
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
    }
}
