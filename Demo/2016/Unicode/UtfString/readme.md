Utf8String: https://github.com/dotnet/corefxlab/tree/master/src/System.Text.Utf8
の簡易実装版。

Utf8Stringは https://github.com/dotnet/corefxlab/tree/master/src/System.Slices に依存してる。

Slicesは安定性とかパフォーマンス向上のためにランタイムのレベルで手を入れたくて、C# 7時点ではリリースできない。
Slicesに依存してるUtf8Stringも当然、リリースできない。

なので、説明用に、Utf8Stringの簡易実装を作ってみた。
