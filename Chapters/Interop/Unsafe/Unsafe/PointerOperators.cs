namespace Unsafe.PointerOperators
{
    using System;

    struct Point
    {
        public short X;
        public short Y;
    }

    class Program
    {
        unsafe static void Main()
        {
            var p = new Point();

            // アンマネージ型の変数にはポインターを使える
            // & でアドレス取得(ポインター化)
            // 型推論(var)も効く
            var pp = &p;

            // int 型のポインターに無理やり代入
            // p のある位置に無理やり int の値を書き込み
            int* pi = (int*)pp;
            *pi = 0x00010002;

            // -> で構造体のポインターのメンバーにアクセス
            Console.WriteLine(pp->X); // (*pp).X と同じ意味 = 2
            Console.WriteLine(pp->Y); // (*pp).Y と同じ意味 = 1

            // byte 型のポインターに無理やり代入
            byte* pb = (byte*)pp;

            // ポインターには配列と同じように [] が使える
            Console.WriteLine(pb[0]); // *(pb + 0) と同じ意味 = 2
            Console.WriteLine(pb[1]); // *(pb + 1) と同じ意味 = 0
            Console.WriteLine(pb[2]); // *(pb + 2) と同じ意味 = 1
            Console.WriteLine(pb[3]); // *(pb + 3) と同じ意味 = 0
        }
    }
}
