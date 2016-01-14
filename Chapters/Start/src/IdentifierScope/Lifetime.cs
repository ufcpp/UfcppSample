
namespace IdentifierScope.Lifetime
{
    using System;

    class Sample
    {
        public Sample()
        {
            Console.WriteLine("Sampleが作られました");
        }
        ~Sample()
        {
            Console.WriteLine("SampleがGCされました");
        }
    }

    public class Program
    {
        public static void M()
        {
            {
                Console.WriteLine("Scope開始");
                var s = new Sample();

                // この時点ではまだ生きているので、GC しても無駄
                GC.Collect();

                Console.WriteLine("Scope終了");
            }

            // この時点で s に入っていた Sample インスタンスは寿命迎えてる
            // GC を強制起動すると回収されるはず
            GC.Collect();
        }
    }
}
