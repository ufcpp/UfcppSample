namespace CardBattle.Models.Messages
{
    /// <summary>
    /// <see cref="CommandPrompt"/>の応答。
    /// </summary>
    /// <remarks>
    /// コマンド入力は逐次手番なので、
    /// - 1人1人の分だけ受け取る
    /// - PlayerId は応答に含めなくても特定できる
    /// </remarks>
    public class CommandResonse : GameResponse
    {
        /// <summary>
        /// 引き直したいカードのインデックス。
        /// 引き直ししない場合は null でもOK。
        /// </summary>
        public int[] RedrawCardIndexes { get; }

        /// <summary>
        /// 引き直しなし。
        /// </summary>
        public CommandResonse() { }

        /// <summary>
        /// 引き直し。
        /// </summary>
        /// <param name="redrawCardIndexes">引き直したいカードのインデックス一覧。</param>
        public CommandResonse(params int[] redrawCardIndexes)
        {
            RedrawCardIndexes = redrawCardIndexes;
        }

        // もしかしたら、GC 除けに以下のようなべた展開したメンバーにした方がいいかも？
        //public int Count { get; }
        //public int Index1 { get; }
        //public int Index2 { get; }
        //public int Index3 { get; }
        //public int Index4 { get; }
        //public int Index5 { get; }

        // もしくは、以下のように引き直すところだけ true 入れる作りにするか。
        //public bool RedrawsIndex0;
        //public bool RedrawsIndex1;
        //...
    }
}
