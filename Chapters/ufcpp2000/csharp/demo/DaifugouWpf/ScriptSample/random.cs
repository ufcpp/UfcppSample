using System;
using System.Collections.Generic;
using System.Linq;
using CardGame;
using Daifugou;

/// <summary>
/// ランダムにカードを選んで出すおバカAI。
/// </summary>
public class SampleRandomAi : IArtificialIntelligence
{
    Random rnd = new Random();

    #region IArtificialIntelligence メンバ

    public IEnumerable<Card> Play(IEnumerable<Card> hand, IEnumerable<Card> table, int rank, Suit suit, Mode mode, bool revolution, History history)
    {
        if (hand == null || hand.Count() == 0)
            return null;

        var count = table.Count();

        if (count == 0)
            count = 1;

        if (hand.Count() < count)
            return null;

        for (int i = 0; i < 50; i++)
        {
            var result = hand.OrderBy(x => rnd.Next()).Take(count);
            if(Game.CanPlay(result, table, rank, suit, mode, revolution))
                return result;
        }

        return null;
    }

    #endregion
}
