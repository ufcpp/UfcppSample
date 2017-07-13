// Win32Dll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

extern "C"
{
	// UTF-16 null終端文字列
	__declspec(dllexport) void __stdcall FillA16(wchar_t* str)
	{
		for (auto p = str; *p; p++)
		{
			*p = L'a';
		}
	}

	// ANSI null終端文字列
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

	__declspec(dllexport) void __stdcall SetValue(UINT16& x)
	{
		x = 0x1234;
	}

	struct Data
	{
		BYTE A;
		BYTE B;
		UINT16 C;
		UINT32 D;
	};

	__declspec(dllexport) Data __stdcall Shift(Data data)
	{
		UINT64* l = (UINT64*)&data;
		*l <<= 1;
		return data;
	}
}
