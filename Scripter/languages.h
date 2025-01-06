#pragma once

#include <UE/structs.h>
#include "dotnet.h"
#include "gui.h"
#pragma comment(lib, "d3d9.lib")

//  https://youtu.be/o-ass4mkdiA

#define DLL_EXPORT _declspec(dllexport)

auto bruh(const char* name)
{
	return FindObject(name);
}

extern "C" {
	DLL_EXPORT uint64_t FindPatternC(const char* signature, bool bRelative, uint32_t offset, bool bIsVar)
	{
		return FindPattern(signature, bRelative, offset, bIsVar);
	}
	DLL_EXPORT void CSharpPrint(const char* str)
	{
		std::cout << "[CSharp] " << str << '\n';
	}
	DLL_EXPORT void AddProcessEventHook(void* func, void(*csfunc)(UObject*, void*))
	{
		ProcessEventHooks[func] = csfunc;
	}
	DLL_EXPORT void InitGui(void(*guifunc)(), LRESULT(*wndproc)(HWND, UINT, WPARAM, LPARAM))
	{
		Init_Gui(guifunc, wndproc);
	}

}