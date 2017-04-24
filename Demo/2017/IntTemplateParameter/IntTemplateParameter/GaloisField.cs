namespace IntTemplateParameter
{
    using static ConstantInt;

    public static class GaloisField
    {
        public static GaloisField<_2> Gf2(this int i) => new GaloisField<_2>(i);
        public static GaloisField<_3> Gf3(this int i) => new GaloisField<_3>(i);
        public static GaloisField<_5> Gf5(this int i) => new GaloisField<_5>(i);
        public static GaloisField<_7> Gf7(this int i) => new GaloisField<_7>(i);
    }
}
