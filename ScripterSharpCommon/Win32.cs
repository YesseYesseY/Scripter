using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharpCommon
{
    public static class Win32
    {
        public delegate nint WndProc(nint hWnd, uint msg, nint wParam, nint lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct WNDCLASSEX
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
            public string? lpszMenuName;
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
        public struct IMAGE_FILE_HEADER
        {
            public ushort Machine;
            public ushort NumberOfSections;
            public uint TimeDateStamp;
            public uint PointerToSymbolTable;
            public uint NumberOfSymbols;
            public ushort SizeOfOptionalHeader;
            public ushort Characteristics;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_DATA_DIRECTORY
        {
            public uint VirtualAddress;
            public uint Size;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_OPTIONAL_HEADER64
        {
            public ushort Magic;
            public byte MajorLinkerVersion;
            public byte MinorLinkerVersion;
            public uint SizeOfCode;
            public uint SizeOfInitializedData;
            public uint SizeOfUninitializedData;
            public uint AddressOfEntryPoint;
            public uint BaseOfCode;
            public ulong ImageBase;
            public uint SectionAlignment;
            public uint FileAlignment;
            public ushort MajorOperatingSystemVersion;
            public ushort MinorOperatingSystemVersion;
            public ushort MajorImageVersion;
            public ushort MinorImageVersion;
            public ushort MajorSubsystemVersion;
            public ushort MinorSubsystemVersion;
            public uint Win32VersionValue;
            public uint SizeOfImage;
            public uint SizeOfHeaders;
            public uint CheckSum;
            public ushort Subsystem;
            public ushort DllCharacteristics;
            public ulong SizeOfStackReserve;
            public ulong SizeOfStackCommit;
            public ulong SizeOfHeapReserve;
            public ulong SizeOfHeapCommit;
            public uint LoaderFlags;
            public uint NumberOfRvaAndSizes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)] public ulong[] DataDirectory;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_NT_HEADERS64
        {
            public uint Signature;
            public IMAGE_FILE_HEADER FileHeader;
            public IMAGE_OPTIONAL_HEADER64 OptionalHeader;

        }
        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_DOS_HEADER
        {
            public ushort e_magic;
            public ushort e_cblp;
            public ushort e_cp;
            public ushort e_crlc;
            public ushort e_cparhdr;
            public ushort e_minalloc;
            public ushort e_maxalloc;
            public ushort e_ss;
            public ushort e_sp;
            public ushort e_csum;
            public ushort e_ip;
            public ushort e_cs;
            public ushort e_lfarlc;
            public ushort e_ovno;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public ushort[] e_res;
            public ushort e_oemid;
            public ushort e_oeminfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)] public ushort[] e_res2;
            public uint e_lfanew;
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
        [DllImport("user32.dll", EntryPoint = "DefWindowProcW")]
        public static extern nint DefWindowProc(nint hWnd, uint msg, nint wParam, nint lParam);
        [DllImport("user32.dll")]
        public static extern int ShowWindow(nint hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern int UpdateWindow(nint hWnd);
        [DllImport("user32.dll")]
        public static extern int DestroyWindow(nint hWnd);
        [DllImport("user32.dll", EntryPoint = "UnregisterClassW")]
        public static extern int UnregisterClass([MarshalAs(UnmanagedType.LPWStr)]string lpClassName, nint hInstance);
        [DllImport("user32.dll", EntryPoint = "MessageBoxW")]
        public static extern int MessageBox(nint hWnd, [MarshalAs(UnmanagedType.LPWStr)]string lpText, [MarshalAs(UnmanagedType.LPWStr)]string lpCaption, uint hInstance);
        [DllImport("user32.dll", EntryPoint = "PeekMessageW")]
        public static unsafe extern int PeekMessage(ref MSG lpMsg, nint hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);
        [DllImport("user32.dll")]
        public static unsafe extern int TranslateMessage(ref MSG lpMsg);
        [DllImport("user32.dll")]
        public static unsafe extern long DispatchMessageW(ref MSG lpMsg);
        [DllImport("user32.dll", EntryPoint = "SetWindowLongW")]
        public static unsafe extern int SetWindowLong(nint hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        public static unsafe extern int SetWindowPos(nint hwnd, nint hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll", EntryPoint = "RegisterClassExW")]
        public static extern nint RegisterClassEx(ref WNDCLASSEX unnamedParam1); // Great choice of name microsoft
        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW")]
        public static extern nint CreateWindowEx(uint dwExStyle, [MarshalAs(UnmanagedType.LPWStr)]string lpClassName, [MarshalAs(UnmanagedType.LPWStr)]string lpWindowName, uint dwStyle,
            int X, int Y, int nWidth, int nHeight, nint hWndParent, nint hMenu, nint hInstance, nint lpParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetModuleHandleW")]
        public static extern nint GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)]string? lpModuleName);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern int AllocConsole();
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern int SetConsoleMode(nint hConsoleHandle, uint dwMode);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "CreateFileW")]
        public static extern int CreateFile([MarshalAs(UnmanagedType.LPWStr)]string lpFileName, uint dwDesiredAccess, uint dwShareMode, 
            nint lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, nint hTemplateFile);
    }
}
