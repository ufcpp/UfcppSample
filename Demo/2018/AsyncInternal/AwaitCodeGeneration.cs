using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AsyncInternal
{
    class AwaitCodeGeneration
    {
        // パターン化: AsyncMethodBuilder 導入(Task 型以外の戻り値を返せるように)
        public static Task<IEnumerable<string>> GetContents()
        {
            var state = 0;
            List<string> contents = null;
            IEnumerator<string> e = null;

            TaskAwaiter<IEnumerable<string>> tIndexes = default;
            TaskAwaiter<IEnumerable<string>> tSelectedIndexes = default;
            TaskAwaiter<string> tContent = default;

            AsyncTaskMethodBuilder<IEnumerable<string>> builder;

            void a()
            {
                switch (state)
                {
                    case 1: goto Case1;
                    case 2: goto Case2;
                    case 3: goto Case3;
                }
                state = 1;
                tIndexes = GetIndex().GetAwaiter();
                if (!tIndexes.IsCompleted)
                {
                    tIndexes.OnCompleted(a); // 本当は builder.AwaitOnCompleted
                    return;
                }
                Case1:
                var indexes = tIndexes.GetResult();
                tIndexes = default;

                state = 2;
                tSelectedIndexes = SelectIndex(indexes).GetAwaiter();
                if (!tSelectedIndexes.IsCompleted)
                {
                    tSelectedIndexes.OnCompleted(a); // 本当は builder.AwaitOnCompleted
                    return;
                }
                Case2:
                var selectedIndexes = tSelectedIndexes.GetResult();
                tSelectedIndexes = default;

                contents = new List<string>();
                e = selectedIndexes.GetEnumerator();

                goto EndLoop;
                BeginLoop:
                state = 3;
                tContent = GetContent(e.Current).GetAwaiter();
                if (!tContent.IsCompleted)
                {
                    tContent.OnCompleted(a); // 本当は builder.AwaitOnCompleted
                    return;
                }
                Case3:
                var content = tContent.GetResult();
                tContent = default;

                contents.Add(content);
                EndLoop:
                if (e.MoveNext()) goto BeginLoop;

                e.Dispose();

                builder.SetResult(contents);
            }

            a();

            return builder.Task;
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
