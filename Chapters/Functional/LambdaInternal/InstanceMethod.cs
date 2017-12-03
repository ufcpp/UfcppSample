using System;

namespace LambdaInternal.InstanceMethod
{
    class Sample
    {
        public int X { get; set; }

        public void M()
        {
            Func<int> f = () => X;
            Console.WriteLine(f());
        }
    }

    namespace Compiled
    {
        class Sample
        {
            public int X { get; set; }

            public void M()
            {
                Func<int> f = AnonymousMethod1;
                Console.WriteLine(f());
            }

            private int AnonymousMethod1() => X;
        }
    }
}
