namespace DisposePattern
{
    /// <summary>
    /// HeapAllocation プロジェクトの方で、単にヒープメモリ管理するだけなら GC を避ける理由はないという話はしたものの。
    /// 一方で、.NET 管理外リソース(ファイルとかネットワーク コネクションとか、ネイティブ相互運用で得るリソース)は GC に頼るわけにはいかない。
    /// 通常、<see cref="IDisposable"/> を実装して明示的にリソースを解放するものの、Dispose忘れや、2重Disposeを起こしかねない(C++時代に戻ったかのようなつらさ)。
    /// Dispose忘れくらい静的コード解析で対応できないのかと思うかもしれないけども、同じインスタンスを複数個所で使うと、「誰がDispose義務を負っていて、誰は負っていない」みたいなのの判定がかなり難しい。
    /// 結局、この用途だと、参照カウント方式(それなりにコストはかかる)が現実解になるかもしれないという話。
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Tests.ManualDispose.Test();
            Tests.Finalizer.Test();
            Tests.StructCantHaveFinalizer.Test();
            Tests.ReferenceCount.Test();
        }
    }
}
