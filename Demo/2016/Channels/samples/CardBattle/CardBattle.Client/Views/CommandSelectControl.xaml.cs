using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CardBattle.Views
{
    /// <summary>
    /// コマンド選択(捨てる手札の選択)画面。
    /// </summary>
    public partial class CommandSelectControl : UserControl
    {
        public CommandSelectControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 決定ボタンを押すまで待機。
        /// </summary>
        /// <returns>決定時に選択されていたインデックス。</returns>
        public async Task<int[]> SelectAsync(CancellationToken ct)
        {
            ok.IsEnabled = true;
            ok.Visibility = System.Windows.Visibility.Visible;

            await ok.FirstClick(ct);

            var items = list.ItemsSource.Cast<object>().ToArray();
            return Enumerable.Range(0, items.Length)
                .Where(i => list.SelectedItems.Contains(items[i]))
                .ToArray();
        }

        /// <summary>
        /// AI用。
        /// 手札を0.5秒表示 → AI選択したカードを選択して0.5秒表示。
        /// </summary>
        /// <param name="selectedIndexes">AIが選択したインデックス。</param>
        /// <param name="ct"></param>
        public async Task ShowAsync(int[] selectedIndexes, CancellationToken ct)
        {
            ok.IsEnabled = false;
            ok.Visibility = System.Windows.Visibility.Collapsed;

            await Task.Delay(500);

            var items = ((System.Collections.IList)list.ItemsSource);
            list.SelectedItems.Clear();
            foreach (var i in selectedIndexes)
            {
                list.SelectedItems.Add(items[i]);
            }

            await Task.Delay(500);
        }
    }
}
