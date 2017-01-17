using System.Windows;

namespace ModalDialogSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var r = await DialogExtensions.ShowDialogAsync<Window1, string>();
            result.Text = r;
        }
    }
}
