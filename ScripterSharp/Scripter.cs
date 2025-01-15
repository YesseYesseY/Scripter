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
        public static ScripterAssemblyLoadContext? AssemblyLoader;
        static Dictionary<nint, Natives.PEHookDelegate> PEhooks = new Dictionary<nint, Natives.PEHookDelegate>();
        static List<Delegate> yestes = new List<Delegate>(); // Literally just exists because of the garbage collector

        public static unsafe void Setup()
        {
            var GetEngineAddr = Utils.FindPattern("40 53 48 83 EC 20 48 8B D9 E8 ? ? ? ? 48 8B C8 41 B8 04 ? ? ? 48 8B D3");
            if (GetEngineAddr == nint.Zero)
            {
                Logger.Error("Couldn't find GetEngineVersion");
                return;
            }
            delegate*<FString> GetEngineVersion = (delegate*<FString>)GetEngineAddr;
            string FullVersion = GetEngineVersion().ToString();

            if (!FullVersion.Contains("Live") && !FullVersion.Contains("Next") && !FullVersion.Contains("Cert"))
            {
                // 4.23.0-6058028+++Fortnite+Release-8.50
                var EngineVersionString = FullVersion.Substring(0, FullVersion.IndexOf('-'));
                var FortniteVersionString = FullVersion.Substring(FullVersion.LastIndexOf('-') + 1);
                if (EngineVersionString.IndexOf('.') != EngineVersionString.LastIndexOf('.'))
                    EngineVersionString = EngineVersionString.Substring(0, EngineVersionString.LastIndexOf('.'));
                try
                {
                    var FortniteVersion = double.Parse(FortniteVersionString, CultureInfo.InvariantCulture);
                    var EngineVersion = double.Parse(EngineVersionString, CultureInfo.InvariantCulture); // I dont see a reason why this whould be * 100
                    Utils.FortniteVersion = FortniteVersion;
                    Utils.EngineVersion = EngineVersion; 
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                    Logger.Error(FortniteVersionString);
                    Logger.Error(EngineVersionString);
                    return;
                }

                Utils.EngineVersionString = EngineVersionString;
                Utils.FortniteVersionString = FortniteVersionString;
            }

            var UseNewObjects = true;

            nint ObjectsAddr = nint.Zero;
            nint ProcessEventAddr = nint.Zero;
            nint FNameToStringAddr = nint.Zero;

            if (Utils.EngineVersion >= 4.16 && Utils.EngineVersion <= 4.20)
            {
                ObjectsAddr = Utils.FindPattern("48 8B 05 ? ? ? ? 48 8D 1C C8 81 4B ? ? ? ? ? 49 63 76 30", false, 7, true);

                if (ObjectsAddr == nint.Zero)
                    ObjectsAddr = Utils.FindPattern("48 8B 05 ? ? ? ? 48 8D 14 C8 EB 03 49 8B D6 8B 42 08 C1 E8 1D A8 01 0F 85 ? ? ? ? F7 86 ? ? ? ? ? ? ? ?", false, 7, true);

                if (Utils.EngineVersion == 4.20)
                    FNameToStringAddr = Utils.FindPattern("48 89 5C 24 ? 57 48 83 EC 40 83 79 04 00 48 8B DA 48 8B F9 75 23 E8 ? ? ? ? 48 85 C0 74 19 48 8B D3 48 8B C8 E8 ? ? ? ? 48");
                else
                {
                    FNameToStringAddr = Utils.FindPattern("40 53 48 83 EC 40 83 79 04 00 48 8B DA 75 19 E8 ? ? ? ? 48 8B C8 48 8B D3 E8 ? ? ? ?");

                    if (FNameToStringAddr == nint.Zero) // This means that we are in season 1 (i think).
                    {
                        FNameToStringAddr = Utils.FindPattern("48 89 5C 24 ? 48 89 74 24 ? 48 89 7C 24 ? 41 56 48 83 EC 20 48 8B DA 4C 8B F1 E8 ? ? ? ? 4C 8B C8 41 8B 06 99");

                        if (FNameToStringAddr != nint.Zero)
                            Utils.EngineVersion = 416;
                    }
                }

                ProcessEventAddr = Utils.FindPattern("40 55 56 57 41 54 41 55 41 56 41 57 48 81 EC ? ? ? ? 48 8D 6C 24 ? 48 89 9D ? ? ? ? 48 8B 05 ? ? ? ? 48 33 C5 48 89 85 ? ? ? ? 48 63 41 0C 45 33 F6");

                UseNewObjects = false;
            }

            if (Utils.EngineVersion >= 4.21 && Utils.EngineVersion <= 4.24)
            {
                FNameToStringAddr = Utils.FindPattern("48 89 5C 24 ? 57 48 83 EC 30 83 79 04 00 48 8B DA 48 8B F9");
                ProcessEventAddr = Utils.FindPattern("40 55 56 57 41 54 41 55 41 56 41 57 48 81 EC ? ? ? ? 48 8D 6C 24 ? 48 89 9D ? ? ? ? 48 8B 05 ? ? ? ? 48 33 C5 48 89 85 ? ? ? ? ? ? ? 45 33 F6");
            }

            if (Utils.EngineVersion >= 4.25 && Utils.EngineVersion < 5.00)
            {
                FNameToStringAddr = Utils.FindPattern("48 89 5C 24 ? 55 56 57 48 8B EC 48 83 EC 30 8B 01 48 8B F1 44 8B 49 04 8B F8 C1 EF 10 48 8B DA 0F B7 C8 89 4D 24 89 7D 20 45 85 C9");
                ProcessEventAddr = Utils.FindPattern("40 55 56 57 41 54 41 55 41 56 41 57 48 81 EC ? ? ? ? 48 8D 6C 24 ? 48 89 9D ? ? ? ? 48 8B 05 ? ? ? ? 48 33 C5 48 89 85 ? ? ? ? 8B 41 0C 45 33 F6");
            }

            if (Utils.EngineVersion >= 4.21 && Utils.EngineVersion <= 4.26)
            {
                ObjectsAddr = Utils.FindPattern("48 8B 05 ? ? ? ? 48 8B 0C C8 48 8D 04 D1 EB 03 48 8B ? 81 48 08 ? ? ? 40 49", false, 7, true);

                if (ObjectsAddr == nint.Zero)
                    ObjectsAddr = Utils.FindPattern("48 8B 05 ? ? ? ? 48 8B 0C C8 48 8B 04 D1", true, 3);
            }

            if (Utils.FortniteVersion >= 16.00 && Utils.FortniteVersion < 18.40) // 4.26.1
            {
                ObjectsAddr = Utils.FindPattern("48 8B 05 ? ? ? ? 48 8B 0C C8 48 8B 04 D1", true, 3);
            }

            if (FNameToStringAddr == nint.Zero)
            {
                Logger.Error("Failed to find FNameToString");
                return;
            }
            if (ProcessEventAddr == nint.Zero)
            {
                Logger.Error("Failed to find ProcessEvent");
                return;
            }
            if (ObjectsAddr == nint.Zero)
            {
                Logger.Error("Failed to find Objects");
                return;
            }

            Natives.FNameToString = (delegate*<FName*, FString*, void>)FNameToStringAddr;
            UObject.Objects = new ObjectArray(ObjectsAddr, UseNewObjects);

            var tempDel = new Natives.ProcessEventDelegate(ProcessEventHook);
            yestes.Add(tempDel);
            Minhook.CreateAndEnableHook<Natives.ProcessEventDelegate>(ProcessEventAddr, tempDel, out Natives.ProcessEvent);

            // Tested on 4.1, 8.51, 13.40
            if (Utils.EngineVersion < 4.22)
                Offsets.SuperStruct = 48;
            else
                Offsets.SuperStruct = 64;
            
            Offsets.Children = Offsets.SuperStruct + 8;
            if (Utils.EngineVersion > 4.24)
            {
                Offsets.ChildrenProperties = Offsets.Children + 8;
                Offsets.PropertiesSize = Offsets.ChildrenProperties + 8;
            }
            else
            {
                Offsets.PropertiesSize = Offsets.Children + 8;
            }
            
            // From what i can see UFunction barely changes between versions, so why not go based off UStruct
            var UStructClass = (UStruct*)UObject.FindObject("Class /Script/CoreUObject.Struct");
            var StructSize = UStructClass->PropertiesSize;
            Offsets.FunctionFlags = StructSize;


            foreach (var mod in modules)
            {
                foreach (var method in mod.GetType().GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    var attrib = (ProcessEventHookAttribute?)Attribute.GetCustomAttribute(method, typeof(ProcessEventHookAttribute));
                    if (attrib is not null)
                    {
                        var UFunc = UObject.FindObject(attrib.name);
                        if (UFunc is null)
                        {
                            Logger.Warn($"Tried to hook \"{attrib.name}\" but couldn't find it");
                            continue;
                        }
                        var del = (Natives.PEHookDelegate)method.CreateDelegate(typeof(Natives.PEHookDelegate), null);
                        yestes.Add(del); // shoutout garbage collector
                        PEhooks.Add((nint)UFunc, del);
                    }
                }
            }

            return;
        }

        private static unsafe void ProcessEventHook(UObject* obj, UObject* func, void* args)
        {
            if (PEhooks.TryGetValue((nint)func, out var hookFunc))
            {
                hookFunc(obj, args);
            }

            obj->ProcessEvent(func, args);
        }

        [UnmanagedCallersOnly]
        public static unsafe void Init() // is basically just Setup() from structs.h
        {
            Win32.AllocConsole();

            Logger.Log($"Hello from c# ({Process.GetCurrentProcess().Id})");

            var ModulesPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ScripterSharp", "Modules");
            Logger.Log($"Module path: {ModulesPath}");
            Directory.CreateDirectory(ModulesPath);

            AssemblyLoader = new ScripterAssemblyLoadContext(ModulesPath);

            foreach (var dll in Directory.GetFiles(ModulesPath, "*.dll"))
            {
                // TODO: Look into unloading the modules
                foreach (var typ in AssemblyLoader.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(dll))).GetExportedTypes())
                {
                    if (typ.IsSubclassOf(typeof(BaseModule)))
                    {
                        BaseModule? mod = (BaseModule?)Activator.CreateInstance(typ);
                        if (mod is null)
                        {
                            Logger.Warn($"Tried to create module {typ.Name} but failed");
                            continue;
                        }

                        modules.Add(mod);

                        Logger.Log($"Initialized module: {mod.Name}");

                        mod.OnInit();
                    }
                }
            }

            if (Minhook.Initialize() != 0)
            {
                Logger.Error("Failed to initialize minhook");
                return;
            }

            new Thread(Setup).Start(); // TODO: You should be able to inject on process start and nothing should go wrong
            new Thread(ScripterGui.Start).Start();
        }
    }
}