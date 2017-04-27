namespace BitFieldsSample
{
    /*
    // 初期状態
    // BitFields という名前の inner enum を定義すると、アナライザーによってクイックアクションが表示される
    // クイックアクションで、「Generate bit-fields」を選択するとコード生成が走る。
    // *.BitFields.cs という名前の別ファイル内に生成。
    struct SingleView
    {
        enum BitFields
        {
            Fraction = 23,
            Exponent = 8,
            Sign = 1,
        }
    }
     */

    /// <summary>
    /// float (IEEE 754 単精度浮動小数点数)の中身をビット操作するための構造体。
    /// </summary>
    partial struct SingleView
    {
        enum BitFields
        {
            Fraction = 23,
            Exponent = 8,
            Sign = 1,
        }

        public unsafe ref float AsFloat()
        {
            fixed (uint* p = &Value)
            {
                return ref *(float*)p;
            }
        }
    }
}