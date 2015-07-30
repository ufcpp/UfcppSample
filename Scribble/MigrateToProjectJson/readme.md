# packages.config → project.json

要するに NuGet v3 化。

## project.json

元々は ASP.NET 5/.NET Core/.xproj 向けのプロジェクト管理情報が入った JSON ファイル。

NuGet v3 で、その project.json のサブセットを使って、.csproj/.vbproj でも NuGet パッケージ管理を project.json でやるようになった。

ただし、公式にサポートするのは以下の3つだけ。

- ASP.NET 5
- Modern PCL (プロジェクト テンプレートの「Class Library (package)」/「クラス ライブラリ (パッケージ)」で作るやつ)
- Universal Windows Platform アプリ(以下、UWP)

前2つは元から .xproj 形式。UWP だけがこの新しい仕組み(.csproj + project.json)で動いてる。

## .csproj + project.json

サポート外(自己責任利用)ではあるものの、任意のプロジェクトから project.json を使うことが可能。

例えば、これまでの NuGet パッケージ管理「packages.confing」で、Rx-Main を参照すると以下のようになってたはず。

```xml
<?xml version="1.0" encoding="utf-8"?>
<packages>
  <package id="Rx-Core" version="2.3.0-beta2" targetFramework="net46" />
  <package id="Rx-Interfaces" version="2.3.0-beta2" targetFramework="net46" />
  <package id="Rx-Linq" version="2.3.0-beta2" targetFramework="net46" />
  <package id="Rx-Main" version="2.3.0-beta2" targetFramework="net46" />
  <package id="Rx-PlatformServices" version="2.3.0-beta2" targetFramework="net46" />
</packages>
```

これを、project.json という名前で、以下のような JSON ファイルに置き替える。

```json
{
  "frameworks": {
    "net46": {}
  },
  "runtimes": {
    "win": {},
    "win-anycpu": {}
  },
  "dependencies": {
    "Rx-Main": "2.3.0-beta2"
  }
}
```

手動でファイルの置き換えをした後、プロジェクトを読み込みなおす(ソリューションを閉じて開きなおすとかでも OK)と、NuGet v3 でパッケージ管理される状態になる。

## NuGet v3 で新しくなったこと

NuGet v3 で、.NET Core/.xproj (2015/7/20 の Visual Studio 2015 リリース時点でベータ状態)の一部分を前倒しで先に使えるようになった状態。

### 依存関係を自動で解決

project.json では、直接依存しているものだけを書けばよくなった。

上記の例の packages.config (NuGet v2)では、Rx-Main を参照するだけで、それが依存している Rx-Core, Rx-Interfaces, Rx-Linq, Rx-PlatformServices が一緒に追加される。
一方、project.json (NuGet v3)では、直接参照している Rx-Main の1行だけになった。

アップグレードやアンインストールの作業がだいぶ楽になるはず。

### .csproj/.vbproj を汚さなくなった

NuGet v2 では、packages.config にパッケージ情報が入るだけじゃなくて、.csproj 側にダウンロードしてきた DLL への参照情報が入ってた。
しかも hint path がソリューションを基準とした相対パスなので、1つのプロジェクトを複数のソリューションから参照するとビルドできなくなったりする。

NuGet v3 では、project.json だけで完結して、.csproj/.vbproj には何も書かなくなった。
1つのプロジェクトを複数のソリューションから参照しても平気に。
また、ソースコード バージョン管理での衝突の可能性も減った。

### キャッシュがユーザー単位に

NuGet v2 では、NuGet サーバーから取ってきたパッケージは、ソリューション直下の `packages` フォルダーにキャッシュされてた。

一方、NuGet v3 では、ユーザー フォルダーの下に、`.nuget/packages` というフォルダーを作ってそこにキャッシュされる。
同じユーザーのすべてのソリューションでキャッシュが共有される。

### まとめ

変更点

- 依存関係を自動解決
- .csproj/.vbproj を汚さない
- キャッシュがユーザー単位

その結果得られる利点

- アップグレード/アンインストール作業が楽
- 1つのプロジェクトを複数のソリューションから参照しても平気
- キャッシュ効率アップ

## 移行プログラム

packages.config から project.json に自動的に置き替えるプログラム書いてみた。

- packages.config からパッケージ情報を読み取る
- project.json を作る
- .csproj から、パッケージのDLLを `Reference` タグで参照してるところを消す
- .csproj の packages.config 行を project.json に書き替える

### 注意点

Visual Studio Tools for Windows が必須。

[開発者センター ダウンロードページ](https://dev.windows.com/ja-jp/downloads)で、「Windows 開発者ツールが付属した Visual Studio 2015」のところからダウンロード。
