# チャネル

いわゆるメッセンジャー。
モデルからビューに何かしてほしい時にイベントを飛ばす中間媒体。

## Reactive Extensionsとの類似

担ってる債務的にはReactive Extensions(Rx)に近い。
RxというかIObservableとの差は、Subscribeする側がIObserverじゃなくてAsyncFuncなこと。
要するに、以下の点がRxとの違い。

- イベントのハンドラー側の処理が終わるのをawaitしたい
- ハンドラー側から結果が返ってくる/結果を受け取りたいことがある
  - 結果を複数のハンドラーから返されても困るので、基本的にはハンドラーは1つだけの想定
- イベント送信には並列送信(配列でメッセージを投げて、それを個別にハンドルしてもらって、WhenAllで並列に待つ)があり得る

## 用途の例

前節のような機能が必要になるのは、以下のような利用場面を想定しているから。

RPGとかのバトルとかの、コマンド入力でゲームが進行していくような例を考える。
あるいは、トランプのゲーム見たいな、手札の入れ替えとかの例を考える。

- プレイヤーの入力に応じてゲームの進行を決める
  - プレイヤーに入力を促すメッセージを投げて、プレイヤーからの応答を待たないといけない
  - 逐次手番(七並べとかみたいに、一定順序で1人1人手札を出すようなやつ)も、同時手番(じゃんけんみたいに、全プレイヤーが同時に手を出すようなやつ)もあり得る
- 応答を必要ないものでも、結果表示用のアニメーション再生などが終わるのを待ってからゲーム進行したい

## チャネルのレイヤー

メッセージ送信側と受信側でインターフェイスを分けた

- ISender<T>  : メッセージ送信側
- IReceiver<T>: メッセージ受信側
- Channel<T>  : ISenderかつIRecieverな、媒介クラス

送信側(IReceiverのインスタンスを持って、メッセージを渡す)は、以下のようなコードを書く。

```cs
// 応答をもらわなくていいもの
await receiver.SendAsync(message)

// 応答をもらうもの
var res = (await receiver.SendAsync(message)).GetResponse()
```

受信側(ISenderのインスタンスを持って、メッセージを処理する)では、以下のようなコードを書く。

```cs
ISender<Holder<GameProgress>> c = new Channel<GameProgress>();

// プレイヤー単位でメッセージを分配
var d = c.ObserveOn().Distribute();

// MyPlayerId な IResponsiveMessage だけ受け取る
var playerChannel = d.GetChannel(0);

playerChannel.Subscribe(
    // メッセージの型に応じてスイッチ
    Create<CommandPrompt>((x, ct) => OnCommandPrompt(x, r)),
    Create<ContinuePrompt>((x, ct) => OnContinuePrompt(x, r))
    );
```
