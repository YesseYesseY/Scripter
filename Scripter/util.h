#include <Windows.h>

#include <UE/structs.h>
#include <filesystem>

namespace fs = std::filesystem;

bool Inject(const std::string& DLLName) // TODO: Remake // skidded go brr https://github.com/LouisTheXIV/DLL-Injection-Cpp/blob/main/Injector/Injector/Injector/Injector.cpp
{
	char dll_path[MAX_PATH];
	
	if (!GetFullPathNameA(DLLName.c_str(), MAX_PATH, dll_path, nullptr)) {
		MessageBoxA(0, _("Failed to get full path of DLL!"), _("GetFullPathName"), MB_ICONERROR);
		return false;
	}
	
	auto ProcID = GetCurrentProcessId();
	
	if (!ProcID) {
		MessageBoxA(0, _("Failed to get process id!"), _("GetProcessID"), MB_ICONERROR);
		return false;
	}

	HANDLE h_process = OpenProcess(PROCESS_ALL_ACCESS, NULL, ProcID);
	
	if (!h_process) {
		MessageBoxA(0, _("Failed to open a handle to process!"), _("OpenProcess"), MB_ICONERROR);
		return false;
	}
	
	void* allocated_memory = VirtualAllocEx(h_process, nullptr, MAX_PATH, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
	
	if (!allocated_memory) {
		MessageBoxA(0, _("Failed to allocate memory!"), _("AllocateMemory"), MB_ICONERROR);
		return false;
	}

	if (!WriteProcessMemory(h_process, allocated_memory, dll_path, MAX_PATH, nullptr)) {
		MessageBoxA(0, _("Failed to write to process memory!"), _("WriteProcessMemory"), MB_ICONERROR);
		return false;
	}

	HANDLE h_thread = CreateRemoteThread(h_process, nullptr, NULL, LPTHREAD_START_ROUTINE(LoadLibraryA), allocated_memory, NULL, nullptr);
	if (!h_thread) {
		MessageBoxA(0, _("Failed to create remote thread!"), _("CreateRemoteThread"), MB_ICONERROR);
		return false;
	}

	CloseHandle(h_process);
	VirtualFreeEx(h_process, allocated_memory, NULL, MEM_RELEASE);
	
	return true;
}

bool ExecuteCSharp(const std::wstring& DLLName) // https://github.com/Vacko/Managed-code-injection/blob/master/Bootstrap/DllMain.cpp // TODO: Return error string instead.
{
	// Idk if i wanna remake this, honestly feels easier to make in c#
}

namespace AppData
{
	static fs::path Path;

	bool Init()
	{
		char* buf{};
		size_t size = MAX_PATH;
		_dupenv_s(&buf, &size, _("LOCALAPPDATA"));
		Path = fs::path(buf) / _("Scripts");
		
		bool res = true;

		if (!fs::exists(Path))
			res = fs::create_directory(Path);

		return res;
	}
}

enum Languages
{
	CSharp,
	UNKNOWN
};

Languages ConvertLanguage(std::string Lang)
{
	std::transform(Lang.begin(), Lang.end(), Lang.begin(), ::tolower);

	if (Lang == _("c#") || Lang == _("csharp") || Lang == _("c sharp") || Lang == _("cs"))
		return Languages::CSharp;

	return Languages::UNKNOWN;
}

std::string AddExtension(const std::string& Path, Languages lang)
{	
	if (Path.contains("."))
		return Path;
	
	switch (lang)
	{
	case Languages::CSharp:
		return Path + ".dll";
	default:
		return Path;
	}
}