using System;
using System.Collections;
using System.Collections.Generic;

namespace CardBattle.Models.GameRules
{
    /// <summary>
    /// プレイヤーの手札。
    /// </summary>
    /// <remarks>
    /// 具体的な操作は<see cref="PlayArea"/>越しでしかさせないためにインターフェイス。
    /// </remarks>
    public interface IHand
    {
        /// <summary>
        /// 手札にあるカード。
        /// </summary>
        IReadOnlyList<Card> Cards { get; }

        /// <summary>
        /// ある時点での手札のスナップショットを取っておくためにクローンが必要。
        /// </summary>
        /// <returns>クローン。</returns>
        IHand Clone();
    }

    public partial class PlayArea
    {
        private const int CardsInHand = 5;

        private class Hand : IHand, IReadOnlyList<Card>
        {
            // GC 除けに固定長実装
            public Card c0;
            public Card c1;
            public Card c2;
            public Card c3;
            public Card c4;

            public IReadOnlyList<Card> Cards => this;

            public ref Card this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0: return ref c0;
                        case 1: return ref c1;
                        case 2: return ref c2;
                        case 3: return ref c3;
                        case 4: return ref c4;
                        default:
                            throw new IndexOutOfRangeException();
                    }
                }
            }

            Card IReadOnlyList<Card>.this[int index] => this[index];

            int IReadOnlyCollection<Card>.Count => 5;

            IEnumerator<Card> IEnumerable<Card>.GetEnumerator()
            {
                yield return c0;
                yield return c1;
                yield return c2;
                yield return c3;
                yield return c4;
            }

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Card>)this).GetEnumerator();

            IHand IHand.Clone()
            {
                return new Hand
                {
                    c0 = c0,
                    c1 = c1,
                    c2 = c2,
                    c3 = c3,
                    c4 = c4,
                };
            }
        }
    }
}
