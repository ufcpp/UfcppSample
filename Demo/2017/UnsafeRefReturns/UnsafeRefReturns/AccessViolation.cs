using System;
using System.Runtime.CompilerServices;

namespace UnsafeRefReturns.AccessViolation
{
    // # blittable/unmanaged types
    //
    // ## unmanaged
    //
    // C# でポインターにできるのは、値型しか含まない構造体だけ。
    // GC 管理対象のmanaged参照を持っているとポインター化するの危ない。
    // (別の型に変換し放題で、managed参照を自由に書き換えられる。
    //  そういうことすると、GCクラッシュさせたりバッファーオーバーフロー的なセキュリティリスクがある。)
    // managed参照を一切持たないという意味で、「unmanaged型」って呼ぶ。
    //
    // ## blittable
    //
    // C#でいうunmanaged型は、別の文脈(主にnative interop)ではblittable型って呼ばれる。
    // ポインターを使うと、メモリ領域を丸コピーしたり、byte操作で読み書きしたりできる。
    // 「メモリ領域丸コピーできる」って意味で、ブロック転送(blt)可能型 → blittable。
    // ネイティブコードに対して、データをブロック転送で渡したい(マーシャリングのコストを掛けたくない)時に使うことが多いんで、「blittable型」って呼ばれる。
    //
    // # blittable 制約
    //
    // これと同じ条件を、ジェネリック型に対しても使いたいんだけど、そのためには型制約が必要。
    // where T : unmanged とか
    // where T : blittable とか
    // where T : fixed とか
    // いくつか候補あり。

    class Program
    {
        unsafe static ref U Cast<T, U>(ref T x)
            // こういう型制約が必要かも
            //where T : blittable
            //where U : blittable
            => ref Unsafe.AsRef<U>(Unsafe.AsPointer(ref x));

        static void Main() // Main には unsafe ついてない
        {
            var s = "abcde";
            ref IntPtr p = ref Cast<string, IntPtr>(ref s); // 元凶のコード。こいつの中身自身は unsafe
            p = (IntPtr)123456789; // 適当な値
            Console.WriteLine(s); // safe な文脈なのに AccessViolation 例外起こせる
        }
    }
}
