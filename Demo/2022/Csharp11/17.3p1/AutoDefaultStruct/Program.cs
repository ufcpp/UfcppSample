var a = new AutoDefault(1);
Console.WriteLine($"{a.X}, {a.Y}"); // 1, 0

// これまで、構造体のコンストラクターでは、
// * コンストラクターを抜けるまでにすべてのフィールドを初期化しないとダメ
// * 関数メンバー(プロパティとかメソッド)に触れる前にすべてのフィールドを初期化しないとダメ
// みたいな制限があった。
//
// 特にこの制限を維持する理由もないみたいなので変更。
struct AutoDefault
{
    public int X;
    public int Y; // CS0649 警告

    public AutoDefault(int x)
    {
        X = x;

        // Y に何も代入しないの、OK になった。
        // (昔はエラーだった。今も警告は出る。)
        //
        // この場合、何も代入してないフィールドには自動的に 0 が入る。
        // (0 というか、いわゆる既定値。0, false, null, default。)
    }
}

#if false
// というのも、C# 11 リリースまでには以下のような「半自動プロパティ構文」も入る予定で、
// 「自動 0 初期化」の需要が上がった。
struct SemiAutoProperty
{
    // field がキーワードに。
    // 自動的にバッキング フィールドへの読み書きになる。
    public int X { get => field; set => field = value; }

    public SemiAutoProperty(int x)
    {
        // 「フィールドを予期化しないままプロパティ(関数メンバー)を呼んじゃダメ」制限に引っかかりそう。
        // auto-default の仕様が入ったなら、「X のバッキング フィールドは 0 初期化された状態で X の set を呼ぶ」になるので平気。
        X = x;
    }
}
#endif
