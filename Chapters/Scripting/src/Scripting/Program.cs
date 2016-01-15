using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Threading.Tasks;

public class Program
{
    public static void Main(string[] args)
    {
        MainAsync().Wait();
    }

    private static async Task MainAsync()
    {
        var result = await CSharpScript.EvaluateAsync<int>(@"
var x = 1;
var y = 2;
x + y
");
        Console.WriteLine(result);
    }
}
