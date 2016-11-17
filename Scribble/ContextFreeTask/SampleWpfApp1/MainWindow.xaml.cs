using System;
using System.Threading.Tasks;
using System.Windows;
using ContextFreeTasks;

namespace SampleWpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }
        private int _mainThreadId;

        // 単体テストプロジェクト作るべきなんだけど、単体テストプロジェクト向けのメッセージポンプ作るのがめんどくさかったのでWPF上で実行
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Output.Text = "running...";
                await A1(10);
                await B1(10);
                Output.Text = "done";
            }
            catch(Exception ex)
            {
                Output.Text = "error: " + ex;
            }
        }

        private void OnMainThread()
        {
            var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            if (_mainThreadId != threadId) throw new InvalidOperationException();
        }

        private void OnPoolThread()
        {
            var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            if (_mainThreadId == threadId) throw new InvalidOperationException();
        }

        private async Task<string> A1(int n)
        {
            OnMainThread();
            var s = await A2(n);
            OnPoolThread();
            await Task.Delay(100);
            OnPoolThread();
            await A3();
            OnPoolThread();
            return s;
        }

        private async ContextFreeTask<string> A2(int n)
        {
            await Task.Delay(100);
            OnPoolThread();
            await Task.Delay(100);
            OnPoolThread();
            await A3();
            OnPoolThread();
            return n.ToString();
        }

        private async ContextFreeTask A3()
        {
            await Task.Delay(100);
            OnPoolThread();
        }

        private async Task<string> B1(int n)
        {
            OnMainThread();
            var s = await B2(n);
            OnMainThread();
            await Task.Delay(100);
            OnMainThread();
            await B3();
            OnMainThread();
            return s;
        }

        private async Task<string> B2(int n)
        {
            await Task.Delay(100);
            OnMainThread();
            await Task.Delay(100);
            OnMainThread();
            await B3();
            OnMainThread();
            return n.ToString();
        }

        private async Task B3()
        {
            await Task.Delay(100);
            OnMainThread();
        }
    }
}
