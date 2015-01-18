namespace Csharp6.Csharp6.IndexInitializer
{
    using System.Collections.Generic;

    class Program
    {
        static void Main()
        {
            var dic = new Dictionary<string, int>
            {
                ["one"] = 1,
                ["two"] = 2,
            };

        }
    }
}

namespace Csharp6.Csharp5.IndexInitializer
{
    using System.Collections.Generic;

    class Sample
    {
        Dictionary<string, int> dic = new Dictionary<string, int>
        {
            ["one"] = 1,
            ["two"] = 2,
        };

        Dictionary<string, int> GetDic(int x, int y) => new Dictionary<string, int>
        {
            ["x"] = x,
            ["y"] = y,
        };
    }
}

namespace Csharp6.Csharp5.ObjectIndexInitializer
{
    class Sample
    {
        public string Name { get; set; }

        public int this[string key]
        {
            get { return 0; }
            set { }
        }
    }

    class Program
    {
        static void Main()
        {
            var s = new Sample
            {
                Name = "sample",
                ["X"] = 1,
                ["Y"] = 2,
            };
        }
    }
}
