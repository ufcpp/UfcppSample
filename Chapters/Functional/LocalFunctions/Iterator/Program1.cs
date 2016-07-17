namespace LocalFunctions.Iterator1
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        static void Main()
        {
            IEnumerable<string> input = null;

            // input が null なので例外を投げてほしい
            // 多くの人がそれを期待する
            var output = input.Where(x => x.Length < 10);

            Console.WriteLine("ここが表示されるとおかしい"); // でも表示される

            foreach (var x in output) // 実際に例外が出るのはこの行
            {
                Console.WriteLine(x);
            }
        }
    }
}
