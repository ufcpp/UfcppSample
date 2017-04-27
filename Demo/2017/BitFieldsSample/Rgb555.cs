using BitFields;

namespace BitFieldsSample
{
    /*
    // 初期状態
    // BitFields という名前の inner enum を定義すると、アナライザーによってクイックアクションが表示される
    // クイックアクションで、「Generate bit-fields」を選択するとコード生成が走る。
    // *.BitFields.cs という名前の別ファイル内に生成。
    struct Rgb555
    {
        enum BitFields
        {
            B = 5,
            G = 5,
            R = 5,
        }
    }
     */

    /// <summary>
    /// RBG それぞれ5ビットずつで色を表現する構造体。
    /// </summary>
    partial struct Rgb555
    {
        enum BitFields
        {
            B = 5,
            G = 5,
            R = 5,
        }

        public Rgb555(Bit5 r, Bit5 g, Bit5 b) : this() => (R, G, B) = (r, g, b);
        public void Deconstruct(out Bit5 r, out Bit5 g, out Bit5 b) => (r, g, b) = (R, G, B);

        public override string ToString() => $"((R): {R}, G: {G}, B: {B})";
    }
}