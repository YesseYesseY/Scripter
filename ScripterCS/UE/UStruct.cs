using System.Runtime.InteropServices;

namespace ScripterCS.UE
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct UStruct
    {
        private UField _obj;
        public void** VFTable => _obj.VFTable;
        public int ObjectFlags => _obj.ObjectFlags;
        public int InternalIndex => _obj.InternalIndex;
        public UStruct* ClassPrivate => _obj.ClassPrivate;
        public FName NamePrivate => _obj.NamePrivate;
        public UObject* OuterPrivate => _obj.OuterPrivate;
        public UField* Next => _obj.Next;
        public UStruct* SuperStruct => *(UStruct**)GetPtrOffset(64);
        public UField* Children => *(UField**)GetPtrOffset(72);
        private unsafe List<nint> GetAllChildren()
        {
            List<nint> ret = new List<nint>();
            for (var CurrentClass = ClassPrivate; CurrentClass != null; CurrentClass = CurrentClass->SuperStruct)
            {
                var Prop = CurrentClass->Children;

                if (Prop != null)
                {
                    var Next = Prop->Next;

                    if (Next != null)
                    {
                        while (Prop != null)
                        {
                            ret.Add((nint)Prop);
                            Prop = Prop->Next;
                        }
                    }
                }
            }
            return ret;
        }

        public UObject* GetChildObject(string name)
        {
            foreach (UObject* child in GetAllChildren())
            {
                if (child->GetName() == name)
                    return child;
            }
            return null;
        }

        public unsafe nint GetPtrOffset(int offset) => _obj.GetPtrOffset(offset);
        public string GetName() => _obj.GetName();
        public string GetFullName() => _obj.GetFullName();
    }
}
