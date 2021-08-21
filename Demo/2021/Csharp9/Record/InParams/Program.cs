using System;

var r = new Record(1, 2, 3, 4);

Console.WriteLine(r.X); // 1

foreach (var item in r.Y)
{
    Console.WriteLine(item); // 2, 3, 4
}

// レコード型のプライマリ コンストラクター引数(からのプロパティ生成)、
// in と params は受け付けるらしい。
public record Record(in int X, params int[] Y);

// ちなみに、 ref と out はダメ。
//public record Record2(ref int X, out int Y);
