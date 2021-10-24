using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// テンプレに1行を追加。DI 用。
builder.Services.AddSingleton<Counter>();

var app = builder.Build();

// テンプレを1行書き換え。引数を DI で受け取ったり、クエリ文字列から受け取ったり。
// counter: ページをリロードするたびに +1。
// value: クエリ文字列で数値を指定。
// その2つの値から何らかの計算して返す。
app.MapGet("/", ([FromServices] Counter counter, [FromQuery] int? value) => counter.Count * (value ?? 1));

app.Run();

// テンプレに1クラス追加。上記 DI で渡すデモ用の型。
class Counter
{
    private int _count;
    public int Count { get => _count++; }
}
