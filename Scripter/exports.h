#pragma once

#include <UE/structs.h>
#include "dotnet.h"
#include <d3d9.h>
#pragma comment(lib, "d3d9.lib")

#define DLL_EXPORT _declspec(dllexport)

extern "C" {
	DLL_EXPORT void AddProcessEventHook(void* func, void(*csfunc)(void*, void*))
	{
		ProcessEventHooks[func] = csfunc;
	}
	
	DLL_EXPORT HRESULT DXD9_CreateDevice(LPDIRECT3D9 thing, UINT Adapter, D3DDEVTYPE DeviceType, HWND hFocusWindow, DWORD BehaviorFlags, D3DPRESENT_PARAMETERS* pPresentationParameters, IDirect3DDevice9** ppReturnedDeviceInterface)
	{
		return thing->CreateDevice(Adapter, DeviceType, hFocusWindow, BehaviorFlags, pPresentationParameters, ppReturnedDeviceInterface);
	}
	
	DLL_EXPORT HRESULT DXD9_SetRenderState(LPDIRECT3DDEVICE9 thing, D3DRENDERSTATETYPE State, DWORD Value)
	{
		return thing->SetRenderState(State, Value);
	}
	
	DLL_EXPORT HRESULT DXD9_Clear(LPDIRECT3DDEVICE9 thing, DWORD Count, CONST D3DRECT* pRects, DWORD Flags, D3DCOLOR Color, float Z, DWORD Stencil)
	{
		return thing->Clear(Count, pRects, Flags, Color, Z, Stencil);
	}
	
	DLL_EXPORT HRESULT DXD9_BeginScene(LPDIRECT3DDEVICE9 thing)
	{
		return thing->BeginScene();
	}
	
	DLL_EXPORT HRESULT DXD9_EndScene(LPDIRECT3DDEVICE9 thing)
	{
		return thing->EndScene();
	}
	
	DLL_EXPORT HRESULT DXD9_TestCooperativeLevel(LPDIRECT3DDEVICE9 thing)
	{
		return thing->TestCooperativeLevel();
	}
	
	DLL_EXPORT HRESULT DXD9_Reset(LPDIRECT3DDEVICE9 thing, D3DPRESENT_PARAMETERS* pPresentationParameters)
	{
		return thing->Reset(pPresentationParameters);
	}
	
	DLL_EXPORT HRESULT DXD9_Present(LPDIRECT3DDEVICE9 thing, CONST RECT* pSourceRect, CONST RECT* pDestRect, HWND hDestWindowOverride, CONST RGNDATA* pDirtyRegion)
	{
		return thing->Present(pSourceRect, pDestRect, hDestWindowOverride, pDirtyRegion);
	}

	DLL_EXPORT void DXD9_ReleaseDevice(LPDIRECT3DDEVICE9 thing)
	{
		thing->Release();
	}
	DLL_EXPORT void DXD9_ReleaseThing(LPDIRECT3D9 thing)
	{
		thing->Release();
	}
}