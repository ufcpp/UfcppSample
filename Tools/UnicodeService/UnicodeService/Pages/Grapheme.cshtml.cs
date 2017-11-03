using System.Linq;
using GraphemeSplitter;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UnicodeService.Pages
{
    public class GraphemeModel : PageModel
    {
        public string Text { get; private set; }
        public string[] SplittedTexts { get; private set; }

        public void OnGet(string s)
        {
            if (s == null) return;

            Text = s;
            SplittedTexts = s.GetGraphemes()
                .Select(x => x.ToString())
                .ToArray();
        }
    }
}
