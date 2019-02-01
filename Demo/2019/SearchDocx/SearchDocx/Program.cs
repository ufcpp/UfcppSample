using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Text;

namespace SearchDocx
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine(@"[usage]
SerachDocx [root path] [keywords]

指定したフォルダー以下にある txt, docx の中から指定したキーワードを含むものを検索する。

root path … 検索対象のフォルダー。
keywords  … 検索したいキーワード。スペース区切り。
");
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var root = args[0];
            var keywords = args.AsSpan().Slice(1);
            Span<bool> found = stackalloc bool[keywords.Length];

            foreach (var (name, content) in ReadAllDocs(root))
            {
                var any = false;
                for (int i = 0; i < keywords.Length; i++)
                {
                    any |= found[i] = content.Contains(keywords[i]);
                }

                if (any)
                {
                    Console.Write(name);
                    Console.Write(" / ");

                    for (int i = 0; i < keywords.Length; i++)
                    {
                        if (found[i])
                        {
                            Console.Write(keywords[i]);
                            Console.Write(" ");
                        }
                    }

                    Console.WriteLine();
                }
            }
        }

        private static IEnumerable<(string name, string content)> ReadAllDocs(string root)
        {
            foreach (var file in Directory.GetFiles(root, "*.txt", SearchOption.AllDirectories))
            {
                yield return (file.Replace(root, ""), ReadText(file));
            }

            foreach (var file in Directory.GetFiles(root, "*.docx", SearchOption.AllDirectories))
            {
                yield return (file.Replace(root, ""), ReadDocx(file));
            }
        }

        private static string ReadDocx(string path)
        {
            var package = Package.Open(path, FileMode.Open);
            var doc = package.GetPart(new Uri("/word/document.xml", UriKind.Relative));
            using var s = new StreamReader(doc.GetStream());
            return s.ReadToEnd();
        }

        private static string ReadText(string path)
        {
            using var s = new StreamReader(path, Encoding.GetEncoding("Shift_JIS"));
            return s.ReadToEnd();
        }
    }
}
