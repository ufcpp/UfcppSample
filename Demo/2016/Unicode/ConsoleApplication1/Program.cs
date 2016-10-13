using System.Linq;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var jis = System.Text.Encoding.GetEncoding("ISO-2022-JP");

            var data = jis.GetBytes("abcあいうdef");
            System.Console.WriteLine(string.Join(", ", data.Select(x => x.ToString("X"))));

            var s = jis.GetString(new byte[] { 0x1b, 0x24, 0x42, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, });
            System.Console.WriteLine(s);
            return;

            NoAllocation.Test();
            AllCharactersInUnicodeData.Count().Wait();
            CharacterLength.WriteLength();
        }
    }
}
