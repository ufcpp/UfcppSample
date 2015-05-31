using static System.Console;

class Program
{
    static void Main(string[] args)
    {
        WriteLine(nameof(Person.Key) == "Id");
    }
}

class Person
{
    public string Key { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
