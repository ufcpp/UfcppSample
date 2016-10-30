// Win32Dll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

extern "C"
{
	__declspec(dllexport) void __stdcall FillA16(wchar_t* str)
	{
		for (auto p = str; *p; p++)
		{
			*p = L'a';
		}
	}

	__declspec(dllexport) void __stdcall FillA8(char* str)
	{
		for (auto p = str; *p; p++)
		{
			*p = 'a';
		}
	}

	__declspec(dllexport) int __stdcall GetValue()
	{
		return 123;
	}
}
