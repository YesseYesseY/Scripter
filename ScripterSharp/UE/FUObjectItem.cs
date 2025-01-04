using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharp.UE
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FUObjectItem
    {
        public UObject* Object;
        public int Flags;
        public int ClusterRootIndex;
        public int SerialNumber;
    }
}
