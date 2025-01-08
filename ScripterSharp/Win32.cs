using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharp
{
    public static class Win32
    {
        public delegate nint WndProc(nint hWnd, uint msg, nint wParam, nint lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct WNDCLASSEXW
        {
            public int cbSize;
            public uint style;
            public WndProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public nint hInstance;
            public nint hIcon;
            public nint hCursor;
            public nint hbrBackground;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszMenuName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszClassName;
            public nint hIconSm;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public nint hwnd;
            public uint message;
            public ulong wParam;
            public long lParam;
            public uint time;
            public POINT pt;
        }

        [DllImport("user32.dll")]
        public static extern void PostQuitMessage(int nExitCode);
        [DllImport("user32.dll")]
        public static extern nint DefWindowProcW(nint hWnd, uint msg, nint wParam, nint lParam);
        [DllImport("user32.dll")]
        public static extern int ShowWindow(nint hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern int UpdateWindow(nint hWnd);
        [DllImport("user32.dll")]
        public static extern int DestroyWindow(nint hWnd);
        [DllImport("user32.dll")]
        public static extern int UnregisterClassW([MarshalAs(UnmanagedType.LPWStr)]string lpClassName, nint hInstance);
        [DllImport("user32.dll")]
        public static unsafe extern int PeekMessageW(ref MSG lpMsg, nint hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);
        [DllImport("user32.dll")]
        public static unsafe extern int TranslateMessage(ref MSG lpMsg);
        [DllImport("user32.dll")]
        public static unsafe extern long DispatchMessageW(ref MSG lpMsg);
        [DllImport("user32.dll")]
        public static unsafe extern int SetWindowLongW(nint hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        public static unsafe extern int SetWindowPos(nint hwnd, nint hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        public static extern nint RegisterClassExW(ref WNDCLASSEXW unnamedParam1); // Great choice of name microsoft
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern nint CreateWindowExW(uint dwExStyle, [MarshalAs(UnmanagedType.LPWStr)]string lpClassName, [MarshalAs(UnmanagedType.LPWStr)]string lpWindowName, uint dwStyle,
            int X, int Y, int nWidth, int nHeight, nint hWndParent, nint hMenu, nint hInstance, nint lpParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern nint GetModuleHandleW([MarshalAs(UnmanagedType.LPWStr)]string lpModuleName);
    }
}
