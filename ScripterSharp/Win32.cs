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

        [DllImport("user32.dll")]
        public static extern void PostQuitMessage(int nExitCode);
        [DllImport("user32.dll")]
        public static extern nint DefWindowProcW(nint hWnd, uint msg, nint wParam, nint lParam);
    }
}
