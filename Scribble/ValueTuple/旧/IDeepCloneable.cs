namespace ValueTuples.旧
{
    /// <summary>
    /// ディープ クローンできる型。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDeepCloneable<T>
    {
        /// <summary>
        /// ディープ クローンを作る。
        /// </summary>
        /// <returns></returns>
        T Clone();
    }

    public static class DeepCloneableExtensions
    {
        /// <summary>
        /// <see cref="IDeepCloneable{T}"/> 実装クラスの場合は <see cref="IDeepCloneable{T}.Clone"/> の結果を返して、
        /// そうでない場合は素通し。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T x)
        {
            var dc = x as IDeepCloneable<T>;
            return dc == null ? x : dc.Clone();
        }
    }
}
