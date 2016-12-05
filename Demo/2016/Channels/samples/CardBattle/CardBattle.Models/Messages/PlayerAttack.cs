using CardBattle.Models.GameRules;
using System.Linq;

namespace CardBattle.Models.Messages
{
    /// <summary>
    /// 誰の攻撃で、ボスにどのくらいのダメージ与えて、ボスのHPがいくつになったか。
    /// </summary>
    public class PlayerAttack : GameProgress
    {
        /// <summary>
        /// 攻撃時点でのプレイヤーのスナップショット。
        /// </summary>
        public PlayerSnapshop Player { get; }

        /// <summary>
        /// この時点での手札。
        /// </summary>
        public IHand Hand { get; }

        /// <summary>
        /// 与えたダメージ。
        /// </summary>
        public int Damage { get; }

        /// <summary>
        /// ダメージを与えた直後のボス体力。
        /// </summary>
        public int BossHp { get; }

        internal PlayerAttack(PlayerSnapshop player, IHand hand, int damage, int bossHp)
        {
            Player = player;
            Hand = hand;
            Damage = damage;
            BossHp = bossHp;
        }

        /// <summary>
        /// View へのバインド用。
        /// <see cref="Cards"/>のクローン。
        /// </summary>
        public Card[] Cards => Hand.Cards.ToArray();
    }
}
