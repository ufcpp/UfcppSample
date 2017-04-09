namespace TupleMutableStruct.Usage
{
    class Program
    {
        static void Variables()
        {
            var x = 1;
            var y = 2;

            // C# の変数は書き換え可能だし、
            // ref/out 越しに参照できる
            x = 10;
            int.TryParse("123", out x);
            ref var r = ref y;
            r = 456;
        }

        // 引数に対して
        static void Parameters(int x, int y)
        {
            int.TryParse("123", out x);
            ref var r = ref y;
            r = 456;
        }

        static void Deconstruction()
        {
            // タプルの分解で得た変数に対して
            var (x, y) = (1, 2);

            x = 10;
            int.TryParse("123", out x);
            ref var r = ref y;
            r = 456;
        }

        static void Tuple()
        {
            // タプル自体に対して
            var t = (x: 1, y: 2);

            t.x = 10;
            int.TryParse("123", out t.x);
            ref var r = ref t.y;
            r = 456;
        }
    }
}
