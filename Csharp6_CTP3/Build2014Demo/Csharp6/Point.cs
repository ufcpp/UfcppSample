namespace Build2014.Csharp6
{
    /// <summary>
    /// immutable なクラス/構造体を作るのに大変ありがたい構文が追加された。
    /// primary constructor、getter-only auto-property
    /// </summary>
    class Point(int x, int y) // 新しいコンストラクター構文(primary constructor)
    {
        public int X { get; } = x; // get だけの自動実装プロパティと、その初期値。初期値には primary constructor の引数が使える
        public int Y { get; } = y;

        public override string ToString() => string.Format("({0}, {1})", X, Y);
    }
}
