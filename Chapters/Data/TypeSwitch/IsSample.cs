using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeSwitch
{
    class IsSample
    {
        static void 型判定のみ(object obj)
        {
            // 型判定のみなら、これまでの is 演算子でも十分
            if (obj is string) Console.WriteLine("string");
        }

        static void 型変換もしたい(object obj)
        {
            // 型変換もしたい
            if (obj is string)
            {
                var s = (string)obj;
                //↑ isとキャストで2つの別命令を使う。二重処理になってるだけで無駄
                Console.WriteLine("string #" + s.Length);
            }

            {
                // 結局、as 演算子 + null チェックを使うことになる
                var s = obj as string;
                if (s != null)
                {
                    Console.WriteLine("string #" + s.Length);
                }
            }
        }

        static void TypeSwitch(object obj)
        {
            // C# 7での新しい書き方
            if (obj is string s)
            {
                Console.WriteLine("string #" + s.Length);
            }
        }

        static void NullCheck()
        {
            string x = null;

            if (x is string)
            {
                // x の変数の型は string なのに、is string は false
                // is 演算子は変数の実行時の中身を見る ＆ null には型がない
                Console.WriteLine("ここは絶対通らない");
            }
        }

        static void F(string nullable)
        {
            if (nullable is string nonNull)
            {
                Console.WriteLine(nonNull.Length);
            }
        }

        static void F(int? x)
        {
#if false
            // C# 6以前の書き方
            if (x.HasValue)
            {
                // この「.Value」が結構うっとおしい
                Console.WriteLine(x.Value * x.Value);
            }
#else
            if (x is int n)
            {
                Console.WriteLine(n * n);
            }
#endif

#if false
            static void F(object obj)
            {
                if (obj is string)
                {
                    // この中では obj を string 扱いできる言語がある
                    // C# ではコンパイル エラー
                    Console.WriteLine("string #" + obj.Length);
                }
                else if (obj is int)
                {
                    // 同上、int 扱いできる言語がある
                    // C# ではコンパイル エラー
                    Console.WriteLine("int " + (obj * obj));
                }
            }

            static void F(object x)
            {
                if (x is string x)
                {
                    // 引数の x とは別に、is 演算子で別の「x」を導入できる言語もある
                    // C# ではコンパイル エラー
                    Console.WriteLine("string #" + x.Length);
                }
            }
#endif
        }
    }
}
