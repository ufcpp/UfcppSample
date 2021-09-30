using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace CsharpToHtml;

public static class HtmlHelper
{
    public static async Task<string> ToHtmlAsync(this Document doc, bool useStyle)
    {
        var text = await doc.GetTextAsync();
        var classifiedSpans = await Classifier.GetClassifiedSpansAsync(doc, TextSpan.FromBounds(0, text.Length));
        return text.ToHtml(classifiedSpans, useStyle);
    }

    public static string ToHtml(this SourceText text, IEnumerable<ClassifiedSpan> spans, bool useStyle)
        => ToHtml(text.ToString(), spans, useStyle);

    public static string ToHtml(ReadOnlySpan<char> text, IEnumerable<ClassifiedSpan> spans, bool useStyle)
    {
        var s = new StringBuilder();

        var last = 0;

        foreach (var span in spans)
        {
            if (span.ClassificationType == "static symbol") continue;

            var end = span.TextSpan.End;

            if (end <= last) continue;

            var start = span.TextSpan.Start;

            if (start > last)
            {
                s.AppendEscape(text[last..start]);
            }

            var @class = ClassTable.TypeToClass(span.ClassificationType);

            if (@class is null)
            {
                s.AppendEscape(text[start..end]);
            }
            else
            {
                if (useStyle)
                {
                    var color = ClassTable.ClassToColor(@class);
                    s.Append($"<span style=\"color:#{color};\">");
                }
                else
                {
                    s.Append($"<span class=\"{@class}\">");
                }
                s.AppendEscape(text[start..end]);
                s.Append("</span>");
            }

            last = end;
        }

        if (last < text.Length)
        {
            s.Append(text[last..text.Length]);
        }

        return s.ToString();
    }

    public static void AppendEscape(this StringBuilder builder, ReadOnlySpan<char> rawText)
    {
        foreach (var c in rawText)
        {
            switch (c)
            {
                case '<':
                    builder.Append("&lt;");
                    break;
                case '>':
                    builder.Append("&gt;");
                    break;
                case '&':
                    builder.Append("&amp;");
                    break;
                case '"':
                    builder.Append("&quot;");
                    break;
                default:
                    builder.Append(c);
                    break;
            }
        }
    }
}
