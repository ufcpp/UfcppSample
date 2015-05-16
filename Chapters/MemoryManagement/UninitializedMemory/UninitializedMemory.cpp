#include <stdio.h>

void main()
{
    int* x = new int[1];
    x[0] = 0xFFFFFFFF;
    printf("%08x\n", x[0]);

    int* px = x;
    delete x;
    printf("%08x\n", px[0]);

    int* y = new int[1];
    printf("%08x\n", y[0]);
}
