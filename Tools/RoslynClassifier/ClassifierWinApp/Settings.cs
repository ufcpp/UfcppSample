using System.IO;
using System.Text.Json;

namespace ClassifierWinApp;

public record Settings(string? CsprojPath)
{
    private const string SettingFileName = "settings.json";
    private static string GetAppPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ClassifierWinApp");

    public static Settings? Load()
    {
        var folder = GetAppPath();
        var path = Path.Combine(folder, SettingFileName);

        if (!File.Exists(path)) return null;

        using var f = File.OpenRead(path);
        return JsonSerializer.Deserialize<Settings>(f);
    }

    public static void Save(Settings settings)
    {
        var folder = GetAppPath();
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        var path = Path.Combine(folder, SettingFileName);

        using var f = File.OpenWrite(path);
        JsonSerializer.Serialize(f, settings);
    }
}
