using System;
using System.Threading;

namespace DisposePattern
{
    /// <summary>
    /// 参照カウントでリソース管理するためのインターフェイス。
    /// </summary>
    public interface IReferenceCoutable : IDisposable
    {
        /// <summary>
        /// 今現在このインスタンスが参照されている数。
        /// </summary>
        ref int Count { get; }
    }

    /// <summary>
    /// <see cref="IReferenceCoutable"/>がらみの基本操作。
    /// </summary>
    /// <remarks>
    /// 以下のようなルールでの運用が必須。
    ///
    /// - InitやShareを空呼びしない(変数で受ける必要がある)
    /// - 最初に必ず1回Initを呼ぶ必要がある
    /// - T型の変数に対して、Init、Share、Moveを介さない生代入をしてはいけない
    /// - T型の変数の書き換え(再代入)をしてはいけない
    /// - T型の変数に対して、Moveがない限り必ずReleaseを呼ばなければならない
    ///   (確実に呼ばれるよう、finally句で呼ぶ必要あり)
    /// - <see cref="IReferenceCoutable"/>フィールドを持ちたい場合、その型も<see cref="IReferenceCoutable"/>でなければならないし、
    ///   <see cref="IDisposable.Dispose"/>でフィールドをReleaseしなければならない
    /// - 参照渡しでもらったT型の変数に対してMoveやReleaseはできない
    ///
    /// ルールが面倒なので、Analyzer を書いて静的コード解析でルールを強制できないと使い物にならない。
    /// 逆に言うと、静的コード解析ができる分、Analyzerさえあれば自前でDispose管理するよりは気が楽。
    /// (誰がDisposeの義務を負っているかが明確で、漏れていたらコンパイルエラーにできる。)
    /// 何なら、コンパイラーによる何らかのサポートが入るべき。
    /// </remarks>
    public static class ReferenceCoutable
    {
        /// <summary>
        /// 参照カウントの初期化。
        /// 最初に必ず1回このメソッド呼ばないといけない。
        /// </summary>
        public static T Init<T>(this T obj)
            where T : IReferenceCoutable
        {
            obj.Count = 1;
            return obj;
        }

        /// <summary>
        /// リソースを共有する。
        /// </summary>
        /// <remarks>
        /// <see cref="IReferenceCoutable.Count"/>を増やす。
        /// 呼び出し元、戻り値で返した側の両方で <see cref="Release{T}(T)"/>義務が発生。
        /// </remarks>
        public static T Share<T>(this T obj)
            where T : IReferenceCoutable
        {
            Interlocked.Increment(ref obj.Count);
            return obj;
        }

        /// <summary>
        /// リソースを譲渡(所有権の移転)する。
        /// </summary>
        /// <remarks>
        /// このメソッドを呼んで以降は、呼び出し元側の変数をもう使ってはいけない。
        /// その代わり、呼び出し元からは<see cref="Release{T}(T)"/>義務が消える。
        /// </remarks>
        public static T Move<T>(this T obj)
            where T : IReferenceCoutable
        {
            return obj;
        }

        /// <summary>
        /// リソースを開放する。
        /// </summary>
        /// <remarks>
        /// <see cref="IReferenceCoutable.Count"/>を減らして。
        /// </remarks>
        public static void Release<T>(this T obj)
            where T : IReferenceCoutable
        {
            if (Interlocked.Decrement(ref obj.Count) == 0)
            {
                obj.Dispose();
            }
        }
    }
}
