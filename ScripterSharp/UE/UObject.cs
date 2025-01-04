using System.Runtime.InteropServices;
namespace ScripterSharp.UE
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct UObject
    {
        public void** VFTable;
        public int ObjectFlags;
        public int InternalIndex;
        public UStruct* ClassPrivate; // TODO: UClass
        public FName NamePrivate;
        public UObject* OuterPrivate;
        
        public unsafe nint GetPtrOffset(int offset)
        {
            fixed (void* ptr = &this)
            {
                return (nint)ptr + offset;
            }
        }

        public string GetName()
        {
            return NamePrivate.ToString();
        }

        public string GetFullName()
        {
            string temp = "";
            for (var outer = OuterPrivate; outer != null; outer = outer->OuterPrivate)
                temp = $"{outer->GetName()}.{temp}";

            return $"{ClassPrivate->GetName()} {temp}{GetName()}";
        }
    }

}
