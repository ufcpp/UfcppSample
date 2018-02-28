# dotnet global tools を試そうのコーナー

https://blogs.msdn.microsoft.com/dotnet/2018/02/27/announcing-net-core-2-1-preview-1/

.NET Core 2.1 で、Global Tools ってのを導入するらしい。
試してみてるのがこのソリューション。

## 試しに作ってみたもの

- cszip: `ZipFile.CreateFromDirectory` を呼んでるだけ
- csunzip: `ZipFile.ExtractToDirectory` を呼んでるだけ
- xstatic: .NET 標準ライブラリ中の任意の静的メソッドを呼び出せる

### 作ったものの背景

C# で何かアプリを書いていて、
PostBuild イベントを拾って何かコマンドを実行したい。
その時に、Windows だと bat を書いて、Mac とかだと sh を書いて…
とかがめんどくさい。

高々 zip コマンドを呼ぶだけでも、いろんな環境でビルドしだすと困ったりする。
せっかく dotnet コマンドがクロスプラットフォームになってるのに、ここで詰まるのは結構つらい。

dotnet でインストールできて、dotnet で実行できるものがあるならそれを使いたい。で、まあ、`ZipFile` のメソッドを呼ぶだけのコマンドがあるとそれなりに使えそうなんじゃないかと。

zip/unzip でコマンド分けるべきか、
分けるの面倒ではないか、
というかむしろいっそのこと任意の静的メソッド呼べるようにしてやろうか。
等々、遊んでみてた結果が xstatic。
名前は適当。

オーバーロード解決めんどくさい。

## ビルド/パッケージ化

Visual Studio でビルドしてもパッケージは作ってくれないみたい。
`pack.bat` にパッケージ化のためのコマンド書いてるので、これを実行。

```bat
dotnet pack -c release cszip/cszip.csproj
dotnet pack -c release csunzip/csunzip.csproj
dotnet pack -c release xstatic/xstatic.csproj
```

## 作った Global Tools のインストール

`install.bat` を実行。

```bat
dotnet install tool -g cszip --configfile .\nuget.config
dotnet install tool -g csunzip --configfile .\nuget.config
dotnet install tool -g xstatic --configfile .\nuget.config
```

補足:

- `pack.bat` で、作ったパッケージを `packages` フォルダーにコピってある
- `nuget.config` に、この `packages` フォルダーからパッケージを探す設定を書いてある

## サンプル

`sample.bat` 中で、インストールした Global Tools を呼んでみてる。

```bat
cszip packages sample.zip
csunzip sample.zip UnzippedFolder
xstatic System.TimeSpan FromSeconds 10000
xstatic System.IO.Path Combine aa bb cc
```

1. `pack.bat` の結果としてできる `packages` フォルダーを ZIP 圧縮
2. 1 で作った ZIP を解凍
3. `TimeSpan.FromSeconds(10000)` を呼んでみる
4. `Path.Combine("aa", "bb", "cc")` を呼んでみる
