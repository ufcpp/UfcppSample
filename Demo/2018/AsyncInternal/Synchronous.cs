using System.Collections.Generic;

namespace AsyncInternal
{
    /// <summary>
    /// とりあえず同期の時の書き方。
    /// </summary>
    class Synchronous
    {
        public static IEnumerable<string> GetContents()
        {
            // インデックスをどこかから取って来て
            var indexes = GetIndex();

            // その中の一部分を選んで
            var selectedIndexes = SelectIndex(indexes);

            // 選んだものの中身を取得
            var contents = new List<string>();
            foreach (var index in selectedIndexes)
            {
                var content = GetContent(index);
                contents.Add(content);
            }

            return contents;
        }

        /// <summary>
        /// インデックス取得。
        /// </summary>
        /// <remarks>
        /// デモ用ということで適当な Delay で代用。
        /// 実際には REST API とかで取ってくるとかそういうのを想定。
        /// </remarks>
        static IEnumerable<string> GetIndex() => Data.Indexes;

        /// <summary>
        /// インデックスから、一部分だけを選択。
        /// </summary>
        /// <remarks>
        /// デモ用ということで、乱数で2分の1の確率で間引く。
        /// 実際にはダイアログなりなんなりを表示して、ユーザーに選んでもらうとかを想定。
        /// </remarks>
        static IEnumerable<string> SelectIndex(IEnumerable<string> list) => Data.RandomSelect(list);

        /// <summary>
        /// コンテンツを取得。
        /// </summary>
        /// <remarks>
        /// これも Delay で代用。
        /// 実際にはこれも REST で取ってくるとか、ファイルから読むとかを想定。
        /// </remarks>
        static string GetContent(string index) => Data.GetContent(index);
    }
}
