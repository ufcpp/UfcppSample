namespace GenericsSample.ValueType
{
    // 値型の場合: 使った分だけそれぞれ別の型に展開
    public class Wrapper_int { public int Value; }
    public class Wrapper_byte { public byte Value; }

    // 参照型の場合、object 型な1つのクラスに集約
    public class Wrapper { public object Value = null!; } // object initializer で初期化するつもり

    class Program
    {
        static void Main(string[] args)
        {
            var i = new Wrapper_int { Value = 1 };
            var b = new Wrapper_byte { Value = 1 };
            var s = new Wrapper { Value = "abc" };
            var a = new Wrapper { Value = new[] { 1, 2, 3 } };

            // 値型はキャスト不要
            int iv = i.Value;
            byte bv = b.Value;

            // 参照型
            // (C#(.NET) の場合はこのキャストを取り除くような最適化もしてる)
            string sv = (string)s.Value;
            int[] av = (int[])a.Value;
        }
    }
}
