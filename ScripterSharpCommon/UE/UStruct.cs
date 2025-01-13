using System.Runtime.InteropServices;

namespace ScripterSharpCommon.UE
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct UStruct
    {
        public UStruct* SuperStruct => *(UStruct**)GetPtrOffset(Offsets.SuperStruct);
        public UField* Children => *(UField**)GetPtrOffset(Offsets.Children);
        public int PropertiesSize => *(int*)GetPtrOffset(Offsets.PropertiesSize);

        public bool IsA(UObject* Class)
        {
            fixed (UStruct* thisPtr = &this)
            {
                for (UStruct* Struct = thisPtr; Struct is not null; Struct = Struct->SuperStruct)
                {
                    if (Struct == Class)
                        return true;
                }
            }
            return false;
        }
        
        // UField
        private UField _obj;
        public UField* Next => _obj.Next;
        
        
        
        // UObject

        public void** VFTable => _obj.VFTable;
        public int ObjectFlags => _obj.ObjectFlags;
        public int InternalIndex => _obj.InternalIndex;
        public UStruct* ClassPrivate => _obj.ClassPrivate;
        public FName NamePrivate => _obj.NamePrivate;
        public UObject* OuterPrivate => _obj.OuterPrivate;

        public List<nint> GetAllChildren() => _obj.GetAllChildren();
        public UProperty* GetChildProperty(string name) => _obj.GetChildProperty(name);
        public nint GetChildPointer(string name) => _obj.GetChildPointer(name);

        public unsafe nint GetPtrOffset(int offset) => _obj.GetPtrOffset(offset);
        public string GetName() => _obj.GetName();
        public string GetFullName() => _obj.GetFullName();
    }
}
