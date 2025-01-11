using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace ScripterSharpCommon.UE
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

        public unsafe List<nint> GetAllChildren() // TODO: Make this IEnumerable?
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

        public int GetChildOffset(string name)
        {
            var Prop = GetChildProperty(name);
            if (Prop is null) return -1;
            return Prop->Offset_Internal;
        }

        public nint GetChildPointer(string name) // i would make this T* GetChildPointer<T>() but that doesnt support pointers!
        {
            var Prop = GetChildProperty(name);
            if (Prop is null) return nint.Zero;
            return GetPtrOffset(Prop->Offset_Internal);
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

        public void ProcessEvent(UObject* func, void* args)
        {
            fixed (UObject* ptr = &this)
            {
                Natives.ProcessEvent(ptr, func, args);
            }
        }

        public static ObjectArray Objects;
        public static unsafe UObject* FindObject(string name)
        {
            foreach (UObject* obj in Objects)
            {
                if (obj->GetFullName().Contains(name)) return obj;
            }
            return null;
        }
    }

}
