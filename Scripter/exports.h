#pragma once

#include <MinHook.h>
#include "dotnet.h"
#include <d3d9.h>
#pragma comment(lib, "d3d9.lib")

#define DLL_EXPORT _declspec(dllexport)

extern "C" {
	//////////////// MINHOOK ////////////////
	
	DLL_EXPORT MH_STATUS MH__Initialize()
	{
		return MH_Initialize();
	}
	
	DLL_EXPORT MH_STATUS MH__CreateHook(LPVOID pTarget, LPVOID pDetour, LPVOID* ppOriginal)
	{
		return MH_CreateHook(pTarget, pDetour, ppOriginal);
	}
	
	DLL_EXPORT MH_STATUS MH__EnableHook(LPVOID pTarget)
	{
		return MH_EnableHook(pTarget);
	}

	//////////////// DXD9 ////////////////

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