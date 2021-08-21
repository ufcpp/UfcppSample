// コンパイラー生成物はプロパティ。
Console.WriteLine(typeof(Point0).GetProperty("X") != null); // true
Console.WriteLine(typeof(Point0).GetField("X") != null); // false

// それをフィールドの手書きで抑止してる版。
Console.WriteLine(typeof(Point1).GetProperty("X") != null); // false
Console.WriteLine(typeof(Point1).GetField("X") != null); // true

// record struct のプライマリ コンストラクター引数から生成するもの、
// record class に合わせてプロパティにした。

// こう書くと、
// public int X { get; set; }
// public int Y { get; set; }
// が生成される。
record struct Point0(int X, int Y);

// 一方で、「フィールドにしたい」という要件はあるだろうから、
// 「フィールドでも手書きしておけばコンパイラー生成を抑止して優先的に使う」という仕様が入った。
// (C# 9.0 時点ではこれができるのはプロパティだけ。)
record struct Point1(int X, int Y)
{
    // 手書きで X, Y フィールドがあるので、
    // コンストラクター引数からのプロパティ X, Y 生成はされない。
    // この手書き X, Y が使われる。
    public int X = X;
    public int Y = Y;
}

// record struct のために入った仕様とはいえ、
// record calss に対して禁止する理由もないので、
// C# 10.0 からは同様の「フィールド手書き」が record class でもできるようになった。
record class Point2(int X, int Y)
{
    // C# 9.0 ではダメだった。
    public int X = X;
    public int Y = Y;

    // ↓なら C# 9.0 の頃でも OK だった。
    //public int X { get; set; } = X;
    //public int Y { get; set; } = Y;
}
