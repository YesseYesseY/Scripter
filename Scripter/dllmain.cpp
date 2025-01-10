#include <Windows.h>
#include <filesystem>
#include <fstream>

#include <exports.h>
#include <json.hpp>

#include <zip_file.hpp>
#include "dotnet.h"

using namespace nlohmann;
namespace fs = std::filesystem;

DWORD WINAPI Main(LPVOID)
{
    DotNet::Init();

    if (MH_Initialize() != MH_OK)
    {
        MessageBoxA(0, "Minhook failed to initialize!", "Setup", MB_OK);
        return 0;
    }
    
    if (!Setup())
    {
        MessageBoxA(0, "Failed!", "Setup", MB_OK);
        return 0;
    }
	
    return 0;
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD dllReason, LPVOID lpReserved)
{
    switch (dllReason)
    {
    case DLL_PROCESS_ATTACH:
        GetModuleFileName(hModule, DotNet::dllpath, MAX_PATH);
        DisableThreadLibraryCalls(hModule);
        CreateThread(0, 0, Main, 0, 0, 0);

        break;
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}