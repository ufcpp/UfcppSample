using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CardBattle.Views
{
    /// <summary>
    /// コンティニュー催促に対して「はい」「いいえ」を答えるUI。
    /// </summary>
    public partial class ContinuePromptControl : UserControl
    {
        public ContinuePromptControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 決定ボタンを押すまで待機。
        /// </summary>
        /// <returns>「はい」ならtrue、「いいえ」ならfalse。</returns>
        public async Task<bool> SelectAsync(CancellationToken ct)
        {
            var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);

            var y = yes.FirstClick(cts.Token);
            var n = no.FirstClick(cts.Token);

            var res = await Task.WhenAny(y, n);

            cts.Cancel();

            return res == y;
        }
    }
}
