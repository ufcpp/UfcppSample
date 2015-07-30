using System.Collections.Generic;

namespace ValueTuples
{
    /// <summary>
    /// 1. シリアライズしたいフィールドを <see cref="ValueTuple"/> で持っている。
    /// 2. JSON みたいな形式のために、各フィールドの名前を返せる。
    /// 3. フィールドの名前からフィールドのインデックス(<see cref="ITuple"/>に渡す)を返せる。
    /// 4. 派生クラスの場合は、元の型を判別するための値(<see cref="Discriminator"/>)を返せる。
    /// </summary>
    public interface IRecord
    {
        ITuple Value { get; set; }

        string GetKey(int index);

        int GetIndex(string key);

        int? Discriminator { get; }
    }
}
