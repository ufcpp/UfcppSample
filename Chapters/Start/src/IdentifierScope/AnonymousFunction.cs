
namespace IdentifierScope.AnonymousFunction
{
    using System;

    class Sample
    {
        public int Value { get; }

        public Sample(int value)
        {
            Value = value;
        }
        ~Sample()
        {
            Console.WriteLine("SampleがGCされました");
        }
    }

    public class Program
    {
        public static Func<int> M()
        {
            Func<int> f;
            {
                var s = new Sample(1);
                f = () => s.Value;
                // 変数 s のスコープはここまで
            }

            // でも、f が内部で s を参照しているので、インスタンスの寿命が延びる
            // 変数 s のスコープを超えて、f のスコープ内でずっと生き残る
            // GC 起動しても回収されず
            GC.Collect();

            return f;
        }
    }
}
