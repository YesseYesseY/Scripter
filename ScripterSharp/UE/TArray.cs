using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharp.UE
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct TArray<T> where T : unmanaged
    {
        public T* Data;
        public int ArrayNum;
        public int ArrayMax;

        public T this[int index]
        {
            get => Data[index];
        }
    }
}
