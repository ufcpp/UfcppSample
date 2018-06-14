namespace Unsafe.Array
{
    class Program
    {
        unsafe static void Main()
        {
            var array = new[] { 1, 2, 3, 4, 5 };

            // 配列 x をポインター px に代入する。
            fixed (int* px = array)
            {
                // ポインターを介して配列 x の内容を変更。
                for (int* p = px; p != px + array.Length; ++p)
                    *p = (*p) * (*p);
            }

            // 結果出力。
            for (int i = 0; i < array.Length; ++i)
                System.Console.Write("{0} ", array[i]);
            // 1 4 9 16 25 と表示される。
        }
    }
}
