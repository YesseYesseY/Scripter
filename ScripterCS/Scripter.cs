using System.Runtime.InteropServices;
using System.Text;

namespace ScripterCS
{
    public static class Scripter
    {
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FUObjectItem
        {
            public UObject* Object;
            public int Flags;
            public int ClusterRootIndex;
            public int SerialNumber;
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FChunkedFixedUObjectArray
        {
            static int NumElementsPerChunk = 64 * 1024;
            FUObjectItem** Objects;
            FUObjectItem* PreAllocatedObjects;
            int MaxElements;
            public int NumElements;
            int MaxChunks;
            int NumChunks;

            public UObject* GetObjectById(int Index)
            {
                if (Index > NumElements || Index < 0) return null;

                int ChunkIndex = Index / NumElementsPerChunk;
                int WithinChunkIndex = Index % NumElementsPerChunk;

                if (ChunkIndex > NumChunks) return null;
                FUObjectItem* Chunk = Objects[ChunkIndex];
                if (Chunk == null) return null;

                var obj = (Chunk + WithinChunkIndex)->Object;

                return obj;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FName
        {
            public uint ComparisonIndex;
            public uint Number;

            public static FString ToFString(FName name)
            {
                FString str;
                FNameToString(&name, &str);
                return str;
            }

            public FString ToFString()
            {
                return ToFString(this);
            }

            public override string ToString()
            {
                return ToFString().ToString();
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct TArray<T>
        {
            public T* Data;
            public int ArrayNum;
            public int ArrayMax;
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct FString
        {
            public TArray<char> arr;

            public override string ToString()
            {
                return new string(arr.Data);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct UObject
        {
            public void** VFTable;
            public int ObjectFlags;
            public int InternalIndex;
            public UObject* ClassPrivate;
            public FName NamePrivate;
            public UObject* OuterPrivate;
        }

        // TODO: Make all this cleaner/better
        unsafe delegate long FindPatternDelegate(nint signature, bool bRelative = false, uint offset = 0, bool bIsVar = false);
        static FindPatternDelegate FindPatternC;
        unsafe delegate void FNameToStringDelegate(FName* name, FString* arr);
        static FNameToStringDelegate FNameToString;
        unsafe delegate void CSharpPrintDelegate(nint str);
        static CSharpPrintDelegate CSharpPrint;

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
            
            FChunkedFixedUObjectArray* ObjObjects = (FChunkedFixedUObjectArray*)ObjectsAddr.ToPointer();
            string objFilePath = "C:/FN/obj.txt";
            Print($"Writing ~{ObjObjects->NumElements} objects to {objFilePath}");
            using (var stream = File.OpenWrite(objFilePath))
            {
                for (int i = 0; i < ObjObjects->NumElements; i++)
                {
                    var obj = ObjObjects->GetObjectById(i);
                    if (obj == null) continue;
                    stream.Write(Encoding.UTF8.GetBytes($"[{i}] {obj->NamePrivate}\n"));
                }
            }
            Print("Finished!");
        }
    }
}
