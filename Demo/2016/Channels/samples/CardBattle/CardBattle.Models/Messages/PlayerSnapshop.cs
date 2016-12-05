using CardBattle.Models.GameRules;

namespace CardBattle.Models.Messages
{
    /// <summary>
    /// ある時点の<see cref="Player"/>のスナップショット。
    /// </summary>
    /// <remarks>
    /// 実際のゲームだと補助魔法とかで能力値も変動するんだけど、このサンプルだとその辺り考えない。
    ///
    /// ボスの方にもほんとはBossSnapshotみたいなクラスが必要だけど、今回はボスがHPしか持ってないし、そっちはint 1個で済ます。
    /// </remarks>
    public class PlayerSnapshop
    {
        /// <summary>
        /// プレイヤーのID。
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// 体力。
        /// </summary>
        public int Hp { get; }

        internal PlayerSnapshop(Player player)
        {
            Id = player.Id;
            Hp = player.Hp;
        }
    }

    public static class PlayerExtensions
    {
        public static PlayerSnapshop Snapshot(this Player p) => new PlayerSnapshop(p);
    }
}
