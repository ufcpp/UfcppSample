namespace DelegateInternal
{
    class Sample
    {
        int _a;
        public Sample(int a = 2) => _a = a;

        public static int Static(int x) => 2 * x;
        public int Instance(int x) => _a * x;
    }

    class Program
    {
        static void Main()
        {
#if false
            // インスタンス メソッドから生成
            var x = new Sample();
            F i = x.Instance;

            // 静的メソッドから生成
            F s = Sample.Static;

            i(10);
            s(20);
#else
            // インスタンス メソッドから生成
            var x = new Sample();
            F i = new F(x.Instance);

            // 静的メソッドから生成
            F s = new F(Sample.Static);

            i.Invoke(10);
            s.Invoke(20);
#endif
        }
    }
}
