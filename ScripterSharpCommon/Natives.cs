using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ScripterSharpCommon.UE;

namespace ScripterSharpCommon
{
    public static unsafe class Natives
    {
        public delegate void PEHookDelegate(UObject* obj, void* argptr); // for some reason delegate*<UObject*, void*> doesn't work
        public delegate void ProcessEventDelegate(UObject* obj, UObject* func, void* args);
        public static unsafe delegate*<FName*, FString*, void> FNameToString;
        public static unsafe ProcessEventDelegate ProcessEvent;
    }
}
