namespace IntTemplateParameter
{
    /// <summary>
    /// 定数もどきジェネリクスのために使うインターフェイス。
    /// </summary>
    /// <remarks>
    /// 1. このインターフェイスを実装して、定数を返す構造体を作る(例: <see cref="ConstantInt._0"/>)
    /// 2. その構造体から、default(T).Value で値を取り出す
    /// 3. 値型ジェネリックの性質上、インライン展開されて、そこそこ良い最適化が掛かる期待がある。
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public interface IConstant<T>
    {
        T Value { get; }
    }
}
