class Closure
{
    static void X(object ? x)
    {
        if (x == null)
        {
            x = new object();
        }

        void a()
        {
            object nonNull = x; // CS8600
        };

        a();
    }
}
