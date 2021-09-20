m(null);
m(new X { Name = "" });
m(new X { Name = "a" });
m(new X { Name = "abc" });

static void m(X? x)
{
    if (x is { Name.Length: 1 })
    {
        Console.WriteLine("single-char Name");
    }

#if false
    // これと同じ意味。
    if (x is { Name: { Length: 1 } })
    {
        Console.WriteLine("single-char Name");
    }
#endif

    // さらに言うとこれとほぼ同じ意味。
    if (x is not null)
    {
        var name = x.Name;
        if (name is not null)
        {
            var length = name.Length;
            if (length == 1)
            {
                Console.WriteLine("single-char Name");
            }
        }
    }
}

class X
{
    public string? Name { get; set; }
}
