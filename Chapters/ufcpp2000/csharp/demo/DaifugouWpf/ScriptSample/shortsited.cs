using System;
using System.Collections.Generic;
using System.Linq;
using CardGame;
using Daifugou;

/// <summary>
/// 出せる限り必ずカードを出すおバカAI。
/// </summary>
public class SampleShortsitedAi : IArtificialIntelligence
{
    #region IArtificialIntelligence メンバ

    public IEnumerable<Card> Play(IEnumerable<Card> hand, IEnumerable<Card> table, int rank, Suit suit, Mode mode, bool revolution, History history)
    {
        if (revolution && rank != Card.RankOfJoker)
        {
            rank = 14 - rank;
        }

        if (hand == null || hand.Count() == 0)
            return null;

        int count = table.Count();

        // 初手はとりあえず一番弱いのを出しとく。
        if (mode.Match(Mode.First))
        {
            var min = hand.Min(x => Game.Rank(x, revolution));
            return hand.Where(x => Game.Rank(x, revolution) == min);
        }

        if (mode.Match(Mode.Normal))
        {
            if(mode.Match(Mode.SuitBound))
            {
                return hand.MinCard(x => Game.Rank(x, revolution) > rank && x.Suit == suit, revolution);
            }
            else
            {
                return hand.MinCard(x => Game.Rank(x, revolution) > rank, revolution);
            }
        }

        if(mode.Match(Mode.Multiple))
        {
            for (int i = rank + 1; i <= 13; i++)
            {
                // 出せる
                var c = hand.Where(x => Game.Rank(x, revolution) == i);
                if (c.Count() >= count)
                    return c.Take(count);

                // Joker含めれば出せる
                if (c.Count() + 1 == count && hand.FirstOrDefault(x => x.Suit == Suit.Joker) != null)
                    return c.Concat(hand.Where(x => x.Suit == Suit.Joker).Take(count));
            }
        }

        if (mode.Match(Mode.SequenceBound))
            return null; //todo また未対応

        if (mode.Match(Mode.Sequence))
        {
            if (mode.Match(Mode.SuitBound))
                return hand.Sequence(rank, revolution, count, suit);
            else
                return hand.Sequence(rank, revolution, count);
        }

        return null;
    }

    #endregion
}

static class CardsExtesions
{
    /// <summary>
    /// cardsの中から、条件predを満たす最少ランクのカードを返す。
    /// </summary>
    /// <param name="cards">対象となる札。</param>
    /// <param name="pred">条件。</param>
    /// <returns>条件を満たす最少ランクのカード。</returns>
    public static IEnumerable<Card> MinCard(this IEnumerable<Card> cards, Func<Card, bool> pred, bool revolution)
    {
        cards = cards.Where(pred);

        if (cards == null || cards.Count() == 0)
            return null;

        int min = int.MaxValue;
        Card minCard = cards.First();

        foreach (var c in cards)
        {
            var rank = Game.Rank(c, revolution);

            if (min > rank)
            {
                min = rank;
                minCard = c;
            }
        }

        return new[] { minCard };
    }

    public static IEnumerable<Card> Sequence(this IEnumerable<Card> cards, int rank, bool revolution, int count, Suit suit)
    {
        int jokers = cards.Count(x => x.Suit == Suit.Joker);

        return cards.Sequence(rank, revolution, count, suit, jokers);
    }

    public static IEnumerable<Card> Sequence(this IEnumerable<Card> cards, int rank, bool revolution, int count, Suit suit, int jokers)
    {
        var c = cards.Where(x => Game.Rank(x, revolution) > rank && (x.Suit == suit));

        if (c.Count() < count)
            return null;

        var cc = c.OrderBy(x => Game.Rank(x, revolution)).ToArray();

        for (int i = 0; i <= cc.Length - count; i++)
        {
            var ccc = cc.Skip(i).Take(count);

            if (Game.IsSequence(ccc) != 0)
            {
                return ccc;
            }
        }

        if (jokers == 0)
            return null;

        for (int i = 0; i <= cc.Length - count; i++)
        {
            var ccc = cc.Skip(i).Take(count - 1).Concat(new[] { new Card(Suit.Joker, 1) });

            if (Game.IsSequence(ccc) != 0)
            {
                return ccc;
            }
        }

        return null;
    }

    public static IEnumerable<Card> Sequence(this IEnumerable<Card> cards, int rank, bool revolution, int count)
    {
        var ss = new[] { Suit.Club, Suit.Spade, Suit.Heart, Suit.Diamond };
        int jokers = cards.Count(x => x.Suit == Suit.Joker);

        foreach (var s in ss)
        {
            var c = cards.Sequence(rank, revolution, count, s, jokers);
            if (c != null)
                return c;
        }

        return null;
    }
}
