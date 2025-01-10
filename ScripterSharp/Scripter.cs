﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using ScripterSharp.UE;

namespace ScripterSharp
{
    public static class Scripter
    {
        public static ObjectArray Objects;

        public static string FortniteVersionString = "";
        public static double FortniteVersion;
        public static string EngineVersionString = "";
        public static double EngineVersion;

        public static void Print(string str) => Natives.CSharpPrint(str);
        public static nint FindPattern(string signature, bool bRelative = false, uint offset = 0, bool bIsVar = false) => new nint(Natives.FindPatternC(signature, bRelative, offset, bIsVar));
        public static unsafe void AddProcessEventHook(UObject* func, Natives.PEHookDelegate csfunc) => Natives.AddProcessEventHook(func, csfunc);

        public static unsafe UObject* FindObject(string name)
        {
            foreach (UObject* obj in Objects)
            {
                if (obj->GetFullName().Contains(name)) return obj;
            }
            return null;
        }

        static List<Delegate> yestes = new List<Delegate>(); 
        private static unsafe bool Setup()
        {
            var GetEngineAddr = FindPattern("40 53 48 83 EC 20 48 8B D9 E8 ? ? ? ? 48 8B C8 41 B8 04 ? ? ? 48 8B D3");
            if (GetEngineAddr == nint.Zero)
            {
                Print("Couldn't find GetEngineVersion");
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
                    Print(e.Message);
                    Print(FortniteVersionString);
                    Print(EngineVersionString);
                    return false;
                }

            }

            var UseNewObjects = true;

            nint ObjectsAddr = nint.Zero;
            nint ProcessEventAddr = nint.Zero;
            nint FNameToStringAddr = nint.Zero;

            if (EngineVersion >= 416 && EngineVersion <= 420)
            {
                ObjectsAddr = FindPattern("48 8B 05 ? ? ? ? 48 8D 1C C8 81 4B ? ? ? ? ? 49 63 76 30", false, 7, true);

                if (ObjectsAddr == nint.Zero)
                    ObjectsAddr = FindPattern("48 8B 05 ? ? ? ? 48 8D 14 C8 EB 03 49 8B D6 8B 42 08 C1 E8 1D A8 01 0F 85 ? ? ? ? F7 86 ? ? ? ? ? ? ? ?", false, 7, true);

                if (EngineVersion == 420)
                    FNameToStringAddr = FindPattern("48 89 5C 24 ? 57 48 83 EC 40 83 79 04 00 48 8B DA 48 8B F9 75 23 E8 ? ? ? ? 48 85 C0 74 19 48 8B D3 48 8B C8 E8 ? ? ? ? 48");
                else
                {
                    FNameToStringAddr = FindPattern("40 53 48 83 EC 40 83 79 04 00 48 8B DA 75 19 E8 ? ? ? ? 48 8B C8 48 8B D3 E8 ? ? ? ?");

                    if (FNameToStringAddr == nint.Zero) // This means that we are in season 1 (i think).
                    {
                        FNameToStringAddr = FindPattern("48 89 5C 24 ? 48 89 74 24 ? 48 89 7C 24 ? 41 56 48 83 EC 20 48 8B DA 4C 8B F1 E8 ? ? ? ? 4C 8B C8 41 8B 06 99");

                        if (FNameToStringAddr != nint.Zero)
                            EngineVersion = 416;
                    }
                }

                ProcessEventAddr = FindPattern("40 55 56 57 41 54 41 55 41 56 41 57 48 81 EC ? ? ? ? 48 8D 6C 24 ? 48 89 9D ? ? ? ? 48 8B 05 ? ? ? ? 48 33 C5 48 89 85 ? ? ? ? 48 63 41 0C 45 33 F6");

                UseNewObjects = false;
            }

            if (EngineVersion >= 421 && EngineVersion <= 424)
            {
                FNameToStringAddr = FindPattern("48 89 5C 24 ? 57 48 83 EC 30 83 79 04 00 48 8B DA 48 8B F9");
                ProcessEventAddr = FindPattern("40 55 56 57 41 54 41 55 41 56 41 57 48 81 EC ? ? ? ? 48 8D 6C 24 ? 48 89 9D ? ? ? ? 48 8B 05 ? ? ? ? 48 33 C5 48 89 85 ? ? ? ? ? ? ? 45 33 F6");
            }

            if (EngineVersion >= 425 && EngineVersion < 500)
            {
                FNameToStringAddr = FindPattern("48 89 5C 24 ? 55 56 57 48 8B EC 48 83 EC 30 8B 01 48 8B F1 44 8B 49 04 8B F8 C1 EF 10 48 8B DA 0F B7 C8 89 4D 24 89 7D 20 45 85 C9");
                ProcessEventAddr = FindPattern("40 55 56 57 41 54 41 55 41 56 41 57 48 81 EC ? ? ? ? 48 8D 6C 24 ? 48 89 9D ? ? ? ? 48 8B 05 ? ? ? ? 48 33 C5 48 89 85 ? ? ? ? 8B 41 0C 45 33 F6");
            }

            if (EngineVersion >= 421 && EngineVersion <= 426)
            {
                ObjectsAddr = FindPattern("48 8B 05 ? ? ? ? 48 8B 0C C8 48 8D 04 D1 EB 03 48 8B ? 81 48 08 ? ? ? 40 49", false, 7, true);

                if (ObjectsAddr == nint.Zero)
                    ObjectsAddr = FindPattern("48 8B 05 ? ? ? ? 48 8B 0C C8 48 8B 04 D1", true, 3);
            }

