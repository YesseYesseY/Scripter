using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

// https://github.com/ocornut/imgui/blob/master/examples/example_win32_directx9/
namespace ScripterSharp
{
    public unsafe static class ScripterGui
    {
        static bool demoWindow = true;
        public static uint ToD3DColor(this Vector4 vec)
        {
            byte a = (byte)(Math.Clamp(vec.W, 0, 1) * 255);
            byte r = (byte)(Math.Clamp(vec.X, 0, 1) * 255);
            byte g = (byte)(Math.Clamp(vec.Y, 0, 1) * 255);
            byte b = (byte)(Math.Clamp(vec.Z, 0, 1) * 255);

            return (uint)((a << 24) | (r << 16) | (g << 8) | b);
        }

        private static Win32.WNDCLASSEXW wc;
        private static DXD9.D3DPRESENT_PARAMETERS g_d3dpp;
        private static nint hwnd;
        private static nint g_pD3D;
        private static nint g_pd3dDevice;
        private static void MainGui()
        {
            ImGui.ShowDemoWindow(ref demoWindow);

            if (ImGui.Begin("Test window :)"))
            {
                ImGui.Text("Welcome to imgui from c#");
                if (ImGui.Button("Test button"))
                {
                    Scripter.Print("\"Hello there\" -button 2024");
                }
                ImGui.End();
            }
        }

        // Under is no touchy zone (unless u know what ur doing)

        public static void Start() => Backend();

        private static void CleanupDeviceD3D()
        {
            DXD9.CleanupDeviceD3D(g_pd3dDevice, g_pD3D);
        }

        private static void ResetDevice()
        {
            ImGui.ImplDX9_InvalidateDeviceObjects();
            fixed (DXD9.D3DPRESENT_PARAMETERS* d3dppPtr = &g_d3dpp)
            {
                DXD9.DXD9_Reset(g_pd3dDevice, d3dppPtr);
            }
            ImGui.ImplDX9_CreateDeviceObjects();
        }

        private static unsafe bool CreateDeviceD3D()
        {
            g_pD3D = DXD9.Direct3DCreate9(32);
            if (g_pD3D == nint.Zero) return false;

            g_d3dpp = new DXD9.D3DPRESENT_PARAMETERS()
            {
                Windowed = 1,
                SwapEffect = 1,
                BackBufferFormat = 0,
                EnableAutoDepthStencil = 1,
                AutoDepthStencilFormat = 80,
                PresentationInterval = 0x00000001
            };

            fixed (DXD9.D3DPRESENT_PARAMETERS* d3dppPtr = &g_d3dpp)
            {
                fixed (nint* devicePtr = &g_pd3dDevice)
                {
                    if (DXD9.DXD9_CreateDevice(g_pD3D, 0, 1, hwnd, 0x00000040, d3dppPtr, devicePtr) < 0)
                        return false;
                }
            }
            
            return true;
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
            wc = new Win32.WNDCLASSEXW()
            {
                cbSize = Marshal.SizeOf(typeof(Win32.WNDCLASSEXW)),
                style = 0x0040,
                lpfnWndProc = ScripterWndProc,
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = Win32.GetModuleHandleW(null),
                hIcon = nint.Zero,
                hCursor = nint.Zero,
                hbrBackground = nint.Zero,
                lpszMenuName = null,
                lpszClassName = "Scripter Gui",
                hIconSm = nint.Zero,
            };
            Win32.RegisterClassExW(ref wc);
            hwnd = Win32.CreateWindowExW(0, wc.lpszClassName, "Scripter from C#", (uint)(0x00000000 | 0x00C00000 | 0x00080000 | 0x0004000L | 0x00020000 | 0x00010000), 100, 100, 1280, 800, nint.Zero, nint.Zero, wc.hInstance, nint.Zero);

            if (!CreateDeviceD3D())
            {
                CleanupDeviceD3D();
                Win32.UnregisterClassW(wc.lpszClassName, wc.hInstance);
                return;
            }

            Win32.ShowWindow(hwnd, 10);
            Win32.UpdateWindow(hwnd);

            ImGui.CreateContext();

            /*
                ImGuiIO& io = ImGui::GetIO();
                io.ConfigFlags |= ImGuiConfigFlags_NavEnableKeyboard;
                io.ConfigFlags |= ImGuiConfigFlags_NavEnableGamepad;
             */

            ImGui.StyleColorsDark();

            ImGui.ImplWin32_Init(hwnd);
            ImGui.ImplDX9_Init(g_pd3dDevice);

            bool g_DeviceLost = false;
            Vector4 clear_color = new Vector4(0.45f, 0.55f, 0.60f, 1.00f);

            bool done = false;
            while (!done)
            {
                Win32.MSG msg = new Win32.MSG();
                while (Win32.PeekMessageW(ref msg, nint.Zero, 0, 0, 0x0001) != 0)
                {
                    Win32.TranslateMessage(ref msg);
                    Win32.DispatchMessageW(ref msg);
                    if (msg.message == 0x0012)
                        done = true;
                }
                if (done)
                    break;

                if (g_DeviceLost)
                {
                    int hr = DXD9.DXD9_TestCooperativeLevel(g_pd3dDevice);
                    if (hr == -2005530520)
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    if (hr == -2005530519)
                        ResetDevice();
                    g_DeviceLost = false;
                }

                ImGui.ImplDX9_NewFrame();
                ImGui.ImplWin32_NewFrame();
                ImGui.NewFrame();

                MainGui();

                ImGui.EndFrame();

                DXD9.DXD9_SetRenderState(g_pd3dDevice, 7, 0);
                DXD9.DXD9_SetRenderState(g_pd3dDevice, 27, 0);
                DXD9.DXD9_SetRenderState(g_pd3dDevice, 174, 0);

                DXD9.DXD9_Clear(g_pd3dDevice, 0, nint.Zero, 0x00000001 | 0x00000002, clear_color.ToD3DColor(), 1.0f, 0);
                if (DXD9.DXD9_BeginScene(g_pd3dDevice) >= 0)
                {
                    ImGui.Render();
                    ImGui.ImplDX9_RenderDrawData(ImGui.GetDrawData());
                    DXD9.DXD9_EndScene(g_pd3dDevice);
                }
                var result = DXD9.DXD9_Present(g_pd3dDevice, nint.Zero, nint.Zero, nint.Zero, nint.Zero);
                if (result == -2005530520)
                    g_DeviceLost = true;

            }
            ImGui.ImplDX9_Shutdown();
            ImGui.ImplWin32_Shutdown();
            ImGui.DestroyContext();

            CleanupDeviceD3D();
            Win32.DestroyWindow(hwnd);
            Win32.UnregisterClassW(wc.lpszClassName, wc.hInstance);
        }
    }
}
