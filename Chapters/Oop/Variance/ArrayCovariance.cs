using System;

namespace Variance
{
    class ArrayCovariance
    {
        public static void F()
        {
            string[] derivedItems = { "Aleph", "Beth", "Gimel" };
            object[] baseItems = derivedItems;

            // 読み出し(戻り値側、out、共変)は常に安全
            for (int i = 0; i < baseItems.Length; i++)
            {
                Console.WriteLine(baseItems[i]);
            }
        }

#if false
        static void G()
        {
            object[] baseItems = { 1, 2, 3 };
            string[] derivedItems = baseItems; // コンパイル エラー
        }
#endif

        public static void H()
        {
            string[] derivedItems = { "Aleph", "Beth", "Gimel" };
            object[] baseItems = derivedItems;

            // 書き込み(引数側、in、反変)は本当はやっちゃいけない
            // でも、コンパイルが成功する。実行時エラーが出る
            baseItems[1] = 100;
        }
    }
}
