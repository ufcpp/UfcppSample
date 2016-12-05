using System.Collections.Generic;

namespace CardBattle.Models.Messages
{
    /// <summary>
    /// 誰かがコンティニューした。
    /// 復活演出とかが必要になるだろうからメッセージ送る。
    /// </summary>
    public class Continue : GameProgress
    {
        /// <summary>
        /// コンティニューしたプレイヤーのコンティニュー直後の状態。
        /// </summary>
        public IEnumerable<PlayerSnapshop> Players { get; }

        internal Continue(IEnumerable<PlayerSnapshop> players)
        {
            Players = players;
        }
    }
}
