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