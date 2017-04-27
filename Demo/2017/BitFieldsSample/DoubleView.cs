namespace BitFieldsSample
{
    /*
    // 初期状態
    // BitFields という名前の inner enum を定義すると、アナライザーによってクイックアクションが表示される
    // クイックアクションで、「Generate bit-fields」を選択するとコード生成が走る。
    // *.BitFields.cs という名前の別ファイル内に生成。
    struct DoubleView
    {
        enum BitFields
        {
            Fraction = 52,
            Exponent = 11,
            Sign = 1,
        }
    }
     */

    /// <summary>
    /// double (IEEE 754 倍精度浮動小数点数)の中身をビット操作するための構造体。
    /// </summary>
    partial struct DoubleView
    {
        enum BitFields
        {
            Fraction = 52,
            Exponent = 11,
            Sign = 1,
        }

        public unsafe ref double AsFloat()
        {
            fixed (ulong* p = &Value)
            {
                return ref *(double*)p;
            }
        }
    }
}