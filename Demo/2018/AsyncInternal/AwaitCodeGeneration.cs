using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncInternal
{
    class AwaitCodeGeneration
    {
        // Step2: メソッド全体を包む匿名なクラスを生成(ここではラムダ式で代用)
        public static async Task<IEnumerable<string>> GetContents()
        {
            void a()
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

            a();
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
