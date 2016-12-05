using SystemAsync;
using System;
using System.Linq;

namespace TaskLibrary.Channels
{
    /// <summary>
    /// 型を見てアクションを呼び分けるのに使う。
    /// <see cref="Type"/>は、<typeparamref name="TBase"/>の子クラスが入る想定。
    /// </summary>
    /// <remarks>
    /// ほぼ、<see cref="KeyValuePair{Type, AsyncAction{TBase}}"/>。
    /// <see cref="Create{TMessage}(AsyncAction{TMessage})"/>の戻り値をKeyValuePairにして、static classにしても良かったかも。
    ///
    /// 初期化時にいったんこの構造体作るだけ。すぐにToDictionaryして使う。
    /// </remarks>
    /// <typeparam name="TBase">共通基底クラスの型。</typeparam>
    public struct TypedAsyncAction<TBase>
    {
        /// <summary>
        /// 受け付ける型。
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 呼び出す処理。
        /// </summary>
        public AsyncAction<TBase> Action { get; }

        /// <summary></summary>
        /// <param name="type"><see cref="Type"/></param>
        /// <param name="action"><see cref="Action"/></param>
        public TypedAsyncAction(Type type, AsyncAction<TBase> action)
        {
            Type = type;
            Action = action;
        }

        public static TypedAsyncAction<TBase> Create<TMessage>(AsyncAction<TMessage> action)
            where TMessage : TBase
            => new TypedAsyncAction<TBase>(typeof(TMessage), (arg, ct) => action((TMessage)arg, ct));
    }

    public static partial class Channel
    {
        /// <summary>
        /// 送信元から送られてくるメッセージはいろんな型があるはずで、<typeparamref name="TMessageBase"/>はその共通基底クラスになってるはず。
        /// これを、具象型に応じて分岐、型ごとにイベントハンドラーを登録できるようにする。
        /// </summary>
        /// <typeparam name="TMessageBase"></typeparam>
        /// <param name="sender"></param>
        /// <param name="actions"></param>
        /// <returns></returns>
        /// <remarks>
        /// A, B, C を T の子クラスとして、以下のように使う想定。
        /// <code><![CDATA[
        /// using static TypedAsyncAction<T>;
        /// 
        /// sender.Subscribe(
        ///     Create<A>(a => ...),
        ///     Create<B>(b => ...),
        ///     Create<C>(c => ...));
        /// ]]></code>
        /// 
        /// C# 7のtype switch構文が入ったら無用の長物かも。普通に、以下のように書くことできるし。
        ///
        /// <code><![CDATA[
        /// sender.Subscribe((T message, CancellationToken ct) =>
        /// {
        ///     switch (message)
        ///     {
        ///         case A a: => ...
        ///         case B b: => ...
        ///         case C c: => ...
        ///     }
        /// });
        /// ]]></code>
        ///
        /// <see cref="Channel.Where{TSource}(ISender{TSource}, Func{TSource, bool})"/>とかでも同様のことはできるんだけど、
        ///
        /// - ハッシュテーブル的な分岐がしたい
        /// - 型の数だけ<see cref="AsyncActionList{T}.Add(AsyncAction{T})"/>されたくない
        ///
        /// など、主に計算量的なコストを避けるためにこのメソッドがある。
        /// </remarks>
        public static IDisposable Subscribe<TMessageBase>(this ISender<TMessageBase> sender, params TypedAsyncAction<TMessageBase>[] actions)
        {
            var dic = actions.ToDictionary(x => x.Type, x => x.Action);
            return sender.Subscribe(async (message, ct) =>
            {
                var t = message.GetType();
                AsyncAction<TMessageBase> handler;
                if (dic.TryGetValue(t, out handler))
                    await handler(message, ct);
            });
        }
    }
}
