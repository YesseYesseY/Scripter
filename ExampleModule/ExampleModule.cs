using System.Runtime.InteropServices;
using ScripterSharpCommon;
using ScripterSharpCommon.UE;

namespace ExampleModule
{
    public unsafe class ExampleModule : BaseModule
    {
        public override string Name => "Example Module";

        public override void OnInit()
        {
            Logger.Log("On Init");
        }

        public override void OnLoad()
        {
            Logger.Log("On Load");
        }

        public override void OnGui()
        {
            if (ImGui.Button("Create Console"))
            {
                var Engine = (UEngine*)UObject.FindObject("FortEngine_");
                var GSC = (UGameplayStatics*)UObject.FindObject("GameplayStatics /Script/Engine.Default__GameplayStatics");
                var ConsoleClass = UObject.FindObject("Class /Script/Engine.Console");
                Engine->GameViewport->ViewportConsole = GSC->SpawnObject(ConsoleClass, (UObject*)Engine->GameViewport);
            }
        }

#if false
        [ProcessEventHook("Function /Script/Engine.GameMode.ReadyToStartMatch")]
        static unsafe void TestEventHook(UObject* obj, void* argPtr)
        {
            Logger.Log($"{obj->GetName()} calling ReadyToStartMatch: {*(bool*)argPtr}");
        }
#endif
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
            if (Func_SpawnObject is null) Func_SpawnObject = UObject.FindObject("Function /Script/Engine.GameplayStatics.SpawnObject");
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
}
