using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ScripterSharpCommon.Win32;

namespace ScripterSharpCommon
{
    public class Utils
    {
        public static string FortniteVersionString = "";
        public static double FortniteVersion;
        public static string EngineVersionString = "";
        public static double EngineVersion;

        private static int[] PatternToByte(string pattern)
        {
            List<int> bytes = new List<int>();
            for (int i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] == '?')
                {
                    i++;
                    if (pattern[i] == '?') i++;
                    bytes.Add(-1);
                }
                else
                {
                    bytes.Add(int.Parse(pattern.Substring(i, 2), NumberStyles.HexNumber));
                    i += 2;
                }
            }
            return bytes.ToArray();
        }

        public static unsafe nint FindPattern(string signature, bool bRelative = false, uint offset = 0, bool bIsVar = false)
        {
            var base_addess = Win32.GetModuleHandleW(null);
            var ntHeaders = (IMAGE_NT_HEADERS64*)(base_addess + ((IMAGE_DOS_HEADER*)base_addess)->e_lfanew);

            var sizeOfImage = ntHeaders->OptionalHeader.SizeOfImage;
            var patternBytes = PatternToByte(signature);
            var scanBytes = (byte*)base_addess;

            var s = patternBytes.Length;

            for (int i = 0; i < sizeOfImage - s; ++i)
            {
                bool found = true;
                for (var j = 0; j < s; ++j)
                {
                    if (scanBytes[i + j] != patternBytes[j] && patternBytes[j] != -1)
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    var address = &scanBytes[i];
                    if (bIsVar)
                        address = (address + offset + *(int*)(address + 3));
                    if (bRelative && !bIsVar)
                        address = (address + offset + 4) + *(int*)(address + offset);
                    return (nint)address;
                }
            }

            return nint.Zero;
        }
    }
}
