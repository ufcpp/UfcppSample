#:property AllowUnsafeBlocks=true

// unsafe ブロックはオプションをつけないと使えない構文。
unsafe
{
    int n = 1;
    int* pn = &n;
    Console.WriteLine($"{(nint)pn:x}");
}
