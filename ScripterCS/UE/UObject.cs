using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static ScripterCS.Scripter;

namespace ScripterCS.UE
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct UObject
    {
        public void** VFTable;
        public int ObjectFlags;
        public int InternalIndex;
        public UObject* ClassPrivate;
        public FName NamePrivate;
        public UObject* OuterPrivate;

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
