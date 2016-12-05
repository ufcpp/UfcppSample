using System;
using System.Linq;

namespace CardBattle.Models.GameRules
{
    /// <summary>
    /// 手札の役判定クラス。
    /// 今回、
    /// - pair    : 同じ数字のカード枚数の最大値(例えば 3, 3, 3, 2, 2 とかだと3枚)
    /// - straight: 連番のカードの枚数の最大値(例えば 1, 2, 3, 7, 8 とかだと3枚)
    /// - flush   : 同柄のカード枚数の最大値(例えば ♡, ♡, ♢, ♠, ♣ とかだと2枚)
    /// を判定。
    /// そろった数字は何番かとかは無関係。
    /// </summary>
    public class Judge
    {
        /// <summary>
        /// pair, straight, flush の枚数を集計。
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>それぞれの枚数</returns>
        /// <remarks>
        /// 最初はジョーカーをまじめに判定するつもりでいたけど、面倒になったのでやってない。
        /// ジョーカーは何の役も作らない。
        /// </remarks>
        public static (int pair, int straight, int flush) Count(IHand hand)
        {
            var nums = hand.Cards.Select(x => x.Number).ToArray();
            var suits = hand.Cards.Select(x => (int)x.Suit).ToArray();

            var pair = nums.GroupBy(x => x).Max(g => g.Count());

            // ストレート判定のために、ソートして、連番で番号を減らす
            Array.Sort(nums);
            for (int i = 0; i < 5; i++) nums[i] -= i;
            var straight = nums.GroupBy(x => x).Max(g => g.Count());

            var flush = suits.GroupBy(x => x).Max(g => g.Count());

            return (pair, straight, flush);
        }

        /// <summary>
        /// プレイヤーの手札の強さを評価。
        /// <seealso cref="Score(IHand, Ability)"/>
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int Score(Player player) => Score(player.Hand, player.Ability);

        /// <summary>
        /// 手札の強さを評価。
        /// </summary>
        /// <param name="hand">手札。</param>
        /// <param name="ability">能力値。</param>
        /// <returns>威力的なもの。</returns>
        /// <remarks>
        /// 適当にそれっぽく。
        ///
        /// ・数字の価値
        /// 2が一番弱くて、エース(1)は14扱いで評価。
        /// 一律下駄をはかせて、2の時の威力が20で、エースが32。
        ///
        /// ・柄と数字の集計
        /// で、これを各柄ごとに集計して、<see cref="Ability.Spades"/>とかと掛ける。
        /// ジョーカーがあったら、全柄分の能力値の和 × 20 を計算。
        /// これらを全部合計する。
        ///
        /// ・役によるボーナス
        /// 役によるボーナスは掛け算。
        /// pair, straight, flush それぞれ、Pow(枚数, 能力値 / 100) を掛ける。
        ///
        /// ・切り上げ
        /// 結果は整数になるよう、小数点以下切り上げ。
        /// </remarks>
        public static int Score(IHand hand, Ability ability)
        {
            var score = 0.0;

            var suits = hand.Cards.GroupBy(x => x.Suit);

            foreach (var s in suits)
            {
                if (s.Key == Suit.Joker)
                {
                    score += 20 * (ability.Spades + ability.Hearts + ability.Diamonds + ability.Clubs);
                    continue;
                }

                var subScore = 0;

                foreach (var x in s)
                {
                    var n = x.Number;
                    var value = n == 1 ? 32 : n + 18;
                    subScore += value;
                }

                var a =
                    s.Key == Suit.Spades ? ability.Spades :
                    s.Key == Suit.Hearts ? ability.Hearts :
                    s.Key == Suit.Diamonds ? ability.Diamonds :
                    s.Key == Suit.Clubs ? ability.Clubs :
                    0;
                score += subScore * a;
            }

            var (pair, straight, flush) = Count(hand);

            score *= Math.Pow(pair, ability.Pair / 100.0);
            score *= Math.Pow(straight, ability.Straight / 100.0);
            score *= Math.Pow(flush, ability.Flush / 100.0);

            return (int)Math.Ceiling(score);
        }
    }
}
