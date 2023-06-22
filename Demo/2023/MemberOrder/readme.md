# Module Initializer の実行順序

## 概要

Module Initializer の実行順序は、ファイルが分かれてる場合、ファイルをコンパイラーに渡した順。

で、ファイルが渡る順序は以下のようになってそう。

* csc 直呼びの場合、引数の順序そのまま
* csproj に `<Compile Include="..."/>` を明示した場合、その並び順
* `<Compile Include="**/*.cs"/>` だと、ビルドツール次第
    * 昔ながらの MSBuild.exe は UTF-16 比較っぽい
    * dotnet build は UTF-8 比較っぽい
    * いずれも Ordinal 比較
 
 ## csc 引数順

 例えば csc 直呼びするなら、

 ```
$csc = 'C:\Program Files\dotnet\sdk\8.0.100-preview.5.23303.2\Roslyn\bincore\csc.dll'
$shared = "C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.0-preview.5.23280.8\"
dotnet $csc -t:library -r:"$shared\System.Private.CoreLib.dll" -out:Az.dll .\A.cs .\Z.cs
dotnet $csc -t:library -r:"$shared\System.Private.CoreLib.dll" -out:Za.dll .\Z.cs .\A.cs 
```

こんな感じで Az.dll と Za.dll の結果が変わる。

## Ab.csproj, Ba.csproj

必要な C# ファイルを明示的に csproj に並べたもの。

Ab.csproj:

```xml
  <ItemGroup>
    <Compile Include="../Program.cs" />
    <Compile Include="../A.cs" />
    <Compile Include="../B.cs" />
  </ItemGroup>
```

この場合、実行結果は

```
Latin A
Latin B
```

Ab.csproj:

```xml
  <ItemGroup>
    <Compile Include="../Program.cs" />
    <Compile Include="../B.cs" />
    <Compile Include="../A.cs" />
  </ItemGroup>
```

A.cs と B.cs の並びが逆。
この場合、実行結果は

```
Latin B
Latin A
```

## Implicit.csproj

`*.cs` でビルド システムにお任せしてしまった場合。

Implicit.csproj:

```xml
  <ItemGroup>
    <Compile Include="../*.cs" />
  </ItemGroup>
```

どうも、Visual Studio (MSBuild.exe) と、dotnet CLI (dotnet build/run) で順序が違うっぽい。

Visual Studio の場合:

```
Latin A
Latin B
Latin Z
Latin-1 A
Greek α
Cyrillic д
カタカナ ア
漢字
Emoji ??
漢字(サロゲートペア) ??(ほっけ)
半角カナ ｱ
```

(絵文字とかサロゲートペアな文字が表示されないのは現状の Visual Studio のバグ。)

絵文字(サロゲートペア、U+D800 近辺)が半角カナ(U+FF00 近辺)の前にいるということはおそらく UTF-16 で比較してる。

dotnet run の場合:

```
Latin A
Latin B
Latin Z
Latin-1 À
Greek α
Cyrillic д
カタカナ ア
漢字
半角カナ ｱ
Emoji 🐈
漢字(サロゲートペア) 𩸽(ほっけ)
```

絵文字が後ろなのでおそらく UTF-8 比較。

## Explicit.csproj

`*.cs` を使いつつ、一度 Remove → Include することで一部の順序を明示。

Explicit.csproj:

```xml
  <ItemGroup>
    <Compile Include="../*.cs" />
    <Compile Remove="../A.cs" />
    <Compile Include="../A.cs" />
  </ItemGroup>
```

(絵文字の順序が先ほどの理屈で環境依存なものの)
これで「A.cs が最後」だけは保証される。

```
Latin B
Latin Z
Latin-1 À
Greek α
Cyrillic д
カタカナ ア
漢字
半角カナ ｱ
Emoji 🐈
漢字(サロゲートペア) 𩸽(ほっけ)
Latin A
```
