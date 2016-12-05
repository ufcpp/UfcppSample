namespace TaskLibrary.Channels
{
    /// <summary>
    /// <see cref="ISender{TMessage}.Subscribe(Async.AsyncAction{TMessage})"/>に複数のハンドラーを登録したとき、
    /// ハンドラーをどう呼び出すか。
    /// </summary>
    /// <remarks>
    /// 要するにハンドラーが並列に動いてまずいかどうかで切り替え。
    ///
    /// 直列であっても、呼び出し順序の保証はできない(Subscribeした順ではあるものの、Subscribeした順なんて管理しきれない)ので注意。
    /// <see cref="Channel{TMessage}"/>の仕組み上、ハンドラー内で<see cref="IResponsiveMessage.Response"/>を設定してもらうことになるけど、
    /// 最後の1個だけが残り、かつ、どれが最後になるのかは制御できない。
    ///
    /// 複数のハンドラーを登録したいときって、大体の場合、処理の前後にログ的なものを挟みたいって要件だと思うので、
    /// 複数回Subscribe書けるより、<see cref="LoggingChannel{T}"/>を使うのを推奨。
    /// </remarks>
    public enum InvocationMode
    {
        /// <summary>
        /// 逐次(foreachで1個1個待つ)。
        /// </summary>
        Sequential,

        /// <summary>
        /// 並列(<see cref="System.Threading.Tasks.Task.WhenAll(System.Threading.Tasks.Task[])"/>で待つようになる)。
        /// </summary>
        Parallel,
    }
}
