using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncInternal
{
    class AwaitCodeGeneration
    {
        // Step1: foreach とかは、if, goto に展開
        public static async Task<IEnumerable<string>> GetContents()
        {
            var indexes = await GetIndex();

            var selectedIndexes = await SelectIndex(indexes);

            var contents = new List<string>();
            var e = selectedIndexes.GetEnumerator();

            goto EndLoop;
            BeginLoop:
            var content = await GetContent(e.Current);
            contents.Add(content);
            EndLoop:
            if (e.MoveNext()) goto BeginLoop;

            e.Dispose();

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
