namespace GenericsSample.Template
{
    // 使った分だけそれぞれ別の型に展開
    public class Wrapper_int { public int Value; }
    public class Wrapper_byte { public byte Value; }
    public class Wrapper_string { public string Value = null!; } // object initializer で初期化するつもり
    public class Wrapper_Array_int { public int[] Value = null!; } // 同上

    class Program
    {
        static void Main(string[] args)
        {
            var i = new Wrapper_int { Value = 1 };
            var b = new Wrapper_byte { Value = 1 };
            var s = new Wrapper_string { Value = "abc" };
            var a = new Wrapper_Array_int { Value = new[] { 1, 2, 3 } };

            // キャストは不要
            int iv = i.Value;
            byte bv = b.Value;
            string sv = s.Value;
            int[] av = a.Value;
        }
    }
}
