namespace CardBattle.Models.GameRules
{
    /// <summary>
    /// キャラ性能的なの。
    /// </summary>
    public class Ability
    {
        /// <summary>
        /// 体力の最大値。
        /// </summary>
        public int MaxHp { get; }

        /// <summary>
        /// <see cref="Suit.Spades"/>の攻撃力が上がる度合い。
        /// </summary>
        public int Spades { get; }

        /// <summary>
        /// <see cref="Suit.Hearts"/>の攻撃力が上がる度合い。
        /// </summary>
        public int Hearts { get; }

        /// <summary>
        /// <see cref="Suit.Diamonds"/>の攻撃力が上がる度合い。
        /// </summary>
        public int Diamonds { get; }

        /// <summary>
        /// <see cref="Suit.Clubs"/>の攻撃力が上がる度合い。
        /// </summary>
        public int Clubs { get; }

        /// <summary>
        /// 同じ数字があったときに掛かるボーナス度合い。
        /// </summary>
        public int Pair { get; }

        /// <summary>
        /// 連番があったときに掛かるボーナス度合い。
        /// </summary>
        public int Straight { get; }

        /// <summary>
        /// 同柄があったときに掛かるボーナス度合い。
        /// </summary>
        public int Flush { get; }

        /// <summary>
        /// 速さ。行動順に影響あるやつ。
        /// </summary>
        public int Speed { get; }

        /// <param name="maxHp"><see cref="MaxHp"/></param>
        /// <param name="spades"><see cref="Spades"/></param>
        /// <param name="hearts"><see cref="Hearts"/></param>
        /// <param name="diamonds"><see cref="Diamonds"/></param>
        /// <param name="clubs"><see cref="Clubs"/></param>
        /// <param name="pair"><see cref="Pair"/></param>
        /// <param name="straight"><see cref="Straight"/></param>
        /// <param name="flush"><see cref="Flush"/></param>
        public Ability(int maxHp, int spades, int hearts, int diamonds, int clubs, int pair, int straight, int flush, int speed)
        {
            MaxHp = maxHp;
            Spades = spades;
            Hearts = hearts;
            Diamonds = diamonds;
            Clubs = clubs;
            Pair = pair;
            Straight = straight;
            Flush = flush;
            Speed = speed;
        }
    }
}
