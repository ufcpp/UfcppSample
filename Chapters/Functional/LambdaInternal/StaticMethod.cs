using System;

namespace LambdaInternal.StaticMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int> f1 = () => 0;
            f1();
        }
    }

    namespace OldCompiled
    {
        class Program
        {
            static int AnonymousMethod1()
            {
                return 0;
            }

            static void Main(string[] args)
            {
                Func<int> f1 = AnonymousMethod1;
                f1();
            }
        }
    }

    namespace NewCompiled
    {
        class Program
        {
            class AnonymousClass
            {
                public static readonly AnonymousClass Singleton = new AnonymousClass();
                public static Func<int>? Cache1;

                internal int AnonymousMethod1()
                {
                    return 0;
                }
            }

            static void Main(string[] args)
            {
                Func<int> f1 = AnonymousClass.Cache1 ??= AnonymousClass.Singleton.AnonymousMethod1;
                f1();
            }
        }
    }
}
