using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AsyncInternal
{
    class AwaitCodeGeneration
    {
        // 実際のコード生成に近づける: ラムダ式で代用するのをやめて、型を1個作る
        public static Task<IEnumerable<string>> GetContents()
        {
            GetContentsStateMachine stateMachine = default;
            stateMachine.builder = AsyncTaskMethodBuilder<IEnumerable<string>>.Create();
            stateMachine.builder.Start(ref stateMachine);
            return stateMachine.builder.Task;
        }

        struct GetContentsStateMachine : IAsyncStateMachine
        {
            int state;
            List<string> contents;
            IEnumerator<string> e;

            TaskAwaiter<IEnumerable<string>> tIndexes;
            TaskAwaiter<IEnumerable<string>> tSelectedIndexes;
            TaskAwaiter<string> tContent;

            public AsyncTaskMethodBuilder<IEnumerable<string>> builder;

            public void MoveNext()
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
                    builder.AwaitOnCompleted(ref tIndexes, ref this);
                    return;
                }
                Case1:
                var indexes = tIndexes.GetResult();
                tIndexes = default;

                state = 2;
                tSelectedIndexes = SelectIndex(indexes).GetAwaiter();
                if (!tSelectedIndexes.IsCompleted)
                {
                    builder.AwaitOnCompleted(ref tSelectedIndexes, ref this);
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
                    builder.AwaitOnCompleted(ref tContent, ref this);
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

            public void SetStateMachine(IAsyncStateMachine stateMachine) => builder.SetStateMachine(stateMachine);
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
