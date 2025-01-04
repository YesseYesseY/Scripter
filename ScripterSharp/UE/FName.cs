using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharp.UE
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FName
    {
        public uint ComparisonIndex;
        public uint Number;

        public static FString ToFString(FName name)
        {
            FString str;
            Scripter.FNameToString(&name, &str);
            return str;
        }

        public FString ToFString()
        {
            return ToFString(this);
        }

        public override string ToString()
        {
            return ToFString().ToString();
        }
    }
}
