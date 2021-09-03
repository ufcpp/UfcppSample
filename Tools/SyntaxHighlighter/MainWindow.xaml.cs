using System;
using System.Windows;
using System.Windows.Input;

namespace SyntaxHighlighter
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new[]
            {
                Mode.Csharp,
                Mode.Xml,
                Mode.Asm,
            };
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ParseClipboard();
        }

        private void ParseClipboard()
        {
            var data = Clipboard.GetDataObject();

            var mode = (Mode)comboType.SelectedItem;

            var converted = Parse(data, mode);

            if (converted != null)
            {
                var tag = mode.Tag;

                var source = string.Format("<pre class=\"{0}\" title=\"\">{1}<code>{2}{3}</code></pre>",
                        tag,
                        Environment.NewLine,
                        converted,
                        Environment.NewLine);

                block.Text = source;
                Clipboard.SetDataObject(source);
            }
        }

        private static string? Parse(IDataObject data, Mode mode)
        {
#pragma warning disable IDE0059
            var xxx = data.GetFormats();
#pragma warning restore IDE0059

            if (mode == Mode.Asm)
            {
                if (data.GetData(DataFormats.Text) is string text)
                {
                    return AsmFormatter.MakeHtml(text);
                }
                return null;
            }

            if (data.GetData(DataFormats.Html) is string html)
            {
                var p = new HtmlParser(mode);
                return p.Parse(html);
            }

            if (data.GetData(DataFormats.Rtf) is string rtf)
            {
                var p = new RtfParser(mode);
                return p.Parse(rtf);
            }

            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ParseClipboard();
        }
    }
}
