using System.Collections.Generic;

namespace CardBattle.Models.Messages
{
    /// <summary>
    /// ボスの攻撃で、どのプレイヤーにどのくらいのダメージ与えて、そのプレイヤーのHPがいくつになったか。
    /// </summary>
    public class BossAttack : GameProgress
    {
        public IEnumerable<BossAttackItem> Attacks { get; }

        internal BossAttack(IEnumerable<BossAttackItem> attacks)
        {
            Attacks = attacks;
        }
    }

    /// <summary>
    /// ボス攻撃は全体攻撃があり得るので、その1個1個。
    /// </summary>
    public class BossAttackItem
    {
        /// <summary>
        /// 攻撃を受けた直後のプレイヤーのスナップショット。
        /// </summary>
        public PlayerSnapshop Player { get; }

        /// <summary>
        /// 与えたダメージ。
        /// </summary>
        public int Damage { get; }

        internal BossAttackItem(PlayerSnapshop player, int damage)
        {
            Player = player;
            Damage = damage;
        }
    }
}
