using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Text;
using ScripterSharpCommon.UE;
using ScripterSharpCommon;

namespace ScripterSharp
{
    public class ScripterAssemblyLoadContext : AssemblyLoadContext
    {
        private string PluginPath;

        public ScripterAssemblyLoadContext(string path)
        {
            PluginPath = path;
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.GetName().Name == assemblyName.Name)
                {
                    Logger.Log($"Using already existing {assemblyName.Name}");
                    return asm;
                }
            }

            return LoadFromAssemblyPath(Path.Combine(PluginPath, $"{assemblyName.Name}.dll"));
        }
    }
    public static class Scripter
    {
        public static List<BaseModule> modules = new List<BaseModule>();
        public static ScripterAssemblyLoadContext AssemblyLoader;

        public static string FortniteVersionString = "";
        public static double FortniteVersion;
        public static string EngineVersionString = "";
        public static double EngineVersion;

        static List<Delegate> yestes = new List<Delegate>(); 
        private static unsafe bool Setup()
        {
            var GetEngineAddr = Utils.FindPattern("40 53 48 83 EC 20 48 8B D9 E8 ? ? ? ? 48 8B C8 41 B8 04 ? ? ? 48 8B D3");
            if (GetEngineAddr == nint.Zero)
            {
                Logger.Error("Couldn't find GetEngineVersion");
                return false;
            }
            delegate*<FString> GetEngineVersion = (delegate*<FString>)GetEngineAddr;
            string FullVersion = GetEngineVersion().ToString();

            if (!FullVersion.Contains("Live") && !FullVersion.Contains("Next") && !FullVersion.Contains("Cert"))
            {
                // 4.23.0-6058028+++Fortnite+Release-8.50
                EngineVersionString = FullVersion.Substring(0, FullVersion.IndexOf('-'));
                FortniteVersionString = FullVersion.Substring(FullVersion.LastIndexOf('-') + 1);
                if (EngineVersionString.IndexOf('.') != EngineVersionString.LastIndexOf('.'))
                    EngineVersionString = EngineVersionString.Substring(0, EngineVersionString.LastIndexOf('.'));
                try
                {
                    FortniteVersion = double.Parse(FortniteVersionString, CultureInfo.InvariantCulture);
                    EngineVersion = double.Parse(EngineVersionString, CultureInfo.InvariantCulture) * 100;
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                    Logger.Error(FortniteVersionString);
                    Logger.Error(EngineVersionString);
                    return false;
                }

            }

            var UseNewObjects = true;

            nint ObjectsAddr = nint.Zero;
            nint ProcessEventAddr = nint.Zero;
            nint FNameToStringAddr = nint.Zero;

            if (EngineVersion >= 416 && EngineVersion <= 420)
            {
                ObjectsAddr = Utils.FindPattern("48 8B 05 ? ? ? ? 48 8D 1C C8 81 4B ? ? ? ? ? 49 63 76 30", false, 7, true);

                if (ObjectsAddr == nint.Zero)
                    ObjectsAddr = Utils.FindPattern("48 8B 05 ? ? ? ? 48 8D 14 C8 EB 03 49 8B D6 8B 42 08 C1 E8 1D A8 01 0F 85 ? ? ? ? F7 86 ? ? ? ? ? ? ? ?", false, 7, true);

                if (EngineVersion == 420)
                    FNameToStringAddr = Utils.FindPattern("48 89 5C 24 ? 57 48 83 EC 40 83 79 04 00 48 8B DA 48 8B F9 75 23 E8 ? ? ? ? 48 85 C0 74 19 48 8B D3 48 8B C8 E8 ? ? ? ? 48");
                else
                {
                    FNameToStringAddr = Utils.FindPattern("40 53 48 83 EC 40 83 79 04 00 48 8B DA 75 19 E8 ? ? ? ? 48 8B C8 48 8B D3 E8 ? ? ? ?");

                    if (FNameToStringAddr == nint.Zero) // This means that we are in season 1 (i think).
                    {
                        FNameToStringAddr = Utils.FindPattern("48 89 5C 24 ? 48 89 74 24 ? 48 89 7C 24 ? 41 56 48 83 EC 20 48 8B DA 4C 8B F1 E8 ? ? ? ? 4C 8B C8 41 8B 06 99");

                        if (FNameToStringAddr != nint.Zero)
                            EngineVersion = 416;
                    }
                }

                ProcessEventAddr = Utils.FindPattern("40 55 56 57 41 54 41 55 41 56 41 57 48 81 EC ? ? ? ? 48 8D 6C 24 ? 48 89 9D ? ? ? ? 48 8B 05 ? ? ? ? 48 33 C5 48 89 85 ? ? ? ? 48 63 41 0C 45 33 F6");

                UseNewObjects = false;
            }

            if (EngineVersion >= 421 && EngineVersion <= 424)
            {
                FNameToStringAddr = Utils.FindPattern("48 89 5C 24 ? 57 48 83 EC 30 83 79 04 00 48 8B DA 48 8B F9");
                ProcessEventAddr = Utils.FindPattern("40 55 56 57 41 54 41 55 41 56 41 57 48 81 EC ? ? ? ? 48 8D 6C 24 ? 48 89 9D ? ? ? ? 48 8B 05 ? ? ? ? 48 33 C5 48 89 85 ? ? ? ? ? ? ? 45 33 F6");
            }

            if (EngineVersion >= 425 && EngineVersion < 500)
            {
                FNameToStringAddr = Utils.FindPattern("48 89 5C 24 ? 55 56 57 48 8B EC 48 83 EC 30 8B 01 48 8B F1 44 8B 49 04 8B F8 C1 EF 10 48 8B DA 0F B7 C8 89 4D 24 89 7D 20 45 85 C9");
                ProcessEventAddr = Utils.FindPattern("40 55 56 57 41 54 41 55 41 56 41 57 48 81 EC ? ? ? ? 48 8D 6C 24 ? 48 89 9D ? ? ? ? 48 8B 05 ? ? ? ? 48 33 C5 48 89 85 ? ? ? ? 8B 41 0C 45 33 F6");
            }

            if (EngineVersion >= 421 && EngineVersion <= 426)
            {
                ObjectsAddr = Utils.FindPattern("48 8B 05 ? ? ? ? 48 8B 0C C8 48 8D 04 D1 EB 03 48 8B ? 81 48 08 ? ? ? 40 49", false, 7, true);

                if (ObjectsAddr == nint.Zero)
                    ObjectsAddr = Utils.FindPattern("48 8B 05 ? ? ? ? 48 8B 0C C8 48 8B 04 D1", true, 3);
            }

            if (FortniteVersion >= 16.00 && FortniteVersion < 18.40) // 4.26.1
            {
                ObjectsAddr = Utils.FindPattern("48 8B 05 ? ? ? ? 48 8B 0C C8 48 8B 04 D1", true, 3);
            }

            if (FNameToStringAddr == nint.Zero)
            {
                Logger.Error("Failed to find FNameToString");
                return false;
            }
            if (ProcessEventAddr == nint.Zero)
            {
                Logger.Error("Failed to find ProcessEvent");
                return false;
            }
            if (ObjectsAddr == nint.Zero)
            {
                Logger.Error("Failed to find Objects");
                return false;
            }


            Natives.FNameToString = (delegate*<FName*, FString*, void>)FNameToStringAddr;
            Natives.ProcessEvent = (delegate*<UObject*, UObject*, void*, void>)ProcessEventAddr;
            UObject.Objects = new ObjectArray(ObjectsAddr, UseNewObjects);

            // TODO: Test more versions
            if (EngineVersion >= 420 && EngineVersion <= 421)
            {
                Offsets.SuperStruct = 48;
                Offsets.Children = 56;
                Offsets.PropertiesSize = 64;
            }
            if (EngineVersion >= 422)
            {
                Offsets.SuperStruct = 64;
                Offsets.Children = 72;
                Offsets.PropertiesSize = 80;
            }

            // From what i can see UFunction barely changes between versions, so why not go based off UStruct
            var UStructClass = (UStruct*)UObject.FindObject("Class /Script/CoreUObject.Struct");
            var StructSize = UStructClass->PropertiesSize;
            Offsets.FunctionFlags = StructSize;

            return true;
        }

        [UnmanagedCallersOnly]
        public static unsafe void Init() // is basically just Setup() from structs.h
        {
            Win32.AllocConsole();

            Logger.Log("Hello from c#");
            
            if (!Setup())
            {
                Logger.Error("Failed setup");
                return;
            }

            var ModulesPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ScripterSharp", "Modules");
            Logger.Log($"Module path: {ModulesPath}");
            Directory.CreateDirectory(ModulesPath);

            AssemblyLoader = new ScripterAssemblyLoadContext(ModulesPath);

            foreach (var dll in Directory.GetFiles(ModulesPath, "*.dll"))
            {
                // TODO: Look into unloading the modules
                foreach (var typ in AssemblyLoader.LoadFromAssemblyName(new AssemblyName("ExampleModule")).GetExportedTypes())
                {
                    foreach (var method in typ.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        var attrib = (ProcessEventHookAttribute?)Attribute.GetCustomAttribute(method, typeof(ProcessEventHookAttribute));
                        if (attrib != null)
                        {
                            var del = (Natives.PEHookDelegate)method.CreateDelegate(typeof(Natives.PEHookDelegate), null);
                            yestes.Add(del); // shoutout garbage collector
                            Natives.AddProcessEventHook(UObject.FindObject(attrib.name), del);
                        }
                    }

                    if (typ.IsSubclassOf(typeof(BaseModule)))
                    {
                        BaseModule? mod = (BaseModule?)Activator.CreateInstance(typ);
                        if (mod == null)
                        {
                            Logger.Warn($"Tried to create {typ.Name} and failed");
                            continue;
                        }

                        modules.Add(mod);

                        Logger.Log($"Loaded module: {mod.Name}");

                        mod.OnLoad();
                    }
                }
            }

            new Thread(ScripterGui.Start).Start();
        }
    }
}