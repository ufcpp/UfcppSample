// Win32Dll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

extern "C"
{
	__declspec(dllexport) void __stdcall FillA(wchar_t* str)
	{
		for (auto p = str; *p; p++)
		{
			*p = L'a';
		}
	}

	__declspec(dllexport) int __stdcall GetValue()
	{
		return 123;
	}
}
