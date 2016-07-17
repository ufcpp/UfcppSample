namespace LocalFunctions.RecursiveFunction
{
    class Program
    {
        static void Main()
        {
            var t = new Tree();

            foreach (var x in t.Inorder())
                System.Console.WriteLine(x);
        }
    }
}
