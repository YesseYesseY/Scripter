using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharp.UE
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FFixedUObjectArray
    {
        public FUObjectItem* Objects;
        public int MaxElements;
        public int NumElements;

        public UObject* GetObjectById(int index)
        {
            return Objects[index].Object;
        }
    }
}
