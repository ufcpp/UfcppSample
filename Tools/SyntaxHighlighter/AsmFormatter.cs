using System;
using System.Linq;
using System.Text;

namespace SyntaxHighlighter
{
    public class AsmFormatter
    {
        /// <summary>
        /// IL/x86 コード用。
        /// こいつはプレーンテキストで来ることを想定して、
        /// ラベル、命令、コメントだけ色付け。
        /// </summary>
        public static string MakeHtml(string asmCode)
        {
            var sb = new StringBuilder();

            var lines = asmCode.Split('\n');

            sb.Append(@"<pre class=""source"">
<code>");

            foreach (var line in lines)
            {
                var trimmed = line.TrimEnd('\r');
                var token = trimmed.Split(' ');

                if (token.Length < 2)
                {
                    sb.Append(trimmed);
                    sb.Append(Environment.NewLine);
                    continue;
                }

                sb.Append("<span style=\"color:purple\">");
                sb.Append(token[0]);
                sb.Append("</span> <span style=\"color:blue\">");
                sb.Append(token[1]);
                sb.Append("</span>");
                var hasComment = false;
                foreach (var t in token.Skip(2))
                {
                    if (t == "//" || t == ";")
                    {
                        hasComment = true;
                        sb.Append("<span style=\"color:green\">");
                    }
                    sb.Append(' ');
                    sb.Append(t);
                }
                if (hasComment) sb.Append("</span>");
                sb.Append(Environment.NewLine);
            }
            sb.Append("</code></pre>");

            return sb.ToString();
        }
    }
}
