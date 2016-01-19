namespace NullabilitySample
{
    public class WhatsNullable
    {
        private static void Run()
        {
            int a = 10; // OK
#if ERRONEOUS_CODE
            int b = null; // コンパイルエラー
#endif

            // 型名の後ろに ? を付けるとnull許容型になる
            int? c = 10; // OK
            int? d = null; // これもOK
            c = a; // int → int? の変換は常に成功
#if ERRONEOUS_CODE
            a = c; // 逆はコンパイルエラー
#endif
            a = c ?? 0; // nullをなくす処理が必要
        }
    }
}
