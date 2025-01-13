using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ScripterSharpCommon;
using ScripterSharpCommon.UE;

// https://github.com/ocornut/imgui/blob/master/examples/example_win32_directx9/
namespace ScripterSharp
{
    public unsafe static class ScripterGui
    {
        private static void MainGui()
        {
            ImGui.ShowDemoWindow();

            if (ImGui.Begin("Module window"))
            {
                if (ImGui.BeginTabBar("Module tab bar"))
                {
                    foreach (var mod in Scripter.modules)
                    {
                        if (ImGui.BeginTabItem(mod.Name))
                        {
                            mod.OnGui();
                            ImGui.EndTabItem();
                        }
                    }

                    ImGui.EndTabBar();
                }
            }
            ImGui.End();

            if (ImGui.Begin("Window thingy"))
            {
                if (ImGui.Button("Dump objects"))
                {
                    string objFilePath = "C:/FN/obj.txt";
                    Logger.Log($"Writing ~{UObject.Objects.Num} objects to {objFilePath}");
                    using (var stream = File.OpenWrite(objFilePath))
                    {
                        foreach (UObject* obj in UObject.Objects)
                        {
                            stream.Write(Encoding.UTF8.GetBytes($"{obj->GetFullName()}\n"));
                        }
                    }
                    Logger.Log("Finished!");
                }
                if (ImGui.Button("Advanced dump"))
                {
                    AdvancedDumper.Dump();
                }
            }
            ImGui.End();
        }

        private static void TestGui()
        {
            if (ImGui.Begin("Test window :)"))
            {
                ImGui.Text("Welcome to imgui from c# " + ImGui.GetVersion());
                if (ImGui.Button("Test button"))
                {
                    Logger.Log("\"Hello there\" -button 2024");
                }
                if (ImGui.SmallButton("Test small button"))
                {
                    Logger.Log("\"Hello there\" -small button 2024");
                }
                ImGui.End();
            }
        }

        // Under is no touchy zone (unless u know what ur doing)

        public static uint ToD3DColor(this Vector4 vec)
        {
            byte a = (byte)(Math.Clamp(vec.W, 0, 1) * 255);
            byte r = (byte)(Math.Clamp(vec.X, 0, 1) * 255);
            byte g = (byte)(Math.Clamp(vec.Y, 0, 1) * 255);
            byte b = (byte)(Math.Clamp(vec.Z, 0, 1) * 255);

            return (uint)((a << 24) | (r << 16) | (g << 8) | b);
        }

        private static Win32.WNDCLASSEX wc;
        private static DXD9.D3DPRESENT_PARAMETERS g_d3dpp;
        private static nint hwnd;
        private static DXD9.LPDIRECT3D9* g_pD3D;
        private static DXD9.LPDIRECT3DDEVICE9* g_pd3dDevice;


        public static void Start() => Backend();

        private static void CleanupDeviceD3D()
        {
            g_pd3dDevice->Release();
            g_pD3D->Release();
        }

        private static void ResetDevice()
        {
            ImGui.ImplDX9_InvalidateDeviceObjects();
            fixed (DXD9.D3DPRESENT_PARAMETERS* d3dppPtr = &g_d3dpp)
            {
                g_pd3dDevice->Reset(d3dppPtr);
            }
            ImGui.ImplDX9_CreateDeviceObjects();
        }

        private static unsafe bool CreateDeviceD3D()
        {
            g_pD3D = DXD9.Direct3DCreate9(32);
            if (g_pD3D is null) return false;

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
                fixed (DXD9.LPDIRECT3DDEVICE9** devicePtr = &g_pd3dDevice)
                {
                    if (g_pD3D->CreateDevice(0, 1, hwnd, 0x00000040, d3dppPtr, devicePtr) < 0)
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

            return Win32.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        private static void Backend()
        {
            wc = new Win32.WNDCLASSEX()
            {
                cbSize = Marshal.SizeOf(typeof(Win32.WNDCLASSEX)),
                style = 0x0040,
                lpfnWndProc = ScripterWndProc,
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = Win32.GetModuleHandle(null),
                hIcon = nint.Zero,
                hCursor = nint.Zero,
                hbrBackground = nint.Zero,
                lpszMenuName = null,
                lpszClassName = "Scripter Gui",
                hIconSm = nint.Zero,
            };
            Win32.RegisterClassEx(ref wc);
            hwnd = Win32.CreateWindowEx(0, wc.lpszClassName, "Scripter from C#", 0, 100, 100, 1280, 800, nint.Zero, nint.Zero, wc.hInstance, nint.Zero);

            // Removes border, titlebar, etc
            // Win32.SetWindowLongW(hwnd, -16, 0);
            // Win32.SetWindowPos(hwnd, nint.Zero, 100, 100, 1280, 800, 0x0020);

            if (!CreateDeviceD3D())
            {
                CleanupDeviceD3D();
                Win32.UnregisterClass(wc.lpszClassName, wc.hInstance);
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
            Vector4 clear_color = new Vector4(0.45f, 0.55f, 0.60f, 0.0f);

            bool done = false;
            Win32.MSG msg = new Win32.MSG();
            while (!done)
            {
                while (Win32.PeekMessage(ref msg, nint.Zero, 0, 0, 0x0001) != 0)
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
                    int hr = g_pd3dDevice->TestCooperativeLevel();
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

                g_pd3dDevice->SetRenderState(7, 0);
                g_pd3dDevice->SetRenderState(27, 0);
                g_pd3dDevice->SetRenderState(174, 0);

                g_pd3dDevice->Clear(0, nint.Zero, 0x00000001 | 0x00000002, clear_color.ToD3DColor(), 1.0f, 0);
                if (g_pd3dDevice->BeginScene() >= 0)
                {
                    ImGui.Render();
                    ImGui.ImplDX9_RenderDrawData(ImGui.GetDrawData());
                    g_pd3dDevice->EndScene();
                }
                var result = g_pd3dDevice->Present(nint.Zero, nint.Zero, nint.Zero, nint.Zero);
                if (result == -2005530520)
                    g_DeviceLost = true;

            }
            ImGui.ImplDX9_Shutdown();
            ImGui.ImplWin32_Shutdown();
            ImGui.DestroyContext();

            CleanupDeviceD3D();
            Win32.DestroyWindow(hwnd);
            Win32.UnregisterClass(wc.lpszClassName, wc.hInstance);
        }
    }
}
