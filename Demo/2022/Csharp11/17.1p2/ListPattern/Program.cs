using System.Text;

//## リスト パターン

var x = new[] { 1, 2, 3, 4, 5 };

if (x.AsSpan() is [1, .. var y, 5]) // これで、1開始、5終端にマッチしたとき、真ん中の要素を取る。
{
    foreach (var item in y) // 2, 3, 4
    {
        Console.WriteLine(item);
    }
}

//## 無駄に再帰させてみる

// こんなのも有効な C# コードだったりする。
// 特に意味はない。 new int[0] is [] と一緒。結局、 Length == 0 と一緒。
if (new int[] { }[..][..][..] is [.. [.. []]])
{
}

//## 利用例1: BOM 除去

// こういう使い方なら実用的かなぁという例。
// UTF-8 なシーケンスから BOM の除去。
var utf8 = Encoding.UTF8.GetBytes("何か").AsSpan();

if (utf8 is [0xEF, 0xBB, 0xBF, .. var noBom])
{
    utf8 = noBom;
}

foreach (var c in utf8)
{
    Console.WriteLine(c);
}

//## 利用例2: head-tail 再起

// まあ、よくあるのは関数型言語でよく見る head-tail。
m(new byte[] { 1, 2, 3, 4 });

void m(Span<byte> span)
{
    if (span is [var head, .. var tail])
    {
        m(tail);
        Console.WriteLine(head);
    }
}
