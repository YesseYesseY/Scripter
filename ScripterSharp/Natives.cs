using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ScripterSharp.UE;

namespace ScripterSharp
{
    public static unsafe class Natives
    {
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern long FindPatternC(string signature, bool bRelative = false, uint offset = 0, bool bIsVar = false);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern long CSharpPrint(string signature);
        [DllImport("Scripter.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddProcessEventHook(void* func, PEHookDelegate csfunc);

        public delegate void PEHookDelegate(UObject* obj, void* argptr); // for some reason delegate*<UObject*, void*> doesn't work
        
        public static unsafe delegate*<FName*, FString*, void> FNameToString;
        public static unsafe delegate*<UObject*, UObject*, void*, void> ProcessEvent;
    }
}
