using System.Windows;

namespace ModalDialogSample
{
    public partial class Window1 : Window, IResultDialog<string>
    {
        public Window1()
        {
            InitializeComponent();
        }

        public string Result => input.Text;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
