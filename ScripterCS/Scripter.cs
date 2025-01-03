using System.Runtime.InteropServices;
using System.Text;
using ScripterCS.UE;

namespace ScripterCS
{
    public static unsafe class Scripter
    {
        // TODO: Make all this cleaner/better
        public unsafe delegate long FindPatternDelegate(nint signature, bool bRelative = false, uint offset = 0, bool bIsVar = false);
        public unsafe delegate void FNameToStringDelegate(FName* name, FString* arr);
        public unsafe delegate void CSharpPrintDelegate(nint str);
        public static FindPatternDelegate FindPatternC;
        public static FNameToStringDelegate FNameToString;
        public static CSharpPrintDelegate CSharpPrint;
        public static FChunkedFixedUObjectArray* ObjObjects;

        public static void Print(string str)
        {
            var cstr = Marshal.StringToHGlobalAnsi(str);
            CSharpPrint(cstr);
            Marshal.FreeHGlobal(cstr);
        }

        public static nint FindPattern(string signature, bool bRelative = false, uint offset = 0, bool bIsVar = false)
        {
            var cstr = Marshal.StringToHGlobalAnsi(signature);
            var addr = FindPatternC(cstr, bRelative, offset, bIsVar);
            Marshal.FreeHGlobal(cstr);
            return new IntPtr(addr);
        }

        [UnmanagedCallersOnly]
        public static unsafe void Init(nint FindPatternPtr, nint CSharpPrintPtr)
        {
            FindPatternC = Marshal.GetDelegateForFunctionPointer<FindPatternDelegate>(FindPatternPtr);
            CSharpPrint = Marshal.GetDelegateForFunctionPointer<CSharpPrintDelegate>(CSharpPrintPtr);

            Print("Hello from c#");

            // TODO: Add more, its 3 am and i dont feel like switching off 8.50
            var FNameToStringAddr = FindPattern("48 89 5C 24 ? 57 48 83 EC 30 83 79 04 00 48 8B DA 48 8B F9");
            var ObjectsAddr = FindPattern("48 8B 05 ? ? ? ? 48 8B 0C C8 48 8D 04 D1 EB 03 48 8B ? 81 48 08 ? ? ? 40 49", false, 7, true);
            if (ObjectsAddr == IntPtr.Zero) ObjectsAddr = FindPattern("48 8B 05 ? ? ? ? 48 8B 0C C8 48 8B 04 D1", true, 3);
            FNameToString = Marshal.GetDelegateForFunctionPointer<FNameToStringDelegate>(FNameToStringAddr);
            ObjObjects = (FChunkedFixedUObjectArray*)ObjectsAddr;

            DumpObjects();
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
/*

UObject (40)
 
4.23:

UField : UObject // size 48
{
    UField* Next; // size 8 offset 40
}

UEnum : UField // size 80 + ?
{
    FString CppType;                   // size 16 offset 48
    TArray<TPair<FName, int64>> Names; // size 16 offset 64
    // Who cares about the rest
}

UStruct : UField // size 136
{
    FStructBaseChain ??? this messes up everything i just did !

    UStruct* SuperStruct;                    // size 8  offset 48
    UField* Children;                        // size 8  offset 56
    int32 PropertiesSize;                    // size 4  offset 64
    int32 MinAlignment;                      // size 4  offset 68
    TArray<uint8> Script;                    // size 16 offset 72
    UProperty* PropertyLink;                 // size 8  offset 88
    UProperty* RefLink;                      // size 8  offset 96
    UProperty* DestructorLink;               // size 8  offset 104
    UProperty* PostConstructLink;            // size 8  offset 112
    TArray<UObject*> ScriptObjectReferences; // size 16 offset 120
}

UClass : UStruct
{
    // TODO   
}
 
 */