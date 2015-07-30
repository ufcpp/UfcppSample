namespace ValueTuples.Reflection
{
    /// <summary>
    /// プロパティに名前でアクセスしたり、インデックスでアクセスしたりを、
    /// リフレクションでなく、事前コード生成でやるためのインターフェイス。
    /// </summary>
    public interface IRecordAccessor
    {
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

        /// <summary>
        /// フィールド名指定でフィールドの値を取り出す。
        /// </summary>
        /// <param name="key">フィールド名</param>
        /// <returns>取り出した値</returns>
        object Get(string key);

        /// <summary>
        /// フィールド名指定でフィールドの値を設定する。
        /// </summary>
        /// <param name="key">フィールド名</param>
        /// <param name="value">設定したい値。</param>
        void Set(string key, object value);
    }
}
