/// <summary>
/// Can be compiled in: C# 2, 6
/// 
/// https://github.com/dotnet/roslyn/blob/master/docs/compilers/CSharp/Definite%20Assignment.md
/// </summary>
class DefiniteAssignment
{
    static void Main(string[] args)
    {
        int x;
        if (false && x == 3) // Dev10 does not consider x definitely assigned
        {
            x = x + 1;
        }
    }
}
