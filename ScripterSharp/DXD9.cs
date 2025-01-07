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

        [StructLayout(LayoutKind.Sequential)]
        public struct LPDIRECT3D9
        {
            private unsafe nint GetPtr()
            {
                fixed (void* ptr = &this)
                {
                    return (nint)ptr;
                }
            }

            public unsafe int CreateDevice(uint Adapter, uint DeviceType, nint hFocusWindow, uint BehaviorFlags, D3DPRESENT_PARAMETERS* pPresentationParameters, LPDIRECT3DDEVICE9** ppReturnedDeviceInterface) =>
                DXD9_CreateDevice(GetPtr(), Adapter, DeviceType, hFocusWindow, BehaviorFlags, pPresentationParameters, ppReturnedDeviceInterface);
            public unsafe void Release() => DXD9_ReleaseThing((LPDIRECT3D9*)GetPtr());
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LPDIRECT3DDEVICE9
        {
            private unsafe nint GetPtr()
            {
                fixed (void* ptr = &this)
                {
                    return (nint)ptr;
                }
            }

            public unsafe int SetRenderState(uint State, uint Value) => DXD9_SetRenderState(GetPtr(), State, Value);
            public unsafe int Clear(uint Count, nint pRects, uint Flags, uint Color, float Z, uint Stencil) => DXD9_Clear(GetPtr(), Count, pRects, Flags, Color, Z, Stencil);
            public unsafe int BeginScene() => DXD9_BeginScene(GetPtr());
            public unsafe int EndScene() => DXD9_EndScene(GetPtr());
            public unsafe int TestCooperativeLevel() => DXD9_TestCooperativeLevel(GetPtr());
            public unsafe int Reset(D3DPRESENT_PARAMETERS* pPresentationParameters) => DXD9_Reset(GetPtr(), pPresentationParameters);
            public unsafe int Present(nint pSourceRect, nint pDestRect, nint hDestWindowOverride, nint pDirtyRegion) => DXD9_Present(GetPtr(), pSourceRect, pDestRect, hDestWindowOverride, pDirtyRegion);
            public unsafe void Release() => DXD9_ReleaseDevice((LPDIRECT3DDEVICE9*)GetPtr());
        }

        [DllImport("d3d9.dll")]
        public static unsafe extern LPDIRECT3D9* Direct3DCreate9(uint SDKVersion);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        private static unsafe extern int DXD9_CreateDevice(nint thing, uint Adapter, uint DeviceType, nint hFocusWindow, uint BehaviorFlags, D3DPRESENT_PARAMETERS* pPresentationParameters, LPDIRECT3DDEVICE9** ppReturnedDeviceInterface);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        private static unsafe extern int DXD9_SetRenderState(nint device, uint State, uint Value);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        private static unsafe extern int DXD9_Clear(nint device, uint Count, nint pRects, uint Flags, uint Color, float Z, uint Stencil);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        private static unsafe extern int DXD9_BeginScene(nint device);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        private static unsafe extern int DXD9_EndScene(nint device);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        private static unsafe extern int DXD9_TestCooperativeLevel(nint device);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        private static unsafe extern int DXD9_Reset(nint device, D3DPRESENT_PARAMETERS* pPresentationParameters);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        private static unsafe extern int DXD9_Present(nint device, nint pSourceRect, nint pDestRect, nint hDestWindowOverride, nint pDirtyRegion);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        private static unsafe extern int DXD9_ReleaseDevice(LPDIRECT3DDEVICE9* device);
        [DllImport("Scripter.dll", CallingConvention = CallingConvention.StdCall)]
        private static unsafe extern int DXD9_ReleaseThing(LPDIRECT3D9* device);
    }
}
