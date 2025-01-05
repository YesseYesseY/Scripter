#pragma once

#include <UE/structs.h>
#include "dotnet.h"

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
	//DLL_EXPORT UObject* FindObject(const char* name)
	//{
	//	return bruh(name);
	//}

	//DLL_EXPORT const char* GetFullName(UObject* Object)
	//{
	//	return Object->GetFullName().c_str();
	//}

	//DLL_EXPORT void* Member(UObject* Object, const char* MemberName)
	//{
	//	return Object->Member<void>(MemberName);
	//}
	//
	//DLL_EXPORT void* Function(UObject* Object, const char* FunctionName)
	//{
	//	return Object->Function(FunctionName);
	//}

	//DLL_EXPORT auto ProcessEvent(UObject* Object, UObject* Function, void* Params)
	//{
	//	// if (Object && Function)
	//	return Object->ProcessEvent(Function, Params);
	//}
	//
	//DLL_EXPORT auto ProcessEventStr(UObject* Object, const char* FuncName, void* Params)
	//{
	//	return Object->ProcessEvent(FuncName, Params);
	//}
	//
	//DLL_EXPORT void cout(const char* str)
	//{
	//	std::cout << str << '\n';
	//}
}