using System;

namespace SampleApp.Lib
{
    /// <summary>
    /// プロパティに名前でアクセスしたり、インデックスでアクセスしたりを、
    /// リフレクションでなく、事前コード生成でやるためのインターフェイス。
    /// </summary>
    /// <remarks>
    /// 旧式。object を介するのでbox化を避けれずつらい。
    /// </remarks>
    public interface IRecordAccessor
    {
        /// <summary>
        /// インデックス指定でフィールドの値を取り出す。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>取り出した値</returns>
        Type GetType(int index);

        /// <summary>
        /// インデックス指定でフィールドの値を取り出す。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>取り出した値</returns>
        object Get(int index);

        /// <summary>
        /// インデックス指定でフィールドの値を設定する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="value">設定したい値。</param>
        void Set(int index, object value);
    }
}
