using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ScripterSharp.UE;

namespace ScripterSharp
{
    public static unsafe class Scripter
    {
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern long FindPatternC(string signature, bool bRelative = false, uint offset = 0, bool bIsVar = false);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern long CSharpPrint(string signature);

        public static unsafe delegate*<FName*, FString*, void> FNameToString;
        public static unsafe delegate*<UObject*, UObject*, void*, void> ProcessEvent;
        public static FChunkedFixedUObjectArray* ObjObjects;

        public static void Print(string str)
        {
            CSharpPrint(str);
        }

        public static nint FindPattern(string signature, bool bRelative = false, uint offset = 0, bool bIsVar = false)
        {
            return new nint(FindPatternC(signature, bRelative, offset, bIsVar));
        }

        public static UObject* FindObject(string name)
        {
            for (int i = 0; i < ObjObjects->NumElements; i++)
            {
                var obj = ObjObjects->GetObjectById(i);
                if (obj == null) continue;
                if (obj->GetFullName().Contains(name)) return obj;
            }
            return null;
        }

        [UnmanagedCallersOnly]
        public static unsafe void Init()
        {
            Print("Hello from c#");

            // TODO: Add more, its 3 am and i dont feel like switching off 8.50
            var FNameToStringAddr = FindPattern("48 89 5C 24 ? 57 48 83 EC 30 83 79 04 00 48 8B DA 48 8B F9");
            var ProcessEventAddr = FindPattern("40 55 56 57 41 54 41 55 41 56 41 57 48 81 EC ? ? ? ? 48 8D 6C 24 ? 48 89 9D ? ? ? ? 48 8B 05 ? ? ? ? 48 33 C5 48 89 85 ? ? ? ? ? ? ? 45 33 F6");
            var ObjectsAddr = FindPattern("48 8B 05 ? ? ? ? 48 8B 0C C8 48 8D 04 D1 EB 03 48 8B ? 81 48 08 ? ? ? 40 49", false, 7, true);
            if (ObjectsAddr == IntPtr.Zero) ObjectsAddr = FindPattern("48 8B 05 ? ? ? ? 48 8B 0C C8 48 8B 04 D1", true, 3);
            
            FNameToString = (delegate*<FName*, FString*, void>)FNameToStringAddr;
            ProcessEvent = (delegate*<UObject*, UObject*, void*, void>)ProcessEventAddr;
            ObjObjects = (FChunkedFixedUObjectArray*)ObjectsAddr;

            //DumpObjects();
            CreateConsole();
        }
        
        static void DumpOffsets<T>()
        {
            foreach (var field in typeof(T).GetFields())
            {
                Print($"{field.Name}: {Marshal.OffsetOf<T>(field.Name)}");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SpawnObjectParams
        {
            public UObject* Class;
            public UObject* Outer;
            public UObject* Return;
        }

        static void CreateConsole()
        {
            var Engine = (UStruct*)FindObject("FortEngine_");
            UStruct* GameViewport = *(UStruct**)Engine->GetChildPointer("GameViewport");
            UObject** ViewportConsole = (UObject**)GameViewport->GetChildPointer("ViewportConsole");

            var ConsoleClass = FindObject("Class /Script/Engine.Console");
            var GSC = FindObject("GameplayStatics /Script/Engine.Default__GameplayStatics");
            var fn = FindObject("Function /Script/Engine.GameplayStatics.SpawnObject");
            SpawnObjectParams args = new()
            {
                Class = ConsoleClass,
                Outer = (UObject*)GameViewport
            };
            ProcessEvent(GSC, fn, &args);
            *ViewportConsole = args.Return;
        }

        static void DumpObjects()
        {
            string objFilePath = "C:/FN/obj.txt";
            Print($"Writing ~{ObjObjects->NumElements} objects to {objFilePath}");
            using (var stream = File.OpenWrite(objFilePath))
            {
                for (int i = 0; i < ObjObjects->NumElements; i++)
                {
                    var obj = ObjObjects->GetObjectById(i);
                    if (obj == null) continue;
                    stream.Write(Encoding.UTF8.GetBytes($"[{i}] {obj->GetFullName()}\n"));
                }
            }
            Print("Finished!");
        }
    }
}