namespace Build2014
{
    /// <summary>
    /// //build/ 2014 でのデモをベースにした C# vNext のサンプル。
    /// 主に、getter-only auto-property、primary constructor、expression bodied function、using static、declaration expressions などのデモ。
    /// "C# 6.0" となる前のプレビュー状態で、バージョンアップのたびに少しずつ仕様追加・変更があって、そのたび追従して修正。
    ///
    /// //build/ のデモではセッションの最中に C# 5.0 の状態のコードを書き替えて新機能紹介していたけども、
    /// このプロジェクトでは書き替えの前後の両方のコードを書いてある。
    /// Csharp5 フォルダー/名前空間以下が C# 5.0 のコード。
    /// Csharp6 フォルダー/名前空間以下が C# 6.0(予定) のコード。
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Csharp5.Program.Run();
            Csharp6.Program.Run();
        }
    }
}
