using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

// https://github.com/ocornut/imgui/blob/master/examples/example_win32_directx9/
namespace ScripterSharp
{
    public unsafe static class ScripterGui
    {
        static bool demoWindow = true;
        public static unsafe void Start()
        {
            // TODO: Move all of gui.h to c#
            ImGui.InitGui(() =>
            {
                ImGui.ShowDemoWindow(ref demoWindow);
            });
        }
    }
}
