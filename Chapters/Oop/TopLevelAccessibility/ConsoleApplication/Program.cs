namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var c1 = new A.PublicClass();
            var s1 = new A.PublicStruct();

            //var c2 = new A.InternalClass();  // コンパイル エラー
            //var s2 = new A.InternalStruct(); // コンパイル エラー
        }
    }
}
