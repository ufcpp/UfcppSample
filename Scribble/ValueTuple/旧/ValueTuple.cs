namespace ValueTuples.旧
{
    /// <summary>
    /// <see cref="ValueTuple{T1, T2}"/> とかのファクトリ。
    /// </summary>
    public static partial class ValueTuple
    {
        /// <summary>
        /// 再帰カウント用。
        /// </summary>
        /// <typeparam name="T">要素の型。</typeparam>
        /// <param name="store">要素。</param>
        /// <returns></returns>
        internal static int Count<T>(ref T store)
        {
            var x = store as ITuple;
            return x == null ? 1 : x.Count;
        }

        /// <summary>
        /// 再帰 get。
        /// </summary>
        /// <typeparam name="T">要素の型。</typeparam>
        /// <param name="store">要素。</param>
        /// <param name="index">get したい位置のインデックス。</param>
        /// <param name="output">取りだした値。インデックスが範囲内の時だけ値が入る。</param>
        /// <returns>インデックスが範囲内のときにtrue。</returns>
        internal static bool Get<T>(ref T store, ref int index, out object output)
        {
            var x = store as ITuple;
            if (x == null)
            {
                if (index == 0)
                {
                    output = store;
                    return true;
                }
                else
                {
                    index--;
                    output = null;
                    return false;
                }
            }
            else
            {
                if (index < x.Count)
                {
                    output = x[index];
                    return true;
                }
                else
                {
                    index -= x.Count;
                    output = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// 再帰 set。
        /// </summary>
        /// <typeparam name="T">要素の型。</typeparam>
        /// <param name="store">要素。</param>
        /// <param name="index">get したい位置のインデックス。</param>
        /// <param name="input">設定したい値。インデックスが範囲内の時だけ<paramref name="store"/>が上書かれる。</param>
        /// <returns>インデックスが範囲内のときにtrue。</returns>
        internal static bool Set<T>(ref T store, ref int index, object input)
        {
            var x = store as ITuple;
            if (x == null)
            {
                if (index == 0)
                {
                    store = (T)input;
                    return true;
                }
                else
                {
                    index--;
                    return false;
                }
            }
            else
            {
                if (index < x.Count)
                {
                    x[index] = input;
                    store = (T)x; // boxing 起きてるので書き戻し必要
                    return true;
                }
                else
                {
                    index -= x.Count;
                    return false;
                }
            }
        }
    }
}
