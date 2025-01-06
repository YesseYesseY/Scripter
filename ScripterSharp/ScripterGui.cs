using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

// https://github.com/ocornut/imgui/blob/master/examples/example_win32_directx9/
namespace ScripterSharp
{
    public unsafe static class ScripterGui
    {
        static bool demoWindow = true;
        public static void Start()
        {
            // TODO: Move all of gui.h to c#
            ImGui.InitGui(Backend, ScripterWndProc);
        }

        private static nint ScripterWndProc(nint hWnd, uint msg, nint wParam, nint lParam)
        {
            if (ImGui.ImplWin32_WndProcHandler(hWnd, msg, wParam, lParam) != nint.Zero) return 1;

            switch (msg)
            {
                case 0x0005: // WM_SIZE
                    if (wParam == 1)
                        return 0;
                    return 0;
                case 0x0112: // WM_SYSCOMMAND
                    if ((wParam & 0xfff0) == 0xF100) // // Disable ALT application menu
                        return 0;
                    break;
                case 0x0002: //WM_DESTROY
                    Win32.PostQuitMessage(0);
                    return 0;
            }

            return Win32.DefWindowProcW(hWnd, msg, wParam, lParam);
        }

        private static void Backend()
        {
            ImGui.ImplDX9_NewFrame();
            ImGui.ImplWin32_NewFrame();
            ImGui.NewFrame();

            MainGui();

            ImGui.EndFrame();
        }

        private static void MainGui()
        {
            ImGui.ShowDemoWindow(ref demoWindow);
        }
    }
}
