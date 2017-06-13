// NativeLib.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

extern "C"
{
	typedef __declspec(dllexport) void (*Callback)(__int32 senderId, void* data, size_t data_len);

	static Callback g_callback;

	// Trigger を呼んだ時に、callback が呼び出される。
	// callback に渡した data ポインターは、通常であれば callback 内でだけ有効。
	// callback 内を超えて有効にするためには AddRef を呼ぶ(この場合、Release をちゃんと呼ばないとメモリリークする)。
	__declspec(dllexport) void SetCallback(Callback callback)
	{
		g_callback = callback;
	}

	// 本来 NativeLib 内で起きたイベントに応じて callback が呼ばれるような想定なんだけど、
	// サンプルということで外からイベントを起こしてもらう。
	// callback に渡す data も適当。乱数でも入れておく。
	__declspec(dllexport) void Trigger(__int32 senderId)
	{
		auto data_len = std::rand() % 1024;

		__int8* p = new __int8[data_len + sizeof(size_t)];
		__int8* data = p + +sizeof(size_t);

		for (int i = 0; i < data_len; i++)
		{
			data[i] = rand();
		}

		g_callback(senderId, data, data_len);

		// AddRef が呼ばれてないときに限り即座に delete

		// ほんとはたぶんロックが必要
		auto refCount = (size_t*)p;
		if (*refCount == 0)
			delete data;
	}

	__declspec(dllexport) void AddRef(void* data)
	{
		auto refCount = (size_t*)((__int8*)data - sizeof(size_t));

		// ほんとはたぶんロックが必要
		*refCount++;
	}

	__declspec(dllexport) void Release(void* data)
	{
		auto refCount = (size_t*)((__int8*)data - sizeof(size_t));

		// ほんとはたぶんロックが必要
		if (*refCount-- == 0)
		{
			delete (__int8*)refCount;
		}
	}
}
