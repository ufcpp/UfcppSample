namespace ParameterlessConstructors;

class NewOrDefault
{
    public static void M()
    {
        Console.WriteLine(new A().X); // コンストラクターが呼ばれて、X == 1 になってる。
        Console.WriteLine(default(A).X); // コンストラクターも呼ばれず 0 初期化で、X == 0 になってる。

        // まだ default(A) は「明示的に default (債務不履行) してる」からいいものの…

        // 配列の要素は暗黙的に default…
        Console.WriteLine((new A[1])[0].X); // default(A) と同じ扱いで、X == 0 になってる。
    }

    struct A
    {
        public int X;
        public A() => X = 1;
    }
}
