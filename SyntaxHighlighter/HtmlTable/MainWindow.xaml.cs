using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace HtmlTable
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ParseClipboard();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ParseClipboard();
        }

        private void ParseClipboard()
        {
            var html = Clipboard.GetData(DataFormats.Html) as string;

            string table;

            if (html != null)
            {
                table = FromHtml(html);
            }
            else
            {
                var data = Clipboard.GetData(DataFormats.UnicodeText) as string;
                if (data == null)
                    data = Clipboard.GetData(DataFormats.Text) as string;

                table = FromTabSeparated(data);
            }

            this.block.Text = table;
            Clipboard.SetDataObject(table);
        }

        static readonly Regex regTable = new Regex(@"\<table[^\>]*\>(?<body>(.|\r|\n)*?)\</table\>", RegexOptions.Compiled | RegexOptions.Multiline);
        static readonly Regex regTag = new Regex(@"(\<(?<tag>([^\>\s])*)(?<rest>([^\>]|\n|\r)*)\>)|(?<text>([^\<]|\s|\n|\r)+)", RegexOptions.Compiled | RegexOptions.Multiline);
        static readonly Regex regColspan = new Regex(@"colspan\=(?<d>\d+)", RegexOptions.Compiled);
        static readonly Regex regRowspan = new Regex(@"rowspan\=(?<d>\d+)", RegexOptions.Compiled);

        static readonly string[] validTags =
        {
            "tr", "/tr", "td", "/td", "br", "br/"
        };

        /// <summary>
        /// Word の表からコピった場合の、書式情報入りまくりの HTML を、プレーンな HTML に変換。
        /// </summary>
        /// <param name="html"></param>
        private string FromHtml(string html)
        {
            var m0 = regTable.Match(html);

            if (!m0.Success) return null;

            var body = m0.Groups["body"].Value;

            var sb = new StringBuilder();

            foreach (Match m in regTag.Matches(body))
            {
                var tag = m.Groups["tag"].Value;

                if (!string.IsNullOrEmpty(tag))
                {
                    if (validTags.Contains(tag))
                    {
                        sb.Append("<" + tag);

                        var rest = m.Groups["rest"].Value;

                        var rows = regRowspan.Match(rest);
                        if (rows.Success)
                        {
                            sb.AppendFormat(" rowspan=\"{0}\"", rows.Groups["d"].Value);
                        }

                        var cols = regColspan.Match(rest);
                        if (cols.Success)
                        {
                            sb.AppendFormat(" rowspan=\"{0}\"", cols.Groups["d"].Value);
                        }

                        sb.Append(">");
                    }
                }
                else
                {
                    var text = m.Groups["text"].Value;
                    sb.Append(text);
                }
            }

            var table = "<table>" + sb.ToString() + "</table>";
            return table;
        }

        /// <summary>
        /// タブ区切りテキストから HTML の table 生成。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string FromTabSeparated(string data)
        {
            data = data
                .Replace("&", "&amp;")
                .Replace("\"", "&quote;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                ;

            var lines = data.Replace("\r", "").Split('\n');

            var sb = new StringBuilder();

            sb.AppendLine("<table>");

            foreach (var line in lines)
            {
                sb.AppendLine("<tr>");

                var tokens = line.Split('\t');

                foreach (var x in tokens)
                {
                    sb.Append("<td>");
                    sb.Append(x);
                    sb.AppendLine("</td>");
                }

                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");

            var table = sb.ToString();
            return table;
        }
    }
}
