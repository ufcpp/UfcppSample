using System;

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

            for (int i = 0; i < 4; i++)
            {
                Console.Write($"{i}: ");
                w(emojis.Plurals[i]?.Count ?? 0);
            }

            // 前提があってれば以下の2つの数字そろうはず。
            Console.WriteLine((count, emojis.Others.Count));
        }
    }
}
