using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private static byte?[] PatternToByte(string pattern)
        {
            List<byte?> bytes = new List<byte?>();
            for (int i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] == '?')
                {
                    i++;
                    if (pattern[i] == '?') i++;
                    bytes.Add(null);
                }
                else
                {
                    bytes.Add(byte.Parse(pattern.Substring(i, 2), NumberStyles.HexNumber));
                    i += 2;
                }
            }
            return bytes.ToArray();
        }

        public static unsafe nint FindPattern(string signature, bool bRelative = false, uint offset = 0, bool bIsVar = false)
        {
            var module = Process.GetCurrentProcess().MainModule;
            if (module is null)
            {
                Logger.Error("No MainModule");
                return nint.Zero;
            }
            var base_address = module.BaseAddress;
            var sizeOfImage = module.ModuleMemorySize;
            var patternBytes = PatternToByte(signature);
            var scanBytes = (byte*)base_address;

            var s = patternBytes.Length;

            Logger.Log($"Searching for {signature}");
            for (int i = 0; i < sizeOfImage - s; ++i)
            {
                bool found = true;
                for (var j = 0; j < s; ++j)
                {
                    if (scanBytes[i + j] != patternBytes[j] && patternBytes[j] != null)
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
