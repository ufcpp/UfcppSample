namespace RecursiveReadOnly.ClassReadonly
{
    // 書き換え可能なクラス
    class MutableClass
    {
        // フィールドを直接公開
        public int X;

        // 書き換え可能なプロパティ
        public int Y { get; set; }

        // フィールドの値を書き換えるメソッド
        public void M(int value) => X = value;
    }

    class Program
    {
        static readonly MutableClass c = new MutableClass();

        static void Main()
        {
#if InvalidCode
            // これは許されない。c は readonly なので、c 自体の書き換えはできない
            c = new MutableClass();
#endif

            // けども、c の中身までは保証してない
            // 書き換え放題
            c.X = 1;
            c.Y = 2;
            c.M(3);
        }
    }
}
