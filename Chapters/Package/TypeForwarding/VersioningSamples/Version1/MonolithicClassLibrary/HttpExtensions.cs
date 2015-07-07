using System.Net.Http;
using System.Threading.Tasks;

public static class HttpExtensions
{
    public static async Task<string> GetStringAsync(this string url)
    {
        var c = new HttpClient();
        var res = await c.GetAsync(url);
        var s = await res.Content.ReadAsStringAsync();
        return s;
    }
}
