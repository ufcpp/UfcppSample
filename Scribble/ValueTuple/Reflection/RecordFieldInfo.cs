namespace ValueTuples.Reflection
{
    /// <summary>
    /// <see cref="RecordTypeInfo"/> のフィールド情報。
    /// </summary>
    public struct RecordFieldInfo
    {
        /// <summary>
        /// フィールドの型。
        /// </summary>
        public RecordTypeInfo Type { get; }

        /// <summary>
        /// フィールド名。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// フィールドが定義されているインデックス。
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"><see cref="Type"/></param>
        /// <param name="name"><see cref="Name"/></param>
        /// <param name="index"><see cref="Index"/></param>
        public RecordFieldInfo(RecordTypeInfo type, string name, int index)
        {
            Type = type;
            Name = name;
            Index = index;
        }
    }
}
