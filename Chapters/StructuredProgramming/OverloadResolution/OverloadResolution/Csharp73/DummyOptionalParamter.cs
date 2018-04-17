namespace OverloadResolution.Csharp73.DummyOptionalParamter
{
    class Program
    {
        // 呼び分け用のダミー型
        struct Struct { }
        struct Class { }

        // ダミー引数を足すことでオーバーロードする。
        static void M<T>(T x, Struct _ = default) where T : struct { }
        static void M<T>(T x, Class _ = default) where T : class { }

        static void Main()
        {
            M(1);     // M(T, Struct) が呼ばれる
            M("abc"); // M(T, Class) が呼ばれる
        }
    }
}
