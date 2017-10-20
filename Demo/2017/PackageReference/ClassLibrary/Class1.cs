namespace ClassLibrary
{
    public class Class1
    {
        public static string Id =
#if A
            "ClassLibrary.A"
#elif B
            "ClassLibrary.B"
#else
            "ClassLibrary"
#endif
        ;
    }
}
