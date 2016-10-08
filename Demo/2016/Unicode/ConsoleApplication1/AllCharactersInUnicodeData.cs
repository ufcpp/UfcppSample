using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Text.Encoding;


namespace ConsoleApplication1
{
    class AllCharactersInUnicodeData
    {
        public static async Task Count()
        {
            const string dataFolder = "data";

            await DownloadDataAsync(dataFolder);
            var items = ReadData(dataFolder).ToArray();

            foreach (var item in items)
            {
                Console.WriteLine(item.Version);

                Console.WriteLine("  category : # all characters / # non-BMP characters");
                Console.WriteLine($"  total: {item.List.Count} / {item.List.Count(x => x > 0x10000)}");
                Console.WriteLine($"  added: {item.Difference.Count} / {item.Difference.Count(x => x > 0x10000)}");

                var buffer = new byte[4];

                var categories = item.List
                    .GroupBy(x => GetUnicodeCategory(x, buffer))
                    .Select(g => (category: g.Key, count: g.Count(), scount: g.Count(x => x > 0x10000)))
                    .ToArray();

                foreach (var c in categories)
                {
                    Console.WriteLine($"  {c.category,25}: {c.count,5} / {c.scount,5}");
                }

                //Console.Read();
            }
        }

        private static UnicodeCategory GetUnicodeCategory(int codePoint, byte[] buffer)
        {
            buffer[0] = unchecked((byte)(codePoint >> 0));
            buffer[1] = unchecked((byte)(codePoint >> 8));
            buffer[2] = unchecked((byte)(codePoint >> 16));
            buffer[3] = unchecked((byte)(codePoint >> 24));
            var s = UTF32.GetString(buffer);
            var c = char.GetUnicodeCategory(s, 0);
            return c;
        }

        struct Ucd
        {
            public string Version { get; }

            /// <summary>
            /// このバージョンが含んでいるコードポイント。
            /// 順序通り。
            /// </summary>
            public IReadOnlyList<int> List { get; }

            /// <summary>
            /// このバージョンが含んでいるコードポイント。
            /// 検索用。
            /// </summary>
            public ICollection<int> Set { get; }

            /// <summary>
            /// 前のバージョンからの差分
            /// </summary>
            public ICollection<int> Difference { get; }

            public Ucd(string version, StreamReader unicodeData, Ucd prev = default(Ucd))
            {
                Version = version;

                var list = new List<int>();

                string line;
                while ((line = unicodeData.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (line.StartsWith("#")) continue;

                    var items = line.Split(';');
                    var codepoint = int.Parse(items[0], System.Globalization.NumberStyles.HexNumber);

                    list.Add(codepoint);
                }

                List = list;
                Set = new HashSet<int>(list);

                if (prev.Set != null)
                {
                    Difference = list.Except(prev.Set).ToArray();
                }
                else
                {
                    Difference = Array.Empty<int>();
                }
            }
        }

        private static IEnumerable<Ucd> ReadData(string dataFolder)
        {
            var prev = default(Ucd);

            foreach (var file in Directory.GetFiles(dataFolder))
            {
                var version = Path.GetFileName(file);

                using (var r = new StreamReader(file))
                {
                    var ucd = new Ucd(version, r, prev);
                    yield return ucd;
                    prev = ucd;
                }
            }
        }

        private static async Task DownloadDataAsync(string dataFolder)
        {
            if (Directory.Exists(dataFolder))
                return;

            Directory.CreateDirectory(dataFolder);

            var urls = new[]
            {
            "http://www.unicode.org/Public/reconstructed/1.0.0/UnicodeData.txt",
            "http://www.unicode.org/Public/reconstructed/1.0.1/UnicodeData.txt",
            "http://www.unicode.org/Public/2.0-Update/UnicodeData-2.0.14.txt",
            "http://www.unicode.org/Public/2.1-Update/UnicodeData-2.1.2.txt",
            "http://www.unicode.org/Public/2.1-Update2/UnicodeData-2.1.5.txt",
            "http://www.unicode.org/Public/2.1-Update3/UnicodeData-2.1.8.txt",
            "http://www.unicode.org/Public/2.1-Update4/UnicodeData-2.1.9.txt",
            "http://www.unicode.org/Public/3.0-Update/UnicodeData-3.0.0.txt",
            "http://www.unicode.org/Public/3.1-Update/UnicodeData-3.1.0.txt",
            "http://www.unicode.org/Public/3.2-Update/UnicodeData-3.2.0.txt",
            "http://www.unicode.org/Public/4.0-Update/UnicodeData-4.0.0.txt",
            "http://www.unicode.org/Public/4.1.0/ucd/UnicodeData.txt",
            "http://www.unicode.org/Public/5.0.0/ucd/UnicodeData.txt",
            "http://www.unicode.org/Public/5.1.0/ucd/UnicodeData.txt",
            "http://www.unicode.org/Public/5.2.0/ucd/UnicodeData.txt",
            "http://www.unicode.org/Public/6.0.0/ucd/UnicodeData.txt",
            "http://www.unicode.org/Public/6.1.0/ucd/UnicodeData.txt",
            "http://www.unicode.org/Public/6.2.0/ucd/UnicodeData.txt",
            "http://www.unicode.org/Public/6.3.0/ucd/UnicodeData.txt",
            "http://www.unicode.org/Public/7.0.0/ucd/UnicodeData.txt",
            "http://www.unicode.org/Public/8.0.0/ucd/UnicodeData.txt",
            "http://www.unicode.org/Public/9.0.0/ucd/UnicodeData.txt",
            };

            var regVersion = new Regex(@"\d+\.\d+\.\d+");
            var h = new HttpClient();

            foreach (var url in urls)
            {
                var version = regVersion.Match(url).Value;

                Console.WriteLine($"download UnicodeData {version}");

                var res = await h.GetAsync(url);
                using (var source = await res.Content.ReadAsStreamAsync())
                using (var destination = File.OpenWrite(Path.Combine(dataFolder, version)))
                {
                    await source.CopyToAsync(destination);
                }
            }
        }
    }
}
