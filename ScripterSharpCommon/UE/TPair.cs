using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharpCommon.UE
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TPair<T, T2>
        where T : unmanaged
        where T2 : unmanaged
    {
        public T First;
        public T2 Second;
    }
}
