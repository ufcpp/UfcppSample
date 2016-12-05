using System;
using System.Collections.Generic;
using System.Linq;

namespace CardBattle.Models.GameRules
{
    /// <summary>
    /// カードの柄。
    /// </summary>
    public enum Suit : byte
    {
        Spades = 1,
        Hearts = 2,
        Diamonds = 4,
        Clubs = 8,
        Joker = 15,
    }

    /// <summary>
    /// 要するにトランプ。
    /// </summary>
    public struct Card
    {
        private byte _value;

        /// <summary>
        /// 数字。
        /// </summary>
        public int Number => (_value >> 4) & 0xf;

        /// <summary>
        /// 柄。
        /// </summary>
        public Suit Suit => (Suit)(_value  & 0xf);

        public Card(int number, Suit suit)
        {
            _value = (byte)((number << 4) | (int)suit);
        }

        public override string ToString() => $"{ToString(Suit)}{Number,2}";

        private static string ToString(Suit s) =>
            s == Suit.Spades ? "♠" :
            s == Suit.Hearts ? "♡" :
            s == Suit.Diamonds ? "♢" :
            s == Suit.Clubs ? "♣" :
            "";

        /// <summary>
        /// ジョーカー。
        /// </summary>
        public static Card Joker { get; } = new Card { _value = 0xff };

        /// <summary>
        /// 柄4つ並べたもの。
        /// </summary>
        public static IEnumerable<Suit> Suits = new[] { Suit.Spades, Suit.Hearts, Suit.Diamonds, Suit.Clubs };

        /// <summary>
        /// ジョーカー含む53枚を列挙。
        /// </summary>
        public static IEnumerable<Card> AllCards
        {
            get
            {
                for (int num = 1; num <= 13; num++)
                {
                    foreach (var suit in Suits)
                    {
                        yield return new Card(num, suit);
                    }
                }
                yield return Joker;
            }
        }

        /// <summary>
        /// <paramref name="cards"/>の先頭から<paramref name="length"/>枚目のカードまでをシャッフル。
        /// </summary>
        /// <param name="cards">シャッフル対象。</param>
        /// <param name="length">先頭からこの枚数までだけをシャッフル。</param>
        /// <param name="random">乱数</param>
        /// <param name="shuffleCount">シャッフル回数。</param>
        public static void Shuffle(Card[] cards, int length, Random random, int shuffleCount = 100)
        {
            void swap(ref Card x, ref Card y)
            {
                var t = x;
                x = y;
                y = t;
            }

            for (int n = 0; n < shuffleCount; n++)
            {
                var i = random.Next(length);
                var j = random.Next(length);
                swap(ref cards[i], ref cards[j]);
            }
        }
    }
}
