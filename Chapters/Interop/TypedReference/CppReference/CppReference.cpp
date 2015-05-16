#include "stdafx.h"

#include "stdio.h"

void sample()
{
    int x = 10;
    int& r = x; // x ‚ÌQÆ‚ğì‚é

    r = 99; // QÆŒ³‚Ì x ‚à‘‚«Š·‚í‚é

    printf("%d", x); // 99
}

int _tmain(int argc, _TCHAR* argv[])
{
    sample();
	return 0;
}

