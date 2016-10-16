using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueTypeGenerics
{
    class Program
    {
        static void Main(string[] args)
        {
        }

#if false
        // コンパイル エラー: T に Count プロパティがない
        static int M<T>(T x) => x.Count;
#endif

        // これなら大丈夫。IList.Count を参照できる
        static int M<T>(T x)
            where T : System.Collections.IList
            => x.Count;

#if false
        // interface 制約では静的メソッドを呼べない
        // なので、ジェネリックを使うと静的メソッドを呼ぶ手段がない
        // コンパイル エラーに
        static T M<T>(T x) => T.StaticMethod(x);

        // + (演算子)は実質的には静的メソッド
        // 演算子もコンパイル エラーに
        static T Add<T>(T x, T y) => x + y;
#endif
    }
}
