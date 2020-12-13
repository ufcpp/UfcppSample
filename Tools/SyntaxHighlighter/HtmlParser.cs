using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SyntaxHighlighter
{
    /// <summary>
    /// HTML → ufcpp.net で使ってる XML に変換。
    /// Productivity Power Tool が出力する HTML を読み込む。
    /// 改行文字は &lt;br/&gt; タグになってる前提。
    /// </summary>
    /// <remarks>
    /// https://marketplace.visualstudio.com/items?itemName=VisualStudioPlatformTeam.CopyAsHtml
    /// こいつをインストールしてる前提。
    /// </remarks>
    internal class HtmlParser : IParser
    {
        public static IDictionary<string, string> ColorToTagNameCsharp = new Dictionary<string, string>
        {
            { "blue", "reserved" },
            { "green", "comment" },
            { "#2b91af", "type" },
            { "#a31515", "string" },
            { "gray", "inactive" },
            { "maroon", "string" },
            { "#74531f", "method" },
            { "#1f377f", "variable" },
            { "#8f08c4", "control" },
        };

        public static IDictionary<string, string> ColorToTagNameXml = new Dictionary<string, string>
        {
            { "blue", "attvalue" },
            { "red", "attribute" },
            { "gray", "inactive" },
            { "green", "comment" },
            { "#2b91af", "xsl" },
            { "#a31515", "element" },
        };

        private readonly IDictionary<string, string> ColorToTagNameMap;

        public HtmlParser(Mode mode)
        {
            if (mode == Mode.Csharp) ColorToTagNameMap = ColorToTagNameCsharp;
            else if (mode == Mode.Xml) ColorToTagNameMap = ColorToTagNameXml;
            else throw new InvalidOperationException();
        }

        private static readonly Regex regPre = new Regex(@"\<pre(.|\s)*?\>(?<body>(.|\s)*)\</pre", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex regColorSpan = new Regex(@"\<span style=""color:(?<color>(.|\s)*?);"">(?<body>(.|\s)*?)\</span\>", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex regEmptySpan = new Regex(@"\<span style="""">(?<body>(.|\s)*?)\</span\>", RegexOptions.Compiled | RegexOptions.Multiline);

        public string Parse(string text)
        {
            var mBody = regPre.Match(text);
            if (!mBody.Success)
            {
                return string.Empty;
            }

            string converted = mBody.Groups["body"].Value
                .Replace("font-weight:bold;", "")
                ;

            converted = regColorSpan.Replace(converted, m =>
            {
                var c = m.Groups["color"].Value;
                var body = m.Groups["body"].Value;
                if (ColorToTagNameMap.TryGetValue(c, out var tag))
                {
                    return $"<span class=\"{tag}\">{body}</span>";
                }
                // VS 2019 から、エスケープシーケンスとかがかなりカラフルに表示されるようになった。
                // そこまで class を用意するのもしんどいので、とりあえず素通しすることに。
                return m.Value;
            });

            converted = regEmptySpan.Replace(converted, m => m.Groups["body"].Value);

            converted = converted
                .Replace(" <br/>", Environment.NewLine)
                .Replace("&nbsp;", " ")
                .Replace("\t", "    ")
                .Replace("<br/>", Environment.NewLine)
                ;

            return converted;
        }
    }
}