            if (FortniteVersion >= 16.00 && FortniteVersion < 18.40) // 4.26.1
            {
                ObjectsAddr = FindPattern("48 8B 05 ? ? ? ? 48 8B 0C C8 48 8B 04 D1", true, 3);
            }

            if (FNameToStringAddr == nint.Zero)
            {
                Print("Failed to find FNameToString");
                return false;
            }
            if (ProcessEventAddr == nint.Zero)
            {
                Print("Failed to find ProcessEvent");
                return false;
            }
            if (ObjectsAddr == nint.Zero)
            {
                Print("Failed to find Objects");
                return false;
            }


            Natives.FNameToString = (delegate*<FName*, FString*, void>)FNameToStringAddr;
            Natives.ProcessEvent = (delegate*<UObject*, UObject*, void*, void>)ProcessEventAddr;
            Objects = new ObjectArray(ObjectsAddr, UseNewObjects);

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
            var UStructClass = (UStruct*)FindObject("Class /Script/CoreUObject.Struct");
            var StructSize = UStructClass->PropertiesSize;
            Offsets.FunctionFlags = StructSize;

            // This should be simpler when "plugins" are implemented
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        var attrib = (ProcessEventHookAttribute?)Attribute.GetCustomAttribute(method, typeof(ProcessEventHookAttribute));
                        if (attrib != null)
                        {
                            Print("Found it");
                            var del = (Natives.PEHookDelegate)method.CreateDelegate(typeof(Natives.PEHookDelegate), null);
                            //GC.KeepAlive(del);
                            yestes.Add(del); // shoutout garbage collector
                            AddProcessEventHook(FindObject(attrib.name), del);
                        }
                    }
                }
            }

            return true;
        }

        [UnmanagedCallersOnly]
        public static unsafe void Init() // is basically just Setup() from structs.h
        {
            Print("Hello from c#");

            if (!Setup())
            {
                Print("Failed setup");
                return;
            }

            //DumpObjects();
            CreateConsole();
            new Thread(ScripterGui.Start).Start();
        }

        [ProcessEventHook("Function /Script/Engine.GameMode.ReadyToStartMatch")]
        static unsafe void TestEventHook(UObject* obj, void* argPtr)
        {
            Print($"{obj->GetName()} calling ReadyToStartMatch: {*(bool*)argPtr}");
        }

        static void DumpOffsets<T>()
        {
            foreach (var field in typeof(T).GetFields())
            {
                Print($"{field.Name}: {Marshal.OffsetOf<T>(field.Name)}");
            }
        }
        [StructLayout(LayoutKind.Explicit, Size = 40)]
        public unsafe struct UGameplayStatics // : UBlueprintFunctionLibrary
        {
            [FieldOffset(0)] private UObject _obj;

            [StructLayout(LayoutKind.Sequential)]
            private struct Params_SpawnObject
            {
                public UObject* ObjectClass;
                public UObject* Outer;
                public UObject* ReturnValue;
            }
            private static UObject* Func_SpawnObject;
            public unsafe UObject* SpawnObject(UObject* ObjectClass, UObject* Outer)
            {
                var args = new Params_SpawnObject();
                args.ObjectClass = ObjectClass;
                args.Outer = Outer;
                if (Func_SpawnObject == null) Func_SpawnObject = Scripter.FindObject("Function /Script/Engine.GameplayStatics.SpawnObject");
                _obj.ProcessEvent(Func_SpawnObject, &args);
                return args.ReturnValue;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        unsafe struct UEngine
        {
            private UObject _obj;

            private static int _GameViewport = -1;
            public UGameViewportClient* GameViewport
            {
                get => *(UGameViewportClient**)_obj.GetPtrOffset(_GameViewport == -1 ? _GameViewport = _obj.GetChildOffset("GameViewport") : _GameViewport);
                set => *(UGameViewportClient**)_obj.GetPtrOffset(_GameViewport == -1 ? _GameViewport = _obj.GetChildOffset("GameViewport") : _GameViewport) = value;
            }
        }
        [StructLayout(LayoutKind.Explicit)]
        unsafe struct UGameViewportClient
        {
            [FieldOffset(0)] private UObject _obj;
            
            private static int _ViewportConsole = -1;
            public UObject* ViewportConsole
            {
                get => *(UObject**)_obj.GetPtrOffset(_ViewportConsole == -1 ? _obj.GetChildOffset("ViewportConsole") : _ViewportConsole);
                set => *(UObject**)_obj.GetPtrOffset(_ViewportConsole == -1 ? _obj.GetChildOffset("ViewportConsole") : _ViewportConsole) = value;
            }

        }

        static unsafe void CreateConsole()
        {
            var Engine = (UEngine*)FindObject("FortEngine_");
            var GSC = (UGameplayStatics*)FindObject("GameplayStatics /Script/Engine.Default__GameplayStatics");
            var ConsoleClass = FindObject("Class /Script/Engine.Console");
            Engine->GameViewport->ViewportConsole = GSC->SpawnObject(ConsoleClass, (UObject*)Engine->GameViewport);
        }

        public unsafe static void DumpObjects()
        {
            string objFilePath = "C:/FN/obj.txt";
            Print($"Writing ~{Objects.Num} objects to {objFilePath}");
            using (var stream = File.OpenWrite(objFilePath))
            {
                foreach (UObject* obj in Objects)
                {
                    stream.Write(Encoding.UTF8.GetBytes($"{obj->GetFullName()}\n"));
                }
            }
            Print("Finished!");
        }
    }
}