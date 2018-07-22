# IValueTaskSource

[ValueTask](https://www.nuget.org/packages/System.Threading.Tasks.Extensions/)には、バージョン 4.5 (.NET Core 2.1 なら標準対応)から、`IValueTaskSource`/`IValueTaskSource<T>`インターフェイスを受け付けるコンストラクターが増えてる。これまでだと、`Task`/`Task<T>`しか受け付けてなかったけど、もうちょっと特殊な実装をした非同期処理クラスを作って`await`したいという要求に応えたもの。

例えば、[Channels](https://github.com/dotnet/corefx/tree/master/src/System.Threading.Channels)とか[Pipelines](https://github.com/dotnet/corefx/tree/master/src/System.IO.Pipelines)が内部で使ってる。
この辺りを眺めてみてる感じ、Channelsの中にある[`AsyncOperation`クラス](https://github.com/dotnet/corefx/blob/master/src/System.Threading.Channels/src/System/Threading/Channels/AsyncOperation.cs)が割とわかりやすそうだった。

ということで、その`AsyncOperation`を写経。デモ用に、最低限の機能に削って、コメントを日本語化した。

## 補足

### AsyncOperation

この`AsyncOperation`クラスは、用途としては`TaskCompletionSource`クラス(`System.Threading.Tasks`名前空間)と同じ(いわゆるPromise)。ただ、

- `Task`じゃなくて`ValueTask`を使う
  - アロケーションが1回少なくてお得
- キャッシュ/リセットできる構造になってる
  - 何回メソッドを呼び出しても、`new` されるインスタンスは1個だけ
- continuation を1個しか受け付けない作りにして最適化
  - 要するに、`List<(Action<object> continuation, object state)>`みたいなものを持たなくて済む

とかをやってる。

### continuation が1個だけ

最後の「continuation を1個しか受け付けない作り」って言うのは、要するに、「同時に2カ所で`await`することないでしょ？」という感じ。↓みたいなコードを書くと実行時エラーになる。

```cs
var t = MAsync();
await t;
await t;
```

`List<T>`が要らなくなるとか、`lock` が要らなくなるとかあり、結構パフォーマンスに寄与しそう。
