#define USE_UTF8

using System;

#if USE_UTF8
using MyString = UtfString.Utf8.String;
#else
using MyString = UtfString.Utf16.String;
#endif

namespace ConsoleApplication1
{
    class NoAllocation
    {
        public static void AllocationCheck()
        {
            const int N = 10000;
            var str = "aáαℵあáあ゙亜👩👩🏽👨🏻‍👩🏿‍👦🏽‍👦🏼";
#if USE_UTF8
            var encoding = System.Text.Encoding.UTF8;
#else
            var encoding = System.Text.Encoding.Unicode;
#endif
            var data = encoding.GetBytes(str);

            Console.WriteLine(data.Length);
            Console.WriteLine(str);
            Console.WriteLine(str.Length);

            GC.Collect();
            {
                Console.WriteLine("--------");
                var begin = GC.GetTotalMemory(false);
                Console.WriteLine(begin);
                (int total, int ascii, int latin1, int utf16, int surrogatePair) len = (0, 0, 0, 0, 0);

                for (int i = 0; i < N; i++)
                {
                    var s = new MyString(data);
                    len = (0, 0, 0, 0, 0);
                    foreach (var x in s)
                    {
                        len.total++;
                        if (x.Value < 0x80) len.ascii++;
                        else if (x.Value < 0x100) len.latin1++;
                        else if (x.Value < 0x10000) len.utf16++;
                        else len.surrogatePair++;
                    }
                }
                var end = GC.GetTotalMemory(false);
                Console.WriteLine(end);
                Console.WriteLine($"{end - begin} {(end - begin) / N}");
                Console.WriteLine("\t" + len);
            }

            GC.Collect();
            {
                Console.WriteLine("--------");
                var begin = GC.GetTotalMemory(false);
                Console.WriteLine(begin);
                (int total, int ascii, int latin1, int utf16, int surrogatePair) len = (0, 0, 0, 0, 0);

                for (int i = 0; i < N; i++)
                {
                    var s = encoding.GetString(data);
                    len = (0, 0, 0, 0, 0);
                    foreach (var x in s)
                    {
                        if (!char.IsLowSurrogate(x)) len.total++;
                        if (x < 0x80) len.ascii++;
                        else if (x < 0x100) len.latin1++;
                        else if (!char.IsSurrogate(x)) len.utf16++;
                        else if (char.IsHighSurrogate(x)) len.surrogatePair++;
                    }
                }
                var end = GC.GetTotalMemory(false);
                Console.WriteLine(end);
                Console.WriteLine($"{end - begin} {(end - begin) / N}");
                Console.WriteLine("\t" + len);
            }
        }
    }
}
