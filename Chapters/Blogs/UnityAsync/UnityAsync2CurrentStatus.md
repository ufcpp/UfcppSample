# Unity上でasync/await: 現状 (UnityAsync2CurrentStatus)

[MinimumAsyncBridge](https://github.com/OrangeCube/MinimumAsyncBridge)の現状について。
どのくらいまっとうに動いているか。

ちなみに、[7月に書いたとき](http://ufcpp.net/blog/2015/07/unityasyncbridge/)には自分の個人アカウントのリポジトリにコードを置いていましたが、
今は、[会社アカウント]((https://github.com/OrangeCube/)の方に移っています。

## 実装状況

[背景](UnityAsync1Background)でも説明した通り、`Run`メソッド(別スレッドで処理を開始する)は実装していません。
ここはUnityコルーチンとかRxとつないで使う想定です。

一方で、Minimumと言いつつ、`Delay`、`WhenAll`、`WhenAny`は実装しました。

これで、実プロジェクトに組み込んで使っていますが、ほぼ要件足りています。
async/awaitを使いたい動機が主にサーバーとの通信(I/O非同期)で、スレッド処理(CPU非同期)をあまり必要としていないという背景はありますが。

4か月使ってみて、MinimumAsyncBridge由来の問題に当たって困ったことはほぼないですし、
困ったのはだいたい実装ミスで、現在では修正済みです。

## IteratorTasksからの移行

うちのプロジェクト、7月までは非同期処理に[IteratorTasks](https://github.com/OrangeCube/IteratorTasks)を使っていました。

IteratorTasksも「`Task`もどき」なので、移行作業、そこまでは苦労しませんでした。必要なのは、

- 名前空間を `IteratorTasks` から `System.Threading.Tasks` に変える
- `IEnumerator`、`yield return` を `Task`、`await` に変えて回る

を地道にやること。それなりに地道な作業なんですが… バグ修正含めて1・2週間ほど。

もし、IteratorTasksを使っていただけてる方で、この1・2週間ほどの時間は取れないという場合があれば、
IteratorTasksに対するawaiter実装もあるので、これが使えるかも。

- [IteratorTasks.AsyncBridge](https://github.com/OrangeCube/MinimumAsyncBridge/tree/master/src/IteratorTasks.AsyncBridge)

## 本家 Task の完全下位互換

`Run`メソッドみたいに意図して実装していないものをのぞいて、
本家`Task`クラスと互換性があります。

要するに、完全下位互換。
[MinimumAsyncBridge](https://github.com/OrangeCube/MinimumAsyncBridge)を使って動けば、
.NET 4.5以降では本家`Task`を使って100%動きます
(逆は、実装していないメソッドを避けないとダメ)。

ちなみに、[型フォワーディング](http://ufcpp.net/study/csharp/package/typeforwarding/?p=3#backporting)という仕組みを使っていて、

- .NET 3.5 (Unity)向けライブラリで[MinimumAsyncBridge](https://github.com/OrangeCube/MinimumAsyncBridge)を使う
- .NET 4.5向けライブラリで本家`Task`クラスを使う
- これらのライブラリを混在させて、.NET 4.5向けアプリを書く

というようなこともできます。.NET 4.5向けアプリから[MinimumAsyncBridge](https://github.com/OrangeCube/MinimumAsyncBridge)を使うと、
本家`Task`クラスに転送されます。

実際、今作っているゲームは.NET 3.5と4.5混在です。

- ちょっとしたデバッグをコンソール アプリで
- ボット使った動作テストしたいときにボットをコンソール アプリで
- 可視化したいデータがあったときにWPFでGUIをちょろっと
- データの編集ツールはWPF製
- (基本、サーバー側は外注なので他社、かつ、PHPなものの)一部、CPU酷使しそうな機能はC#で実装

これらのアプリは全部、Unity上で動かすゲームのロジックを共有しています。
また、現在は、これらは全部.NET 4.6にバージョンアップ済みです。


### 既知の制限: .NET 4では使えない

.NET 3.5と4.5以上では問題なく動きますが、.NET 4では動きません。

おそらくピンポイントに.NET 4が必要になる(.NET 4は使えるけど4.5以上へのアップグレードはできない)場面はほとんどないと思いますが、一応。

これは、.NET 4に、async/awaitはできない中途半端なバージョンの`Task`クラスが存在するため、
型フォワーディングしづらいせいです
(新規クラスを足すのは簡単。既存クラスにメソッドを追加するのは無理)。

### NuGetパッケージ

[NuGetパッケージ化](https://www.nuget.org/packages/MinimumAsyncBridge/)してあって、

- .NET 3.5 のプロジェクトからこのNuGe パッケージを参照すると、バックポーティング実装が参照される
- .NET 4.5 のプロジェクトからだと、型フォワーディング実装が参照される

という挙動をします。とりあえず、NuGet経由で参照するとわずらわしい設定不要で、.NET 3.5と4.5の混在ができます。

### UniRx向け型フォワーディング

同じような仕組み、[UniRx](https://github.com/neuecc/UniRx)に対しても使えます。つまり、

- Unity向けライブラリではUniRx使う
- .NET 4.5向けライブラリでは[本家Rx](https://www.nuget.org/packages/Rx-Main/)を使う
- これらのライブラリを混在させて使う

とか。とりあえずこの作業やって、Pull Request は出してあったりします。

- [https://github.com/neuecc/UniRx/pull/88](https://github.com/neuecc/UniRx/pull/88)

やったことは、

- 名前空間が `UniRx` になっているところを`System.Reactive`に戻す
- 意図して本家Rxとは実装変えていた部分を元に戻す(`FirstAsync`→`First`とか)
- 別途、型フォワーディング用のライブラリを実装
- 元々のUniRx利用者に影響が出ないように、`#if`分岐

とかです。

### 既知の制限: C# 6.0 がらみ

[MinimumAsyncBridge](https://github.com/OrangeCube/MinimumAsyncBridge)というかIL2CPP側の問題なんですが、
IL2CPPを使う場合(つまり、iOSで実行する場合)、ちょっとだけC# 6.0で使えない構文があります。

現状、以下の構文はIL2CPPでのビルドに失敗、あるいは、iOS実行時に実行時例外を起こします。
async/awaitの問題というか、だいたいは例外処理がらみの問題です。

- catch finally 内での await は使えない
- [catch句内での再スロー](http://ufcpp.net/study/csharp/misc_stacktrace.html#rethrow)ができない
- [例外フィルター](http://ufcpp.net/study/csharp/oo_exception.html#exception-filter)が使えない

## iOS実行・Android実行

iOSでの実行は、IL2CPPを使えば動きます。

IL2CPPのバグをいくつか踏み抜いて、Unity 5.2.2では動かなくなっていたりしましたが、
Unityへのフィードバックの結果、Unity 5.2.3では直っています。

(Unity 5.2.2の時は、ビルドで失敗していました。
ビルドまで成功したら、実行時エラーが出たことはこれまでどのバージョンでもありませんでした。)

ちなみに、Android実行では1度も問題出ていません。

## 本家Taskクラスらの劣化

`Run`メソッドを実装していないなど、意図的にそうした部分の他に、いくらか残念な部分があります。

- 一部、多少効率悪いはず
- 例外のスタックトレース紛失

### 効率

`TaskCompletionSource`クラスとかawaiterの実装などはほぼベタに本家からの移植なので、そんなに差はないはずです。

問題は`Delay`、`WhenAll`、`WhenAny`の実装。
本家はこの辺りをネイティブ実装していて、
[MinimumAsyncBridge](https://github.com/OrangeCube/MinimumAsyncBridge)では別実装で移植しました。
この辺りは、本家ほど最適な実装はできないんで、さすがに効率ちょっと悪いはず。

- `Delay`: `System.Threading.Timer`で実装
- `WhenAll`/`WhenAny`: 複数の`Task`をリスト管理
  - 本家実装だと同時実行するタスク数がむちゃくちゃ多くてもメモリ食わない実装になってるらしいけど、こっちはしっかりタスク数分のメモリを食う。

### スタックトレース

本家`Task`を使ったasync/awaitの何がすごいって、
スレッド間でスタックトレース情報を伝搬させて、スタックトレースを追えるようにしていることなんですが。

ここはさすがに移植できなかったというか、移植が不可能だったりします。
この機能の実現には.NET 4.5で追加された`System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture`が必須です。
.NET 3.5向けのバックポーティングはできません。

なので、デバッグは本家`Task`よりも少し大変になります(非同期メソッドの呼び出し元が分からなくなったりする)。

## まとめ

[MinimumAsyncBridge](https://github.com/OrangeCube/MinimumAsyncBridge)、
実装し始めた当初の予想よりもだいぶあっさりと完成。

- .NET 3.5/4.5混在でも使えます
  - .NET 4では使えません
- ほとんど問題起きず、少ない修正で安定してしまいました
- iOS(IL2CPP)でも動きます
  - Unity 5.2.2でIL2CPPのバグで一瞬動かなくなっていたものの、5.2.3で修正されました
- 本家`Task`の完全下位互換です
  - [MinimumAsyncBridge](https://github.com/OrangeCube/MinimumAsyncBridge)で動けば、本家では100%動く
  - 一部、多少性能的に劣る
  - スタックトレース紛失で少しデバッグが大変
