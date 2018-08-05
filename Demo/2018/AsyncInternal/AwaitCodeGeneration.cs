using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncInternal
{
    class AwaitCodeGeneration
    {
        // Step5: await のところに ContinueWith を仕込む
        public static async Task<IEnumerable<string>> GetContents()
        {
            var state = 0;
            List<string> contents = null;
            IEnumerator<string> e = null;

            Task<IEnumerable<string>> tIndexes = null;
            Task<IEnumerable<string>> tSelectedIndexes = null;
            Task<string> tContent = null;

            void a()
            {
                state = 1;
                tIndexes = GetIndex();
                tIndexes.ContinueWith(_ => a());
                return;
                Case1:
                var indexes = tIndexes.Result;

                state = 2;
                tSelectedIndexes = SelectIndex(indexes);
                tSelectedIndexes.ContinueWith(_ => a());
                return;
                Case2:
                var selectedIndexes = tSelectedIndexes.Result;

                contents = new List<string>();
                e = selectedIndexes.GetEnumerator();

                goto EndLoop;
                BeginLoop:
                state = 3;
                tContent = GetContent(e.Current);
                tContent.ContinueWith(_ => a());
                return;
                Case3:
                var content = tContent.Result;
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
