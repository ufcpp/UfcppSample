namespace TaskLibrary.Channels
{
    /// <summary>
    /// 値 or その型の配列を格納する型。
    /// union 型を使って T | T[] とか書きたいやつ。
    /// </summary>
    /// <typeparam name="T">値の型。</typeparam>
    /// <remarks>
    /// 名前は要検討…
    /// T[] で持って、値の時は new[] { value } するとかでもいいんだけど、ヒープ アロケーション減らすためにこれを使う。
    /// </remarks>
    public struct Holder<T>
    {
        private readonly object _value;

        /// <summary>
        /// 値を代入。
        /// </summary>
        /// <param name="value"></param>
        public Holder(T value)
        {
            _value = value;
        }

        /// <summary>
        /// 配列を導入。
        /// </summary>
        /// <param name="value"></param>
        public Holder(T[] value)
        {
            _value = value;
        }

        /// <summary>
        /// 中身が配列の時に限り、その配列を返す。
        /// </summary>
        /// <remarks>
        /// 中身が値の時に呼ぶとnullを返す。
        /// <see cref="IsArray"/>が別途ある以上、中身が値の時に呼んだら例外出す方がAPIとしてはきれいだけど、性能優先。
        /// is して as し直しみたいな流れがちょっともったいない(だったら as して null チェックの方が高速)ので、こういう仕様にしている。
        /// </remarks>
        public T[] Array => _value as T[];

        /// <summary>
        /// 中身が値の時に限り、その値を返す。
        /// </summary>
        /// <remarks>
        /// こちらは、中身が配列だったら例外飛ばす。
        /// 値型に対して as が使えないのでこういう仕様。
        /// </remarks>
        public T Value => (T)_value;

        /// <summary>
        /// 中身が配列のときにtrue。
        /// </summary>
        public bool IsArray => _value is T[];

        public static implicit operator Holder<T>(T value) => new Holder<T>(value);
        public static implicit operator Holder<T>(T[] value) => new Holder<T>(value);
    }
}
