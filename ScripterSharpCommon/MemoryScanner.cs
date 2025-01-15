using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharpCommon
{
    public class MemoryScanner
    {
        nint basePtr;
        byte[] buffer;
        public unsafe MemoryScanner()
        {
            var process = Process.GetCurrentProcess();
            var module = process.MainModule;
            if (module is null)
            {
                Logger.Error("MainModule is null");
                return;
            }
            var memSize = module.ModuleMemorySize;
            buffer = new byte[memSize];
            basePtr = module.BaseAddress;
            fixed (byte* bufferPtr = buffer)
            {
                Win32.ReadProcessMemory(process.Handle, basePtr, bufferPtr, (ulong)memSize, out ulong bytesRead);
            }
        }

        private static byte?[] PatternToByte(ReadOnlySpan<char> pattern)
        {
            List<byte?> bytes = new List<byte?>(pattern.Length / 2);
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
                    bytes.Add(byte.Parse(pattern.Slice(i, 2), NumberStyles.HexNumber));
                    i += 2;
                }
            }
            return bytes.ToArray();
        }

        public unsafe nint FindPattern(string signature, bool bRelative = false, uint offset = 0, bool bIsVar = false)
        {
            var patternBytes = PatternToByte(signature);

            var s = patternBytes.Length;

            Logger.Log($"Scanner searching for {signature}");
            fixed (byte* scanBytes = buffer)
            {
                for (int i = 0; i < buffer.Length - s; ++i)
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
                        var address = basePtr + i;

                        if (bIsVar)
                            address = (nint)(address + offset + *(int*)(address + 3));

                        if (bRelative && !bIsVar)
                            address = (nint)(address + offset + 4) + *(int*)(address + offset);

                        return address;
                    }
                }
            }

            return nint.Zero;
        }
    }
}
