namespace ByRef.OutPerameter.Ref
{
    using System;

    class Program
    {
        static void Main()
        {
            int a = 0; // この 0 という値には意味はないけど、必須
            int b = 0; // 同上
            MultipleReturns(ref a, ref b); // a, b を
            Console.Write("{0}\n", a);
        }

        static void MultipleReturns(ref int a, ref int b)
        {
            a = 10; // a を初期化
            // 本当は b も初期化してやらないといけないけど、忘れててバグってる
        }
    }
}
