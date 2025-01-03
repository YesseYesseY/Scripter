using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScripterCS.UE
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FString
    {
        public TArray<char> arr;

        public FString(string str)
        {
            arr.ArrayNum = str.Length;
            arr.ArrayMax = str.Length;
            arr.Data = (char*)Marshal.StringToHGlobalUni(str);
        }

        public override string ToString()
        {
            return new string(arr.Data);
        }
    }
}
