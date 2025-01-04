#pragma once
#include <Windows.h>

#include <coreclr_delegates.h>
#include <hostfxr.h>
#include <nethost.h>
#include <iostream>

// mostly from https://github.com/dotnet/samples/tree/main/core/hosting/src
// rn you need to insall nethost in vcpkg "vcpkg.exe install nethost:x64-windows-static"

namespace DotNet
{
    wchar_t dllpath[MAX_PATH];

    hostfxr_initialize_for_dotnet_command_line_fn init_for_cmd_line_fptr;
    hostfxr_initialize_for_runtime_config_fn init_for_config_fptr;
    hostfxr_get_runtime_delegate_fn get_delegate_fptr;
    hostfxr_run_app_fn run_app_fptr;
    hostfxr_close_fn close_fptr;

    using string_t = std::basic_string<wchar_t>;

    bool load_hostfxr(const wchar_t* assembly_path)
    {
        get_hostfxr_parameters params{ sizeof(get_hostfxr_parameters), assembly_path, nullptr };
        // Pre-allocate a large buffer for the path to hostfxr
        wchar_t buffer[MAX_PATH];
        size_t buffer_size = sizeof(buffer) / sizeof(wchar_t);
        int rc = get_hostfxr_path(buffer, &buffer_size, &params);
        if (rc != 0)
            return false;

        // Load hostfxr and get desired exports
        // NOTE: The .NET Runtime does not support unloading any of its native libraries. Running
        // dlclose/FreeLibrary on any .NET libraries produces undefined behavior.
        void* lib = (void*)LoadLibraryW(buffer);
        init_for_cmd_line_fptr = (hostfxr_initialize_for_dotnet_command_line_fn)GetProcAddress((HMODULE)lib, "hostfxr_initialize_for_dotnet_command_line");

        init_for_config_fptr = (hostfxr_initialize_for_runtime_config_fn)GetProcAddress((HMODULE)lib, "hostfxr_initialize_for_runtime_config");

        get_delegate_fptr = (hostfxr_get_runtime_delegate_fn)GetProcAddress((HMODULE)lib, "hostfxr_get_runtime_delegate");

        run_app_fptr = (hostfxr_run_app_fn)GetProcAddress((HMODULE)lib, "hostfxr_run_app");

        close_fptr = (hostfxr_close_fn)GetProcAddress((HMODULE)lib, "hostfxr_close");
        return (init_for_config_fptr && get_delegate_fptr && close_fptr);
    }

    load_assembly_and_get_function_pointer_fn get_dotnet_load_assembly(const char_t* config_path)
    {
        // Load .NET Core
        void* load_assembly_and_get_function_pointer = nullptr;
        hostfxr_handle cxt = nullptr;
        int rc = init_for_config_fptr(config_path, nullptr, &cxt);
        if (rc != 0 || cxt == nullptr)
        {
            std::cerr << "Init failed: " << std::hex << std::showbase << rc << std::endl;
            close_fptr(cxt);
            return nullptr;
        }

        // Get the load assembly function pointer
        rc = get_delegate_fptr(
            cxt,
            hdt_load_assembly_and_get_function_pointer,
            &load_assembly_and_get_function_pointer);
        if (rc != 0 || load_assembly_and_get_function_pointer == nullptr)
            std::cerr << "Get delegate failed: " << std::hex << std::showbase << rc << std::endl;

        close_fptr(cxt);
        return (load_assembly_and_get_function_pointer_fn)load_assembly_and_get_function_pointer;
    }

    void Init()
    {
        string_t root_path = fs::absolute(std::wstring(dllpath) + L"\\..\\ScripterSharp\\");
        //
        // STEP 1: Load HostFxr and get exported hosting functions
        //
        if (!load_hostfxr(nullptr))
        {
            std::cout << "Failure: load_hostfxr()\n";
            return;
        }

        //
        // STEP 2: Initialize and start the .NET Core runtime
        //
        const string_t config_path = root_path + L"ScripterSharp.runtimeconfig.json";
        load_assembly_and_get_function_pointer_fn load_assembly_and_get_function_pointer = nullptr;
        load_assembly_and_get_function_pointer = get_dotnet_load_assembly(config_path.c_str());
        if (load_assembly_and_get_function_pointer == nullptr) std::cout << "Failure: get_dotnet_load_assembly()\n";

        //
        // STEP 3: Load managed assembly and get function pointer to a managed method
        //
        const string_t dotnetlib_path = root_path + L"ScripterSharp.dll";
        const wchar_t* dotnet_type = L"ScripterSharp.Scripter, ScripterSharp";

        // Function pointer to managed delegate with non-default signature
        typedef void (CORECLR_DELEGATE_CALLTYPE* custom_entry_point_fn)();
        custom_entry_point_fn initcs = nullptr;

        // UnmanagedCallersOnly
        int rc = load_assembly_and_get_function_pointer(
            dotnetlib_path.c_str(),
            dotnet_type,
            L"Init" /*method_name*/,
            UNMANAGEDCALLERSONLY_METHOD,
            nullptr,
            (void**)&initcs);
        if (!(rc == 0 && initcs != nullptr)) std::cout << "Failure: load_assembly_and_get_function_pointer()\n";
        initcs();
    }
}