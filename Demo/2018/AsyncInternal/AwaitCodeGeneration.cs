using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncInternal
{
    class AwaitCodeGeneration
    {
        public static async Task<IEnumerable<string>> GetContents()
        {
            // インデックスをどこかから取って来て
            var indexes = await GetIndex();

            // その中の一部分を選んで
            var selectedIndexes = await SelectIndex(indexes);

            // 選んだものの中身を取得
            var contents = new List<string>();
            foreach (var index in selectedIndexes)
            {
                var content = await GetContent(index);
                contents.Add(content);
            }

            return contents;
        }

        static async Task<IEnumerable<string>> GetIndex()
        {
            await Task.Delay(500);
            return Data.Indexes;
        }

        static async Task<IEnumerable<string>> SelectIndex(IEnumerable<string> list)
        {
            await Task.Delay(500);
            return Data.RandomSelect(list);
        }

        static async Task<string> GetContent(string index)
        {
            await Task.Delay(500);
            return Data.GetContent(index);
        }
    }
}
