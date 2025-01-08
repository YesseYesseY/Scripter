using System.Runtime.InteropServices;

namespace ScripterSharp.UE
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct UField
    {
        private UObject _obj;
        public void** VFTable => _obj.VFTable;
        public int ObjectFlags => _obj.ObjectFlags;
        public int InternalIndex => _obj.InternalIndex;
        public UStruct* ClassPrivate => _obj.ClassPrivate;
        public FName NamePrivate => _obj.NamePrivate;
        public UObject* OuterPrivate => _obj.OuterPrivate;
        public UField* Next => *(UField**)_obj.GetPtrOffset(40);

        public List<nint> GetAllChildren() => _obj.GetAllChildren();
        public UProperty* GetChildProperty(string name) => _obj.GetChildProperty(name);
        public nint GetChildPointer(string name) => _obj.GetChildPointer(name);

        public unsafe nint GetPtrOffset(int offset) => _obj.GetPtrOffset(offset);
        public string GetName() => _obj.GetName();
        public string GetFullName() => _obj.GetFullName();
    }
}
