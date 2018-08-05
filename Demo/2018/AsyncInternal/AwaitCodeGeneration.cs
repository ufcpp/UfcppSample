using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncInternal
{
    class AwaitCodeGeneration
    {
        // 最適化: 未完了 Task だけを ContinueWith、終わったあと null 上書き
        public static Task<IEnumerable<string>> GetContents()
        {
            var state = 0;
            List<string> contents = null;
            IEnumerator<string> e = null;

            Task<IEnumerable<string>> tIndexes = null;
            Task<IEnumerable<string>> tSelectedIndexes = null;
            Task<string> tContent = null;

            var tcs = new TaskCompletionSource<IEnumerable<string>>();

            void a()
            {
                switch (state)
                {
                    case 1: goto Case1;
                    case 2: goto Case2;
                    case 3: goto Case3;
                }
                state = 1;
                tIndexes = GetIndex();
                if (!tIndexes.IsCompleted)
                {
                    tIndexes.ContinueWith(_ => a());
                    return;
                }
                Case1:
                var indexes = tIndexes.Result;
                tIndexes = null;

                state = 2;
                tSelectedIndexes = SelectIndex(indexes);
                if (!tSelectedIndexes.IsCompleted)
                {
                    tSelectedIndexes.ContinueWith(_ => a());
                    return;
                }
                Case2:
                var selectedIndexes = tSelectedIndexes.Result;
                tSelectedIndexes = null;

                contents = new List<string>();
                e = selectedIndexes.GetEnumerator();

                goto EndLoop;
                BeginLoop:
                state = 3;
                tContent = GetContent(e.Current);
                if (!tContent.IsCompleted)
                {
                    tContent.ContinueWith(_ => a());
                    return;
                }
                Case3:
                var content = tContent.Result;
                tContent = null;

                contents.Add(content);
                EndLoop:
                if (e.MoveNext()) goto BeginLoop;

                e.Dispose();

                tcs.SetResult(contents);
            }

            a();

            return tcs.Task;
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
