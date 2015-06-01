using static System.Console;

class Program
{
    static void Main(string[] args)
    {
        WriteLine(nameof(Person.Id) == "Id");
    }
}

class Person
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
