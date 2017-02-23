using BenchmarkDotNet.Running;

namespace Generics
{
    /// <summary>
    /// .NET のジェネリック メソッドがどんな感じで解釈されているかの例。
    ///
    /// 簡単に言うと、
    /// - 参照型 → 全クラス共通の1つのメソッドに展開
    /// - 値型   → 1つ1つ別々に展開
    /// になってる。
    ///
    /// この例の内容:
    /// ・型
    ///   - C から始まる型(<see cref="CInt"/>, <see cref="CString"/>) … クラスで<see cref="ISample"/>を実装したもの
    ///   - S から始まる型(<see cref="SInt"/>, <see cref="SString"/>) … 構造体で<see cref="ISample"/>を実装したもの
    /// ・ベンチマーク
    ///   - Generic から始まるベンチマーク … ジェネリック メソッドを呼び出す
    ///   - Instantiated から始まるベンチマーク … そのジェネリック メソッドがどんな感じに展開されているか、手動展開したもの
    ///   - Object から始まるベンチマーク … (参考)ジェネリックを使わず、object 経由にした場合
    ///
    /// 実行すると以下のような感じに。
    ///               Method |       Mean |    StdErr |    StdDev |  Gen 0 | Allocated |
    /// -------------------- |----------- |---------- |---------- |------- |---------- |
    ///          GenericCInt | 21.8227 ns | 0.1326 ns | 0.5134 ns | 0.0069 |      36 B |
    ///     InstantiatedCInt | 33.9643 ns | 0.3534 ns | 1.9678 ns | 0.0069 |      36 B |
    ///       GenericCString | 25.9516 ns | 0.1144 ns | 0.4281 ns | 0.0069 |      36 B |
    ///  InstantiatedCString | 37.5406 ns | 0.1983 ns | 0.6869 ns | 0.0071 |      36 B |
    ///          GenericSInt |  3.9738 ns | 0.0200 ns | 0.0775 ns |      - |       0 B |
    ///     InstantiatedSInt |  3.8817 ns | 0.0205 ns | 0.0795 ns |      - |       0 B |
    ///           ObjectSInt | 36.9110 ns | 0.1633 ns | 0.6326 ns | 0.0067 |      36 B |
    ///       GenericSString | 15.4912 ns | 0.0675 ns | 0.2614 ns |      - |       0 B |
    ///  InstantiatedSString | 16.5152 ns | 0.1842 ns | 0.7132 ns |      - |       0 B |
    ///        ObjectSString | 45.3602 ns | 0.2891 ns | 1.0423 ns | 0.0067 |      36 B |
    ///
    /// Generic… と Instantiated… はそんなに差が出ないはず。
    /// キャスト不要になる分ちょっとだけ Generic… が有利。
    ///
    /// 値型に対して Object… を呼んでしまうとだいぶ遅くなる。
    /// あと、「Gen 0」(GC 発生回数)と「Allocated」(ヒープ確保した量)の列を見ての通り、object を介したときだけヒープ確保が発生。
    /// これは、値型を object に代入した時点で box 化が起きてるっていうこと。
    /// ジェネリック メソッドが、値型だけ1つ1つ別々に展開してるのは、box 化を起こさないようにするため。
    ///
    /// <see cref="SInt"/>が断トツで速いのは、インライン展開の結果、ほぼ、ただのintに対する操作に展開されてるから。
    /// 値型に限り、1つ1つ別々に展開された結果、「仮想呼び出しが必要ない」(= インライン展開可能)判定を受けられるようになってる。
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<BenchmarkCode>();
        }
    }
}
