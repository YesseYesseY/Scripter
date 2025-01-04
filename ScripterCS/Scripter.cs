using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ScripterCS.UE;

namespace ScripterCS
{
    public static unsafe class Scripter
    {
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern long FindPatternC(string signature, bool bRelative = false, uint offset = 0, bool bIsVar = false);
        [DllImport("Scripter.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern long CSharpPrint(string signature);


        public unsafe delegate void FNameToStringDelegate(FName* name, FString* arr);
        public static FNameToStringDelegate FNameToString;
        public static FChunkedFixedUObjectArray* ObjObjects;

        public static void Print(string str)
        {
            CSharpPrint(str);
        }

        public static nint FindPattern(string signature, bool bRelative = false, uint offset = 0, bool bIsVar = false)
        {
            return new nint(FindPatternC(signature, bRelative, offset, bIsVar));
        }

        [UnmanagedCallersOnly]
        public static unsafe void Init()
        {
            Print("Hello from c#");

            // TODO: Add more, its 3 am and i dont feel like switching off 8.50
            var FNameToStringAddr = FindPattern("48 89 5C 24 ? 57 48 83 EC 30 83 79 04 00 48 8B DA 48 8B F9");
            var ObjectsAddr = FindPattern("48 8B 05 ? ? ? ? 48 8B 0C C8 48 8D 04 D1 EB 03 48 8B ? 81 48 08 ? ? ? 40 49", false, 7, true);
            if (ObjectsAddr == IntPtr.Zero) ObjectsAddr = FindPattern("48 8B 05 ? ? ? ? 48 8B 0C C8 48 8B 04 D1", true, 3);
            FNameToString = Marshal.GetDelegateForFunctionPointer<FNameToStringDelegate>(FNameToStringAddr);
            ObjObjects = (FChunkedFixedUObjectArray*)ObjectsAddr;

            //for (int i = 0; i < ObjObjects->NumElements; i++)
            //{
            //    var obj = ObjObjects->GetObjectById(i);
            //    if (obj == null) continue;
            //    if (obj->GetFullName().Contains("FortEngine_"))
            //    {
            //        var Engine = (UStruct*)obj;
            //        var GameViewport = Engine->GetChildObject("GameViewport");
            //        Print(GameViewport->GetFullName());
            //        break;
            //    }
            //}
            DumpObjects();
        }
        
        static void DumpOffsets<T>()
        {
            foreach (var field in typeof(T).GetFields())
            {
                Print($"{field.Name}: {Marshal.OffsetOf<T>(field.Name)}");
            }
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