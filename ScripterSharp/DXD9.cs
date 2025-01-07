using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharp
{
    public static class DXD9
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct D3DPRESENT_PARAMETERS
        {
            public uint BackBufferWidth;
            public uint BackBufferHeight;
            public uint BackBufferFormat;
            public uint BackBufferCount;
            
            public uint MultiSampleType;
            public uint MultiSampleQuality;
            
            public uint SwapEffect;
            public nint hDeviceWindow;
            public int Windowed;
            public int EnableAutoDepthStencil;
            public uint AutoDepthStencilFormat;
            public uint Flags;

            public uint FullScreen_RefreshRateInHz;
            public uint PresentationInterval;
        }


        [DllImport("d3d9.dll")]
        public static extern nint Direct3DCreate9(uint SDKVersion);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        public static unsafe extern int DXD9_CreateDevice(nint thing, uint Adapter, uint DeviceType, nint hFocusWindow, uint BehaviorFlags, D3DPRESENT_PARAMETERS* pPresentationParameters, nint* ppReturnedDeviceInterface);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        public static unsafe extern int DXD9_SetRenderState(nint thing, uint State, uint Value);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        public static unsafe extern int DXD9_Clear(nint thing, uint Count, nint pRects, uint Flags, uint Color, float Z, uint Stencil);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        public static unsafe extern int DXD9_BeginScene(nint thing);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        public static unsafe extern int DXD9_EndScene(nint thing);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        public static unsafe extern int DXD9_TestCooperativeLevel(nint thing);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        public static unsafe extern int DXD9_Reset(nint thing, D3DPRESENT_PARAMETERS* pPresentationParameters);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        public static unsafe extern int DXD9_Present(nint thing, nint pSourceRect, nint pDestRect, nint hDestWindowOverride, nint pDirtyRegion);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        public static unsafe extern int CleanupDeviceD3D(nint thing, nint thing2);
    }
}
