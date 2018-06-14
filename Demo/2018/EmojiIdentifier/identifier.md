# 識別子に使える文字

unicode.org が、汎用識別子に Unicode を使う場合に推奨するパターンについて仕様を出してる。

[Unicode® Standard Annex #31 - UNICODE IDENTIFIER AND PATTERN SYNTAX](http://unicode.org/reports/tr31/)

C#、Java、Go なんかは概ねこれに沿って識別子に使える文字を規定してるっぽい。

## ZERO WIDTH JOINER

ゼロ幅接合子(zero width joiner)を無視するみたいな仕様もこれに沿った結果っぽい。

```cs
using System;

class Program
{
    static void Main()
    {
        // U+200D = ZWJ
        var a\u200Db = 1;
        ab = 2;

        Console.WriteLine(a\u200Db); // ab 扱いなので、2に上書きされてる
        Console.WriteLine(nameof(a\u200Db)); // これも、表示されるのは「ab」
    }
}
```

## 拡張面文字

拡張面の文字、UTF-16 の場合サロゲートペアになってるやつの扱い。

拡張面の文字にも1文字1文字カテゴリーが設定されているので、「letter は全部識別子として使える」となっている言語なら、拡張面文字でも letter な限り識別子として使えるべき。

実際、Java、Go は使える。

Java:

```java
public class HelloWorld
{
  public static void main(String[] args)
  {
    int 𩸽 = 2;
    int 𒀀 = 3;
    int 𓀀 = 5;
    System.out.print(𩸽 * 𒀀 * 𓀀);
  }
}
```

Go:

```go
package main
import "fmt"
func main() {
  𩸽 := 2
  𒀀 := 3
  𓀀 := 5
  fmt.Println(𩸽 * 𒀀 * 𓀀)
}
```

が、C# コンパイラーは現状(少なくとも Roslyn 2.8/Visual Studio 15.7/C# 7.3時点)ではバグってる。UTF-16で処理していて、サロゲートペアは全部「Surrogateカテゴリー」扱いにしているせい。以下のコードはコンパイルエラーになる。

```cs
class Program
{
    static void Main()
    {
        // Error CS1056 Unexpected character
        int 𩸽 = 2; // CJK Extension B
        int 𒀀 = 3; // Cuneiform
        int 𓀀 = 5; // Egyptian Hieroglyph
        System.Console.WriteLine(𩸽 * 𒀀 * 𓀀);
    }
}
```
