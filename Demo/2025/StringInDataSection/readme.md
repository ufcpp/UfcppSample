
[String literals in data section](https://github.com/dotnet/roslyn/blob/main/docs/features/string-literals-data-section.md) を試してみる。

NormalAscii, NormalJapanese, DataSectionAscii, DataSectionJapanese の4つのプロジェクトがあって、
全部「長い文字列を `Console.WriteLine` してるだけ」のプログラム。
ビルド オプションと文字列の内容が違うだけで、以下の内容。

* ビルド オプション
  * Normal: オプション未指定
  * DataSection: `experimental-data-section-string-literals=0` 指定あり
* 文字列リテラル
  * Ascii: ASCII 文字のみ
  * Japanexe: ひらがな(+ 改行文字)のみ

(補足: UTF-8 だと、ASCII は半分になるものの、日本語とかは1.5倍になる。)

リリースビルド時の DLL サイズ:

| DLL | サイズ |
| --- | --- |
| NormalAscii.dll | 68,608  |
| DataSectionAscii.dll | 36,864 |
| NormalJapanese.dll | 53,248 |
| DataSectionJapanese.dll | 75,776  |
