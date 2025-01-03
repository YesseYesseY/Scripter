using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScripterCS.UE
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct UProperty
    {
        private UField _obj;
        public void** VFTable => _obj.VFTable;
        public int ObjectFlags => _obj.ObjectFlags;
        public int InternalIndex => _obj.InternalIndex;
        public UStruct* ClassPrivate => _obj.ClassPrivate;
        public FName NamePrivate => _obj.NamePrivate;
        public UObject* OuterPrivate => _obj.OuterPrivate;
        public UField* Next => _obj.Next;
        
        //public int ArrayDim => *(int*)_obj.GetPtrOffset(48);
        //public int ElementSize => *(int*)_obj.GetPtrOffset(52);
        //public long PropertyFlags => *(long*)_obj.GetPtrOffset(56);
        //public ushort RepIndex => *(ushort*)_obj.GetPtrOffset(64);
        //public byte BlueprintReplicationCondition => *(byte*)_obj.GetPtrOffset(66);
        //public byte Offset_Internal => *(byte*)_obj.GetPtrOffset(67);

        public unsafe nint GetPtrOffset(int offset) => _obj.GetPtrOffset(offset);
        public string GetName() => _obj.GetName();
        public string GetFullName() => _obj.GetFullName();
    }
}
