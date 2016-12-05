using System;
using System.Collections.Generic;
using System.Linq;

namespace CardBattle.Models.GameRules
{
    /// <summary>
    /// 場全体。
    /// </summary>
    /// <remarks>
    /// 山札の残り枚数、捨て札の枚数くらいは公開情報にしてもいい気がしつつ。
    /// 言実装はその辺り手抜きで、手札のみ public。
    /// </remarks>
    public partial class PlayArea
    {
        /// <summary>
        /// 山札。
        /// 固定長で最大枚数(1デッキ = 53枚)分取っておく。
        /// </summary>
        private Card[] _pile;

        /// <summary>
        /// 山札のうち、何枚目まで配ったか。
        /// 0ベース(<see cref="_pile"/>のインデックス)。
        /// </summary>
        private int _pileTop;

        /// <summary>
        /// 捨て札。
        /// 固定長で最大枚数(1デッキ = 53枚)分取っておく。
        /// </summary>
        private Card[] _discards;

        /// <summary>
        /// 捨て札が何枚積もったか。
        /// 0ベース(<see cref="_discards"/>のインデックス)。
        /// </summary>
        private int _discardsBottom;

        private Random _random;

        private Hand[] _players;

        /// <summary>
        /// </summary>
        /// <param name="randomSeed">乱数シード。</param>
        /// <param name="numPlayers">プレイヤー人数。</param>
        public PlayArea(int randomSeed, int numPlayers = 4)
        {
            _random = new Random(randomSeed);

            _pileTop = 0;
            _pile = Card.AllCards.ToArray();
            Card.Shuffle(_pile, _pile.Length, _random);

            _discardsBottom = 0;
            _discards = new Card[_pile.Length];

            //todo: 上限人数チェックしないと、53枚で収まらなくなる。
            _players = new Hand[numPlayers];
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i] = new Hand();
            }

            // 最初に5枚ずつ配る
            for (int i = 0; i < CardsInHand; i++)
            {
                foreach (var p in _players)
                {
                    p[i] = GetNext();
                }
            }
        }

        /// <summary>
        /// 各プレイヤーの手札。
        /// </summary>
        public IReadOnlyList<IHand> Players => _players;

        /// <summary>
        /// 山札から1枚取得。
        /// 全部なくなったら<see cref="Refresh"/>。
        /// </summary>
        /// <returns></returns>
        private Card GetNext()
        {
            var card = _pile[_pileTop];
            _pileTop++;

            if (_pileTop == _pile.Length)
            {
                Refresh();
            }

            return card;
        }

        /// <summary>
        /// 捨て札をシャッフルして山札に戻す。
        /// </summary>
        private void Refresh()
        {
            Card.Shuffle(_discards, _discardsBottom, _random);
            _pileTop = _pile.Length - _discardsBottom;

            for (int i = _pile.Length - 1, j = _discardsBottom - 1; j >= 0; i--, j--)
            {
                _pile[i] = _discards[j];
            }

            _discardsBottom = 0;
        }

        /// <summary>
        /// カードを1枚捨てる
        /// </summary>
        /// <param name="c"></param>
        private void Discard(Card c)
        {
            _discards[_discardsBottom] = c;
            _discardsBottom++;
        }

        /// <summary>
        /// あるプレイヤーのカードを1枚捨てて引き直す。
        /// </summary>
        /// <param name="playerIndex">プレイヤーのインデックス。</param>
        /// <param name="cardIndex">カードのインデックス</param>
        public void Redraw(int playerIndex, int cardIndex)
        {
            var p = _players[playerIndex];

            Redraw(p, cardIndex);
        }

        /// <summary>
        /// あるプレイヤーのカードを全部捨てて引き直す。
        /// </summary>
        /// <param name="playerIndex"></param>
        public void RedrawAll(int playerIndex)
        {
            var p = _players[playerIndex];

            for (int i = 0; i < CardsInHand; i++)
            {
                Redraw(p, i);
            }
        }

        private void Redraw(Hand p, int cardIndex)
        {
            Discard(p[cardIndex]);
            p[cardIndex] = GetNext();
        }

        /// <summary>
        /// 数枚まとめて引き直し。
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="cardIndexes"></param>
        public void Redraw(int playerIndex, int[] cardIndexes)
        {
            var p = _players[playerIndex];

            foreach (var i in cardIndexes)
            {
                Redraw(p, i);
            }
        }
    }
}
