using System;

namespace FlowAnalysis.BclAnnotation
{
    class Program
    {
        static void Main()
        {
            Warning();
            NoWarning();
        }

        private static void Warning()
        {
            var p = typeof(string).GetProperty("Length");
            Console.WriteLine(p.PropertyType);
        }

        private static void NoWarning()
        {
            var p = typeof(string).GetProperty("Length");

            if (p is null) return;

            Console.WriteLine(p.PropertyType);
        }
    }
}
