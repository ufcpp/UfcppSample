namespace OutVar
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        class csharp6
        {
            static int? ParseOrDefault(string s)
            {
                int x;
                return int.TryParse(s, out x) ? x : default(int?);
            }
        }

        class csharp7int
        {
            static int? ParseOrDefault(string s)
            {
                return int.TryParse(s, out int x) ? x : default(int?);
            }
        }

        class csharp7var
        {
            static int? ParseOrDefault(string s)
            {
                return int.TryParse(s, out var x) ? x : default(int?);
            }
        }

        class csharp7expression
        {
            static int? ParseOrDefault(string s) => int.TryParse(s, out var x) ? x : default(int?);
        }
    }
}
