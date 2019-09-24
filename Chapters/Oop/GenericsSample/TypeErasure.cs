namespace GenericsSample.TypeErasure
{
    // object 型な1つのクラスに集約
    // 元の型情報を残さない = 型消去
    public class Wrapper { public object Value = null!; } // object initializer で初期化するつもり

    class Program
    {
        static void Main(string[] args)
        {
            var i = new Wrapper { Value = new Integer(1) };
            var b = new Wrapper { Value = new Byte(1) };
            var s = new Wrapper { Value = "abc" };
            var a = new Wrapper { Value = new[] { 1, 2, 3 } };

            // キャストが必要
            int iv = ((Integer)i.Value).Value;
            byte bv = ((Byte)i.Value).Value;
            string sv = (string)s.Value;
            int[] av = (int[])a.Value;
        }
    }

    //↓こんな感じのクラスが標準ライブラリ中にある

    public class Integer
    {
        public int Value;
        public Integer(int value) { Value = value; }
    }

    public class Byte
    {
        public byte Value;
        public Byte(byte value) { Value = value; }
    }
}
