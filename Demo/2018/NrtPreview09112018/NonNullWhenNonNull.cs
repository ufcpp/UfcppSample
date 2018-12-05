namespace NrtPreview09112018
{
    static class NonNullWhenNonNull
    {
        class Base { }
        class Derived : Base { }

        static void M()
        {
            Base[] b = new Derived[] { new Derived() };
            Derived[] d = b.Convert<Base, Derived>(); // expected non-null bacause b is not null, but CS8600
        }

        public static TDerived[]? Convert<TBase, TDerived>(this 
            // I want [NonNullWhenNonNull] attribute.
            TBase[]? array)
            where TDerived : class, TBase
        {
            if (array is TDerived[] x) return x;
            if (array == null) return null;

            TDerived[] d = new TDerived[array.Length];
            TBase[] b = d;
            for (int i = 0; i < d.Length; i++)
                b[i] = array[i];
            return d;
        }
    }
}
