namespace VS16_1_p2.ReadOnlyMember
{
    // 構造体自体は readonly にしない。
    // フィールドは書き換えたい
    struct NonReadOnly
    {
        public float X;
        public float Y;

        // でも、このプロパティ内ではフィールドを書き換えない
        public float LengthSquared => X * X + Y * Y;
    }

    // NonReadOnly との差は LengthSquared の readonly の有無だけ
    struct ReadOnly
    {
        public float X;
        public float Y;

        // readonly 修飾でフィールドを書き換えないことを明示
        public readonly float LengthSquared => X * X + Y * Y;
    }

    class Program
    {
        // こっちは、LengthSquared 内での X, Y の書き換えを恐れて隠れたコピーが発生する。
        static float M(in NonReadOnly x) => x.LengthSquared;

        // こっちは、LengthSquared に readonly が付いているのでコピー発生しない。
        static float M(in ReadOnly x) => x.LengthSquared;

        static void Main(string[] args)
        {
            M(new NonReadOnly { X = 1, Y = 2 });
            M(new ReadOnly { X = 1, Y = 2 });
        }
    }
}
