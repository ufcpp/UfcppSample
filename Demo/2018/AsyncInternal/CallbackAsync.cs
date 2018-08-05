using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncInternal
{
    /// <summary>
    /// <see cref="Synchronous"/> と同じことを、コールバック型の非同期でやると…
    /// </summary>
    class CallbackAsync
    {
        public static void GetContents(Action<IEnumerable<string>> callback)
        {
            // インデックスをどこかから取って来て
            GetIndex(indexes =>
            {
                // その中の一部分を選んで
                SelectIndex(indexes, selectedIndexes =>
                {
                    // 選んだものの中身を取得
                    var contents = new List<string>();

                    var e = selectedIndexes.GetEnumerator();
                    Action<string> getContentCallback = null;

                    void next()
                    {
                        if (e.MoveNext()) GetContent(e.Current, getContentCallback);
                        else callback(contents);
                    }

                    getContentCallback  = content =>
                    {
                        contents.Add(content);
                        next();
                    };

                    next();
                });
            });
        }

        static void GetIndex(Action<IEnumerable<string>> callback)
        {
            Task.Delay(500).ContinueWith(_ => callback(Data.Indexes));
        }

        static void SelectIndex(IEnumerable<string> list, Action<IEnumerable<string>> callback)
        {
            Task.Delay(500).ContinueWith(_ => callback(Data.RandomSelect(list)));
        }

        static void GetContent(string index, Action<string> callback)
        {
            Task.Delay(500).ContinueWith(_ => callback(Data.GetContent(index)));
        }
    }
}
