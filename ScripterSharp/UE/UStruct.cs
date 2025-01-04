using System.Runtime.InteropServices;

namespace ScripterSharp.UE
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
        private unsafe List<nint> GetAllChildren() // TODO: Make this IEnumerable?
        {
            List<nint> ret = new List<nint>();
            for (var CurrentClass = ClassPrivate; CurrentClass != null; CurrentClass = CurrentClass->SuperStruct)
            {
                var Child = CurrentClass->Children;

                if (Child != null)
                {
                    var Next = Child->Next;

                    if (Next != null)
                    {
                        while (Child != null)
                        {
                            ret.Add((nint)Child);
                            Child = Child->Next;
                        }
                    }
                }
            }
            return ret;
        }

        public UProperty* GetChildProperty(string name)
        {
            foreach (UProperty* child in GetAllChildren())
            {
                if (child->GetName() == name)
                    return child;
            }
            return null;
        }

        public nint GetChildPointer(string name) // i would make this T* GetChildPointer<T>() but that doesnt support pointers!
        {
            var Prop = GetChildProperty(name);
            if (Prop is null) return nint.Zero;
            return GetPtrOffset(Prop->Offset_Internal);
        }

        public unsafe nint GetPtrOffset(int offset) => _obj.GetPtrOffset(offset);
        public string GetName() => _obj.GetName();
        public string GetFullName() => _obj.GetFullName();
    }
}
