using System;
using System.Collections.Generic;
using System.Text;

namespace Span.SpanSafetyRules
{
    class SpanSafety
    {
        // 引数で受け取ったものは戻り値で返せる
        private static Span<int> Success(Span<int> x) => x;

#if InvalidCode
        // ローカルで確保したもの変数はダメ
        private static Span<int> Error()
        {
            Span<int> x = stackalloc int[1];
            return x;
        }
#endif
        // ちゃんと「メモリ確保」があったかどうかを見てる
        // 同じようなコードでもこれは OK (default だと何も確保しない)
        private static Span<int> Success1()
        {
            Span<int> x = default;
            return x;
        }

        // 多段の場合も元をたどって出所を調べてくれる
        private static Span<int> Success(Span<int> x, Span<int> y)
        {
            var r1 = x;
            var r2 = y;
            var r3 = r1.Length >= r2.Length ? r1 : r2;

            // r3 は出所をたどると引数の x か y
            // x も y も引数なので大丈夫
            return r3;
        }

#if InvalidCode
        private static Span<int> Error(Span<int> x, int n)
        {
            var r1 = x;
            Span<int> r2 = stackalloc int[n];
            var r3 = r1.Length >= r2.Length ? r1 : r2;

            // r2 がローカルなのでダメ
            return r3;
        }
#endif
    }

    class SpanAndRefSafty
    {
        // 引数で受け取った Span 由来の ref 戻り値は返せる
        private static ref int Success(Span<int> x) => ref x[0];

#if InvalidCode
        // ローカルで確保した Span 由来の ref 戻り値はダメ
        private static ref int Error()
        {
            Span<int> x = stackalloc int[1];
            return ref x[0];
        }
#endif
    }

    namespace ReadOnlyRef
    {
        using System;

        // ref だけ
        ref struct RefToSpan
        {
            private readonly Span<int> _span;
            public RefToSpan(Span<int> span) => _span = span;

            // 例え _span に readonly が付いていても、this 書き換えが可能
            public void Method(Span<int> span) { this = new RefToSpan(span); }
        }

        // readonly ref
        readonly ref struct RORefToSpan
        {
            private readonly Span<int> _span;
            public void Method(Span<int> span) { }
        }

        class Program
        {
#if InvalidCode
            public static void LocalToRef(RefToSpan r)
            {
                Span<int> local = stackalloc int[1];
                r.Method(local); // ここでエラーになる。r の中身が書き換えられることで、local が外に漏れる可能性を危惧

                // 注: この例の場合は実際には漏れはしないものの、RefToSpan の作り次第なので保証はできない
            }
#endif

            public static void LocalToRORef(RORefToSpan r)
            {
                Span<int> local = stackalloc int[1];
                r.Method(local); // readonly ref に対してなら OK
            }
        }
    }

    class Unsafe
    {
        unsafe static Span<int> X()
        {
            // ローカル
            int x = 10;

            // unsafe な手段でローカルなものの参照を作って返す
            // これをやってしまうとまずいものの、コンパイル時にはエラーにできない
            return new Span<int>(&x, 1);
        }
    }

#if InvalidCode
    namespace StackOnly
    {
        using System;
        using System.Collections.Generic;
        using System.Threading.Tasks;

        //❌ そもそもクラスに ref を付けれないのも stack-only を保証するため
        ref class Class { }

        //❌ インターフェイス実装
        ref struct RefStruct : IDisposable { public void Dispose() { } }

        class Program
        {
            //❌ 非同期メソッドの引数
            static async Task Async(Span<int> x)
            {
                //❌ 非同期メソッドのローカル変数
                Span<int> local = stackalloc int[10];
            }

            //❌ イテレーターの引数
            static IEnumerable<int> Iterator(Span<int> x)
            {
                Span<int> local = stackalloc int[10];
                local[0] = 1; //⭕ yield return をまたがないならOK
                yield return 0;
                //❌ yield をまたいだ読み書き
                local[0] = 2; // ダメ
            }

            static void Main()
            {
                Span<int> local = stackalloc int[1];

                //❌ box 化
                object obj = local;

                //❌ object のメソッド呼び出し
                var str = local.ToString();

                //❌ クロージャ
                Func<int> a1 = () => local[0];
                int F() => local[0];

                //❌ 型引数にも渡せない
                List<Span<int>> list;
            }
        }
    }
#endif

#if Sample
    // Span<T> は ref 構造体になっている
    public readonly ref struct Span<T> { ... }

    // ref 構造体を持てるのは ref 構造体だけ
    ref struct RefStruct
    {
        private Span<int> _span; //OK
    }

    // NG。構造体以外を「ref 型」にはできない
    ref class InvalidClass { }

    // ref がついていない普通の構造体は ref 構造体を持てない
    struct NonRefStruct
    {
        private Span<int> _span; //NG
    }
#endif
}
