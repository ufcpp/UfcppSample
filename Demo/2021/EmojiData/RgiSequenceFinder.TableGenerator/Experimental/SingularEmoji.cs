using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RgiSequenceFinder.TableGenerator.Experimental
{
    /// <summary>
    /// <see cref="GroupedEmojis.Singlulars"/>, <see cref="GroupedEmojis.Plurals"/> の動作検証。
    /// </summary>
    class SingularEmoji
    {
        public static void CheckCount()
        {
            var emojis = GroupedEmojis.Create();

            var count = 0;
            void w(int c)
            {
                count += c;
                Console.WriteLine(c);
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Console.Write($"{j}, {i}: ");
                    w(emojis.Singlulars[j, i]?.Count ?? 0);
                }
            }

            // 前提があってれば以下の2つの数字そろうはず。(仕様変更でそろわなくなってる)
            Console.WriteLine((count, emojis.Others.Count));
        }

        public static void CollisionCount()
        {
            var emojis = GroupedEmojis.Create();

            CollisionCount(emojis.Singlulars[0, 0]!.Select(t => t.c).ToArray());
            CollisionCount(emojis.Singlulars[1, 0]!.Select(t => t.c).ToArray());
            CollisionCount(emojis.Singlulars[0, 1]!.Select(t => t.c).ToArray());
            CollisionCount(emojis.Singlulars[1, 1]!.Select(t => t.c).ToArray());
            CollisionCount(emojis.Singlulars[0, 2]!.Select(t => t.c).ToArray());
            CollisionCount(emojis.Singlulars[1, 2]!.Select(t => t.c).ToArray());
            CollisionCount(emojis.Singlulars[0, 3]!.Select(t => t.c).ToArray());
        }

        private static void CollisionCount(IEnumerable<char> list)
        {
            var count = list.Count();
            var bits = (int)Math.Round(Math.Log2(count));

            // ビット数削るほど被り率上がるので、256/512 を境にビット数増やしてる。
            bits = bits <= 7 ? bits + 2 : bits + 1;
            var capacity = 1 << bits;
            var mask = capacity - 1;

            // 元々、下位桁に被りがあんまりないので単純に mod をハッシュ値にしても大して被らないみたい。
            // これでハッシュ値衝突率1割ないくらいになる。
            var groups = list.Select(x => x & mask).GroupBy(x => x);
            var hashDistinct = groups.Count();
            var max = groups.Max(g => g.Count());
            var ave = groups.Average(g => (double)g.Count());

            Console.WriteLine($"{bits,2} ({capacity,4}) {hashDistinct,3}/{count,3} max: {max}, ave: {ave}");
        }
    }
}
