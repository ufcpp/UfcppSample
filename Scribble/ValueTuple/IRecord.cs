namespace ValueTuples
{
    /// <summary>
    /// レコード。
    /// 1. シリアライズしたいフィールドを <see cref="ValueTuple"/> で持っている。
    /// </summary>
    public interface IRecord
    {
        ITuple Value { get; set; }
    }

    //todo: Type GetFieldType(int index) みたいなのも必要そう。ITuple に持たせるべきか、こっちに持たせるべきか。

    // というか、今の作りだと、ITuple.this[index] の計算量が O(n), n はメンバー数で、結構非効率。どうしたものか。
    // ITuple.Values が struct TypeValuePair { Type Type; object Value; } みたいなの返すべきか。

    /// <summary>
    /// レコードの型情報。
    /// リフレクションが使えない環境向けに、ビルド前のコード生成で作る。
    /// <see cref="ITuple"/>自体が各フィールドの型だけは持ってる(名前は Item1 とかになって紛失する)ので、その差分だけ。
    ///
    /// 1. JSON みたいな形式のために、各フィールドの名前を返せる。(必ずしもフィールド名そのまま返さない。場合によっては、snake_case化した名前とかを返すとかする。)
    /// 2. フィールドの名前からフィールドのインデックス(<see cref="ITuple"/>に渡す)を返せる。
    /// 3. デシリアライズ用に、フィールドのインスタンスを new できる。
    /// 4. 派生クラスの場合は、元の型を判別するための値(<see cref="Discriminator"/>)を返せる。
    /// </summary>
    public interface IRecordInfo
    {
        string GetKey(int index);

        int GetIndex(string key);

        object NewInstance(int index, int? discriminator);

        int? Discriminator { get; }
    }

    public interface ITypedRecord : IRecord
    {
        IRecordInfo GetInfo();
    }
}
