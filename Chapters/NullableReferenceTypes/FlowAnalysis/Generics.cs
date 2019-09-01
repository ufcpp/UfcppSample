namespace FlowAnalysis.Generics
{
    namespace Struct
    {
        class Program
        {
            static void M<T>(T? x)
                where T : struct { }

            static void Main()
            {
                M<int>(0);
#if ERROR
                M<int?>(0);
                M<string>("");
                M<string?>(null);
#endif
            }
        }
    }

    namespace Class
    {
        class Program
        {
            static void M<T>(T? x)
                where T : class
            { }

            static void Main()
            {
#if ERROR
                M<int>(0);
                M<int?>(0);
#endif
                M<string>("");
                M<string?>(null);
            }
        }
    }

    namespace NullableClass
    {
        class Program
        {
            static void M<T>(T x)
                where T : class?
            { }

            static void Main()
            {
                M<string>("");
                M<string?>(null);
            }

#if ERROR
            static void M1<T>(T? x)
                where T : class ?
            { }
#endif
        }
    }

    namespace NotNull
    {
        class Program
        {
            static void M<T>(T x)
                where T : notnull
            { }

            static void Main()
            {
                M<int>(0);
                M<int?>(0);
                M<string>("");
                M<string?>(null);
            }

#if ERROR
            static void M1<T>(T? x)
                where T : notnull
            { }
#endif
        }
    }
}
