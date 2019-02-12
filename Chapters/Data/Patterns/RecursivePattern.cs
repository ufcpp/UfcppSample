namespace Patterns.RecursivePattern
{
    class Program
    {
        static int M(object obj)
            => obj switch
        {
            0 => 1,
            int i => 2,
            Point (1, _) => 4, // new!
            Point { X: 2, Y: var y } => y, // new!
            _ => 0
        };
    }
}
