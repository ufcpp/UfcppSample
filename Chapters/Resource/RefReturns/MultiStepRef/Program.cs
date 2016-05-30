namespace RefReturns.MultiStepRef
{
    using System;

    class Program
    {
        public static void Main()
        {
            var x = 10;
            ref var y = ref Ref(ref x);
            y = 0; // y は巡り巡って x を参照。x も 0 に

            Console.WriteLine($"{x}, {y}"); // 0, 0
        }

        static ref int Ref(ref int x) => ref x;
    }
}
