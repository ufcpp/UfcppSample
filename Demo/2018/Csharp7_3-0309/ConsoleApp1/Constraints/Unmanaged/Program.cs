using System;

namespace ConsoleApp1.Constraints.Unmanaged
{
    /// <summary>
    /// これまで、ジェネリックな型に対してポインターは作れなかった。
    ///
    /// ポインターは、GC 管理下(managed)のオブジェクト(参照型)を1つも含まない型に対してしか使えない。
    /// この制限がないと、GC の動作を狂わせて、かなり深刻な問題(セキュリティ ホールを含む)を起こすことができてしまう。
    ///
    /// その保証のために、元々、C# は、以下のような型だけをポインター化可能:
    /// - プリミティブ型、enum
    /// - ポインター型
    /// - 再帰的に、上記の型しか含まない構造体
    ///
    /// (ちなみに、この条件を満たす型を、managed なオブジェクトを含まないという意味で、「unmanaged な型」と呼ぶ。)
    ///
    /// この制限はちょっと過剰気味で、ジェネリックな型に対してはポインターを作れなかった。
    ///
    /// これに対して C# 7.3 では、ジェネリック型引数に対して unmanaged 制約と言うのを付けることで、
    ///
    /// - unmanaged 型(上記条件を満たす型)に対してだけ具象化できる
    /// - 型引数であってもポインター化できる
    ///
    /// というようなジェネリック定義ができるようになった。
    /// </summary>
    class Program
    {
        static void Main()
        {
            var point = new Point { X = 1, Y = 2, Z = 2 };
            string s = "abc";

            unsafe
            {
                // ジェネリックでなければ、Point は元からポインター化可能。
                Point* p = &point;

#if Uncompilable
                // これはダメ。
                // 参照型のポインターは作れない。
                string* ps = &s;
#endif
            }

            MemSet0(ref point);
            Console.WriteLine(point); // (0, 0, 0)

#if Uncompilable
            // これはダメ。
            // MemSet0<T> の T に unmanaged  制約が付いてるので、string では呼べない
            MemSet0(ref s);
#endif

#if Uncompilable
            // これもダメ。
            // unmanaged な型だけを含む構造体なので、原理的にはポインター化しても安全なんだけど。
            // 再帰的なジェネリック型に対して安全保証するのはメリットの割に大変らしく、断念したとのこと。
            var kv = new KeyValuePair<int, Point>(1, point);
            MemSet0(ref kv);
#endif
        }

        unsafe static void MemSet0<T>(ref T x)
            where T : unmanaged // この型制約が C# 7.3 の新機能。
        {
            // 今まではこの T* が許されなかった。
            // たとえ、Point みたいにポインター化可能な型で MemSet0<Point> を呼んだとしてもダメ。
            // unmanaged  制約のおかげで、ポインター化可能になった。
            fixed (T* p = &x)
            {
                var b = (byte*)p;
                var size = sizeof(T);
                for (int i = 0; i < size; i++)
                {
                    b[i] = 0;
                }
            }
        }

        static void SafeStackalloc<T>()
            where T : unmanaged
        {
            // C# 7.2 の safe stackalloc 機能も、unmanaged 型に対してしか使えない。
            // ポインターとまったく同じで、これまではジェネリック不可、C# 7.3 で unmanaged 制約を付ければ可。
            Span<T> span = stackalloc T[4];
        }

        // ちなみに、unmanaged 制約は暗黙的に struct 制約にもなってる。
        // unmanaged な時点で構造体であることが確定。
        // なので、where T : unmanaged, struct みたいな重複な指定は不可。
        // where T : unmanaged, class みたいな矛盾した指定も不可。
    }
}
