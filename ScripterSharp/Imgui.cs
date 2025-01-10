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
        // TODO: Drawlist, Viewport
        [StructLayout(LayoutKind.Sequential)]
        public struct ImVec2
        {
            public float x, y;

            public ImVec2(float _x, float _y)
            {
                x = _x;
                y = _y;
            }
        }
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
        }

        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowDemoWindow@ImGui@@YAXPEA_N@Z")]
        public static extern unsafe void ShowDemoWindow(bool* show = null);
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
        public static extern unsafe bool ImplDX9_Init(DXD9.LPDIRECT3DDEVICE9* device);
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
        public static extern unsafe bool Begin([MarshalAs(UnmanagedType.LPStr)] string name, bool* p_open = null, WindowFlags flags = WindowFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?End@ImGui@@YAXXZ")]
        public static extern unsafe void End();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Text@ImGui@@YAXPEBDZZ")]
        public static extern unsafe void Text([MarshalAs(UnmanagedType.LPStr)] string str);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?SmallButton@ImGui@@YA_NPEBD@Z")]
        public static extern unsafe bool SmallButton([MarshalAs(UnmanagedType.LPStr)] string label);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Button@ImGui@@YA_NPEBDAEBUImVec2@@@Z")]
        public static extern unsafe bool Button([MarshalAs(UnmanagedType.LPStr)] string label, in ImVec2 size = new());
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowMetricsWindow@ImGui@@YAXPEA_N@Z")]
        public static extern unsafe void ShowMetricsWindow(bool* p_open = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowDebugLogWindow@ImGui@@YAXPEA_N@Z")]
        public static extern unsafe void ShowDebugLogWindow(bool* p_open = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowIDStackToolWindow@ImGui@@YAXPEA_N@Z")]
        public static extern unsafe void ShowIDStackToolWindow(bool* p_open = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowAboutWindow@ImGui@@YAXPEA_N@Z")]
        public static extern unsafe void ShowAboutWindow(bool* p_open = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowStyleEditor@ImGui@@YAXPEAUImGuiStyle@@@Z")]
        public static extern unsafe bool ShowStyleEditor(void* _ref = null);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowStyleSelector@ImGui@@YA_NPEBD@Z")]
        public static extern unsafe void ShowStyleSelector([MarshalAs(UnmanagedType.LPStr)] string label);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowFontSelector@ImGui@@YAXPEBD@Z")]
        public static extern unsafe void ShowFontSelector([MarshalAs(UnmanagedType.LPStr)] string label);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ShowUserGuide@ImGui@@YAXXZ")]
        public static extern unsafe void ShowUserGuide();
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetVersion@ImGui@@YAPEBDXZ")]

        private static extern unsafe nint _GetVersion();
        private static string ImGuiVersionStr = "";
        public static string GetVersion()
        {
            if (string.IsNullOrEmpty(ImGuiVersionStr))
            {
                var _str = Marshal.PtrToStringAnsi(_GetVersion());
                if (_str != null)
                    ImGuiVersionStr = _str;
            };

            return ImGuiVersionStr;
        }


        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?StyleColorsLight@ImGui@@YAXPEAUImGuiStyle@@@Z")]
        public static extern unsafe void StyleColorsLight(nint dst = 0);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?StyleColorsClassic@ImGui@@YAXPEAUImGuiStyle@@@Z")]
        public static extern unsafe void StyleColorsClassic(nint dst = 0);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "?BeginChild@ImGui@@YA_NPEBDAEBUImVec2@@HH@Z")]
        public static extern unsafe bool BeginChild([MarshalAs(UnmanagedType.LPStr)]string str_id, ImVec2 size = new(), ChildFlags child_flags = ChildFlags.None, WindowFlags window_flags = WindowFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?BeginChild@ImGui@@YA_NIAEBUImVec2@@HH@Z")]
        public static extern unsafe bool BeginChild(uint id, ImVec2 size = new(), ChildFlags child_flags = ChildFlags.None, WindowFlags window_flags = WindowFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EndChild@ImGui@@YAXXZ")]
        public static extern unsafe void EndChild();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?IsWindowAppearing@ImGui@@YA_NXZ")]
        public static extern unsafe bool IsWindowAppearing();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?IsWindowCollapsed@ImGui@@YA_NXZ")]
        public static extern unsafe bool IsWindowCollapsed();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?IsWindowFocused@ImGui@@YA_NH@Z")]
        public static extern unsafe bool IsWindowFocused(FocusedFlags flags = FocusedFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?IsWindowHovered@ImGui@@YA_NH@Z")]
        public static extern unsafe bool IsWindowHovered(HoveredFlags flags = HoveredFlags.None);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetWindowDpiScale@ImGui@@YAMXZ")]
        public static extern unsafe float GetWindowDpiScale();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetWindowPos@ImGui@@YA?AUImVec2@@XZ")]
        public static extern unsafe ImVec2 GetWindowPos();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetWindowSize@ImGui@@YA?AUImVec2@@XZ")]
        public static extern unsafe ImVec2 GetWindowSize();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetWindowWidth@ImGui@@YAMXZ")]
        public static extern unsafe float GetWindowWidth();
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?GetWindowHeight@ImGui@@YAMXZ")]
        public static extern unsafe float GetWindowHeight();





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

    }
}
