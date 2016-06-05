using RefReturns.ValueTypePassedByReference;
using System;

namespace RefReturns
{
    class Program
    {
        static void Main(string[] args)
        {
            CircularBufferSample.Main();
            ValueTypePassedByReference.Program.Main();
            RefReturns.Program.Main();
            RefLocal.Program.Main();
        }

        static void Caller()
        {
            var x = 10;
            ref var y = ref Ref(ref x);
            y = 0; // y は巡り巡って x を参照。x も 0 に

            Console.WriteLine($"{x}, {y}"); // 0, 0
        }

        static ref int Ref(ref int x) => ref x;

#if false

        // 参照引数は参照戻り値で返せる
        private static ref int Success1(ref int x) => ref x;

        // 値渡しの引数はダメ
        private static ref int Error1(int x) => ref x;

        // ローカル変数はダメ
        private static ref int Error2()
        {
            var x = int.Parse(Console.ReadLine());
            return ref x;
        }

        // 多段の場合も元をたどって出所を調べてくれる
        private static ref int Success1(ref int x, ref int y)
        {
            ref int r1 = ref x;
            ref int r2 = ref y;
            ref int r3 = ref Max(ref r1, ref r2);

            // r3 は出所をたどると引数の x か y の参照
            // x も y も参照引数なので大丈夫
            return ref r3;
        }

        private static ref int Error1(ref int x, int y)
        {
            ref int r1 = ref x;
            ref int r2 = ref y;
            ref int r3 = ref Max(ref r1, ref r2);

            // y が値渡しなのでダメ
            return ref r3;
        }

        private static ref int Error2(ref int x)
        {
            var y = int.Parse(Console.ReadLine());
            ref int r1 = ref x;
            ref int r2 = ref y;
            ref int r3 = ref Max(ref r1, ref r2);

            // y がローカル変数なのでダメ
            return ref r3;
        }

#endif

        private static ref int Max(ref int x, ref int y)
        {
            if (x >= y) return ref x;
            else return ref y;
        }

        private static int MaxIndex(int[] array, int i, int j)
        {
            if (array[i] >= array[j]) return i;
            else return j;
        }

        private static void Old()
        {
            var data = new[] { 3, 5, 2, 1, 4 };
            var m = MaxIndex(data, 0, 1);
            data[m] = 0;
            Console.WriteLine($"{data[0]}, {data[1]}");
        }

        private static void New()
        {
            var data = new[] { 3, 5, 2, 1, 4 };
            ref var m = ref Max(ref data[0], ref data[1]);
            m = 0;
            Console.WriteLine($"{data[0]}, {data[1]}");
        }
    }
}
