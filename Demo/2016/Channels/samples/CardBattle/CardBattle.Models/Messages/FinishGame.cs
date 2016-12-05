using System.Collections.Generic;

namespace CardBattle.Models.Messages
{
    /// <summary>
    /// ゲーム終了メッセージ。
    /// </summary>
    public class FinishGame : GameProgress
    {
        /// <summary>
        /// ゲームの最終状態。
        /// </summary>
        public GameStatus Status { get; }

        /// <summary>
        /// 最終ターン。
        /// </summary>
        public int Turn { get; }

        /// <summary>
        /// 最終的なボスのHP。
        /// </summary>
        public int BossHp { get; }

        /// <summary>
        /// プレイヤーの最終状態。
        /// </summary>
        public IReadOnlyCollection<PlayerSnapshop> Players { get; }

        internal FinishGame(GameStatus status, int turn, int bossHp, IReadOnlyCollection<PlayerSnapshop> players)
        {
            Status = status;
            Turn = turn;
            BossHp = bossHp;
            Players = players;
        }
    }
}
