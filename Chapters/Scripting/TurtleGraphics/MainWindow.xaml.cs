using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Threading;
using System.Windows.Controls;

namespace TurtleGraphics
{
    public partial class MainWindow
    {
        public ViewModels.TurtleGraphicsViewModel ViewModel { get; } = new ViewModels.TurtleGraphicsViewModel();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = ViewModel;

            var ct = new CancellationTokenSource();
            var t = ViewModel.Start(ct.Token);

            //Loaded += (_1, _2) => MainPage_Loaded(ViewModel.Commander);

            Unloaded += async (_1, _2) =>
            {
                ct.Cancel();
                await t;
            };
        }

        private void MainPage_Loaded(ViewModels.Commander c)
        {
            c.speed(50);
            c.walk(50);
            c.clear();

            for (int i = 0; i < 5; i++)
            {
                c.turn(144);
                c.walk(100);
            }
        }

        private ScriptState<object> _state;

        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            var s = tb.Text;

            if (string.IsNullOrWhiteSpace(s))
                return;

            if (s[s.Length - 1] == '\n')
            {
                try
                {
                    if (_state == null)
                        _state = await CSharpScript.RunAsync(s, globals: ViewModel.Commander);
                    else
                        _state = await _state.ContinueWithAsync(s);
                }
                catch { }

                tb.Text = "";
            }
        }
    }
}
