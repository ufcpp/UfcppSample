int x;
(x, var u) = (1, 2);

Console.WriteLine(x);
Console.WriteLine(u);

#if ERROR
void m()
{
    // 式の途中に分解宣言 (var 付きの宣言) が来るのは C# 10.0 でもダメ。
    int x, y;
    (x, var u) = (var v, y) = (1, 2);
}
#endif
