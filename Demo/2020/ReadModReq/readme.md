# modreq の動作確認

`modreq` の説明自体はまた改めて別の機会に。
ここでは、`modreq` を使って古いコンパイラーからの参照を拒絶してる様子だけをデモ。

## modreq を使っている機能

- `in` 引数 (C# 7.2 移行)
  - ただし、`abstract` か `virtual` の時だけ `modreq` が付く
- ジェネリック型引数の `unmanaged` 制約 (C# 8.0 移行)

要するに、正しい解釈ができないバージョンのコンパイラーから下手に触られると困る機能に `modreq` を付けてる。

## フォルダー構成

- `LibModReq`
  - `modreq` が付与される機能(`in` 引数と `unmanaged` 制約)を使ってるライブラリを用意
- `ReadModReq`
  - `LibModReq` を `Microsoft.CodeAnalysis.CSharp` を使って参照しているコード
- `CompilerVersions` ソリューション フォルダー以下 (`Csharp7.0`, `Csharp7.2`, `Csharp8.0`)
  - `ReadModReq` と同じソースコードで、`Microsoft.CodeAnalysis.CSharp` のバージョンだけが違うもの

対応するバージョンの .NET SDK をインストールしなくても動作確認できるように、`Microsoft.CodeAnalysis.CSharp` を使ってコンパイルして、`Microsoft.CodeAnalysis.CSharp` のバージョン違いで C# バージョンを選んでる。
(正確には、`GetDiagnostics` で診断だけ取ってる。`Emit` とかはしてない。)

## 実行結果

`Csharp7.0`, `Csharp7.2`, `Csharp8.0` を実行すると、

- `Csharp7.0`: `in` 引数と `unmanaged` 制約の行でエラー
- `Csharp7.2`: `unmanaged` 制約の行でエラー
- `Csharp8.0`: 何も表示されない(エラーがない)

になる。
未対応の個所では「not supported by the language」みたいなメッセージが出る
(これが `modreq` の効果)。
