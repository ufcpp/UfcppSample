using System;

class Program
{
    static void Main(string[] args)
    {
        X(__arglist(1, "aaa", 'x', 1.5)); // 呼び出し側にも __arglist を書く
    }

    static void X(__arglist) // 仮引数のところに __arglist を書く
    {
        // 中身のとりだしには ArgIterator 構造体を使う
        ArgIterator argumentIterator = new ArgIterator(__arglist);
        while (argumentIterator.GetRemainingCount() > 0)
        {
            object value = null;

            var r = argumentIterator.GetNextArg(); // 可変個引数の1個1個は TypedReference になっている
            var t = __reftype(r); // TypedReference から、元の型を取得

            // 型で分岐して、__refvalue で値の取り出し
            if (t == typeof(int)) value = __refvalue(r, int);
            else if (t == typeof(char)) value = __refvalue(r, char);
            else if (t == typeof(double)) value = __refvalue(r, double);
            else value = __refvalue(r, string);

            Console.WriteLine(t.Name + ": " + value);
        }
    }
}
