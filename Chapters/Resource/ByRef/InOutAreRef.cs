namespace ByRef.InOutAreRef
{
    class Program
    {
#if InvalidCode
        void F(ref int x) { }
        void F(in int x) { }

        void G(ref int x) { }
        void G(out int x) => x = 0;

        void H(in int x) { }
        void H(out int x) => x = 0;
#endif
    }

#if InvalidCode
    interface Contravariance<in T>
    {
        // 普通の引数は共変
        void M(T x);

        // 本来できてもいいはずなものの、.NET 的には無理
        void M(in T x);
    }

    interface Covariance<out T>
    {
        // 普通の戻り値は反変
        T M();

        // 本来できてもいいはずなものの、.NET 的には無理
        void M(out T x);
    }
#endif
}
