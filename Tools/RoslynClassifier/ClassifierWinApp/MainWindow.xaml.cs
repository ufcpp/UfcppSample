using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ClassifierWinApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static MainWindow()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorWebView();
        serviceCollection.AddSingleton<Models.ClassfierWorkspace>();
        Services = serviceCollection.BuildServiceProvider();
    }

    public MainWindow()
    {
        InitializeComponent();
    }

    public static ServiceProvider Services { get; }
}
