// これまではフィールド初期化子も書けなかった。
// (フィールド初期化子には暗黙的に引数なしコンストラクターが必要だった。)
struct FieldInitializer
{
    public int X = 1;
    public int Y = 2;
}

#if false

// 初期化子を持つなら全部のメンバーに初期化子を持たせないと警告が出る。
struct BadFieldInitializer
{
    public int X = 1;
    public int Y; // 警告だけなんだ…
}

// 参考・比較: コンストラクター内で未初期化フィールドがあるとコンパイルエラー。
struct NotFullyAssigned
{
    public int X;
    public int Y;

    public NotFullyAssigned()
    {
        X = 1;
    }
}

#endif
