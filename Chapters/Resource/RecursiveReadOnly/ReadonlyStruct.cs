namespace RecursiveReadOnly.ReadonlyStruct
{
    using System;

    // 構造体自体に readonly を付ける
    readonly struct Point
    {
        // フィールドには readonly が必須
        public readonly int X;
        public readonly int Y;

        public Point(int x, int y) => (X, Y) = (x, y);

        // readonly を付けない場合と違って、以下のような this 書き換えも不可
        //public void Set(int x, int y) => this = new Point(x, y);
    }

    class Program
    {
        static void Main()
        {
            var p = new Point(1, 2);

            // p.X = 0; とは書けない。これはちゃんとコンパイル エラーになる
            // p.Set(3, 4); みたいなのもダメ

            Console.WriteLine(p.X); // 1 しかありえない
            Console.WriteLine(p.Y); // 2 しかありえない
        }
    }
}
