using System.Windows;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new
            {
                A = "abc",
                B = "cde",
                C = new[]
                {
                    1,2,3,4,5
                },
                α = new Sample.Alpha { Id = 1, Name = "アルファ" },
                β = new Sample.Beta { Id = 2, Name = "ベータ" },
                γ = new Sample.Gamma { Id = 3, Name = "ガンマ" },
            };
        }
    }

    namespace Sample
    {
        public class Alpha
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Beta
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Gamma
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
