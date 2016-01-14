namespace IdentifierScope.Nest
{
    using System;

    public class Program
    {
        public static void M1()
        {
            int x = 10;

            {
                //int x = 20;
                Console.WriteLine(x);
            }

            Console.WriteLine(x);
        }

        public static void M2()
        {
            {
                int x = 10;
                Console.WriteLine(x);
            }

            {
                // 別ブロック = 別スコープ。↑のxとは完全に別物
                string x = "a";
                Console.WriteLine(x);
            }
        }

        public static void M3()
        {
            {
                // 下で定義されている string の方の x と名前被り
                //int x = 20; // コンパイル エラー
                //Console.WriteLine(x);
            }

            // string の方の x はここから下でしか使えない
            // にも関わらず、x のスコープはメソッド内全体
            string x = "a";
            Console.WriteLine(x);
        }
    }
}
