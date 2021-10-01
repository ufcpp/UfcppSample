using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ClassifierWinApp;

public partial class MainWindow : Window
{
    static MainWindow()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorWebView();
        serviceCollection.AddSingleton<Models.ClassfierWorkspace>();
        Services = serviceCollection.BuildServiceProvider();
    }

    public static ServiceProvider Services { get; }

    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        var settings = Settings.Load();

        if (settings is not null)
        {
            var workspace = Services.GetService<Models.ClassfierWorkspace>()!;
            workspace.CsprojPath = settings.CsprojPath;
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        var workspace = Services.GetService<Models.ClassfierWorkspace>()!;
        Settings.Save(new(workspace.CsprojPath));
        base.OnClosed(e);
    }
}
