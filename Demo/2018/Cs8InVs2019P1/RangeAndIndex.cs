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

        // 最終的に、.NET Core 3.0 には Span<int> に Range 型を受け取るインデクサーが入るはず。
        // 今はその実装がないので自前で同じ機能を作る。
        static Span<int> Slice(Span<int> data, Range range)
        {
            int getIndex(int length, Index i) => i.FromEnd ? length - i.Value : i.Value;
            var s = getIndex(data.Length, range.Start);
            var e = getIndex(data.Length, range.End);
            return data.Slice(s, e - s);
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
