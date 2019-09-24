namespace WeakReferenceSample.WeakTable
{
    /// <summary>
    /// 名簿の1項目の情報。
    ///
    /// 仮に、このクラスが自作じゃなくて、どこか別のライブラリで定義されているものとする。
    /// 自分のプログラムでは、ID と名前だけじゃなくて、住所も足したくなったとして…
    /// </summary>
    public class Person
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
