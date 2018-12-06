namespace Cs8InVs2019P1.RangeAndIndex
{
    using System;

    class Program
    {
        static void Main()
        {
            var data = new[] { 0, 1, 2, 3, 4, 5 };

            // 1～2要素目。2 は exclusive。なので、表示されるのは 1 だけ。
            Write(Slice(data, 1..2));

            // 先頭から1～末尾から1。表示されるのは 1, 2, 3, 4
            Write(Slice(data, 1..^1));

            // 先頭～末尾から1。表示されるのは 0, 1, 2, 3, 4
            Write(Slice(data, ..^1));

            // 先頭から1～末尾。表示されるのは 1, 2, 3, 4, 5
            Write(Slice(data, 1..));
        }

        // .NET Core 3.0 には Span<int> に Range 型を受け取るインデクサーが入る予定。
        // Preview 1 以降なら実装入ってるっぽい。
        // Preview 1 以前の daily build な .NET Core 3.0 を使ってて、「なぜかまだない」と思ってこのメソッド実装しちゃったけど、不要。
        static Span<int> Slice(Span<int> data, Range range)
        {
#if ForOlderVersion
            int getIndex(int length, Index i) => i.FromEnd ? length - i.Value : i.Value;
            var s = getIndex(data.Length, range.Start);
            var e = getIndex(data.Length, range.End);
            return data.Slice(s, e - s);
#else
            return data[range];
#endif
        }

        // 表示確認用。Span の中身を , 区切り表示。
        static void Write<T>(Span<T> items)
        {
            var first = true;
            foreach (var x in items)
            {
                if (first) first = false;
                else Console.Write(", ");
                Console.Write(x);
            }
            Console.WriteLine();
        }
    }
}
