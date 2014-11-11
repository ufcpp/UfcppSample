using System;
using System.Collections.Generic;

namespace VersionSample.Csharp4
{
    /// <summary>
    /// ジェネリックの共変注釈(variance annotation、要するに、型引数の in/out 指定)は、小手先の構文糖衣じゃないし、ライブラリのレベルでどうこうできる機能じゃなくて、ランタイムの型システムが対応してる必要があるもの。
    /// で、.NET ランタイムのレベルでは実は .NET 2.0 から実装されてた(要するに、ジェネリック導入と同時にこの機能もあった)。
    /// C# や VB などのプログラミング言語レベルでの実装がなかったのを、C# 4.0/VB 7 で実装しただけ。
    /// なので、当然、(自前のクラスに in/out を付ける分には).NET 3.0 以前でも使える。
    /// </summary>
    public class VarianceSample
    {
        public static void X()
        {
            var inner = new Dictionary<object, string>
            {
                { "one", "un" },
                { "two", "deux" },
                { "three", "trois" },
                { "four", "quatre" },
            };

            DictionaryWrapper<object, string> dic = new DictionaryWrapper<object, string>(inner);

            // dic と idic で型が違うように見えて、
            // in (反変)型引数の場合、object → string (親クラスから子クラス)に代入可能
            // out (共変)型引数の場合、string → object (子クラスから親クラス)に代入可能
            // なので、代入できる。
            IReadOnlyDictionary<string, object> idic = dic;
        }

        // インターフェイスの型引数に、in/out 注釈を付ける。
        // in を付ける場合、関数メンバーの引数/set にしか使えない。
        // out を付ける場合、関数メンバーの戻り値/get にしか使えない。
        interface IReadOnlyDictionary<in TKey, out TValue>
        {
            TValue this[TKey key] { get; }
        }

        class DictionaryWrapper<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
        {
            Dictionary<TKey, TValue> _inner;

            public DictionaryWrapper(Dictionary<TKey, TValue> inner) { _inner = inner; }

            public TValue this[TKey key]
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}

#if Ver4

namespace VersionSample.Csharp4
{
    /// <summary>
    /// 一方で、標準ライブラリ中のインターフェイスに対して、付けれるものにはきっちり全部編成注釈が付いたのは .NET 4 になってから。
    /// なので、例えば、<see cref="IEnumerable{object}"/> の変数に対して <see cref="List{string}"/> のインスタンスを代入できるようになるのは .NET 4 移行。
    /// </summary>
    public class VarianceBclSample
    {
        public static void X()
        {
            List<string> x = new List<string> { "one", "two", "three" };
            IEnumerable<object> y = x;
        }
    }
}

#endif
