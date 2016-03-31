using System;

struct Buffer<T>
{
    public int BaseIndex { get; }

    private readonly T[] _array;

    public Buffer(int baseIndex, int count)
    {
        BaseIndex = baseIndex;
        _array = new T[count];
    }

    public ref T this[int index] => ref _array[index - BaseIndex];
}

class Base { }
class A : Base { public int Value { get; set; } }
class B : Base { public string Name { get; set; } }

class Program
{
    static void Main(string[] args)
    {
        BinaryLiteralsDeigitSeparator();
        RefReturnsRefLocals();
        LocalFunctions();
        PatternMatching(new A { Value = 1 });
        PatternMatching(new A { Value = 2 });
        PatternMatching(new B { Name = "abc" });
    }

    private static void PatternMatching(Base x)
    {
        if (x is A a) Console.WriteLine($"A: {a.Value}");
        else if (x is B b) Console.WriteLine($"B: {b.Name}");

        switch (x)
        {
            case A { Value is 1 }:
                Console.WriteLine($"A whose value is 1");
                break;
            case A a:
                Console.WriteLine($"A: {a.Value}");
                break;
            case B b:
                Console.WriteLine($"B: {b.Name}");
                break;
            case *:
                break;
        }
    }

    private static void BinaryLiteralsDeigitSeparator()
    {
        var b = 0b1000_0001;
        Console.WriteLine(b);
    }

    private static void LocalFunctions()
    {
        int F(int n) => n >= 1 ? n * F(n - 1) : 1;
        Console.WriteLine(F(5));
    }

    private static void RefReturnsRefLocals()
    {
        var buffer = new Buffer<int>(5, 5);
        buffer[5] = 1;
        ref var x = ref buffer[6];
        x = 2;
        Console.WriteLine($"{buffer[5]}, {buffer[6]}");
    }
}
