namespace GenericsSample
{
    public class Wrapper<T>
    {
        public T Value = default!; // object initializer で初期化するつもり
    }

    class Program
    {
        static void Main(string[] args)
        {
            var i = new Wrapper<int> { Value = 1 };
            var b = new Wrapper<byte> { Value = 1 };
            var s = new Wrapper<string> { Value = "abc" };
            var a = new Wrapper<int[]> { Value = new[] { 1, 2, 3 } };

            int iv = i.Value;
            byte bv = b.Value;
            string sv = s.Value;
            int[] av = a.Value;
        }
    }
}
