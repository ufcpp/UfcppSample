using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncInternal
{
    class AwaitCodeGeneration
    {
        // Step3: state 導入。await のところに state 更新と goto 用ラベルを追加
        public static async Task<IEnumerable<string>> GetContents()
        {
            var state = 0;

            void a()
            {
                state = 1;
                var indexes = await GetIndex();
                Case1:

                state = 1;
                var selectedIndexes = await SelectIndex(indexes);
                Case2:

                var contents = new List<string>();
                var e = selectedIndexes.GetEnumerator();

                goto EndLoop;
                BeginLoop:
                state = 1;
                var content = await GetContent(e.Current);
                Case3:
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
