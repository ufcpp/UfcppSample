namespace InterfaceSample.Explicit
{
    using System;
    using System.Collections.Generic;

    interface IDataSource<T>
    {
        IEnumerable<T> Items { get; }
    }

    /// <summary>
    /// 同名・別戻り値型なメソッド/プロパティは、C#では普通には作れないので、
    /// こういう場合は明示的実装が必須。
    /// </summary>
    class MultipleDataSource : IDataSource<int>, IDataSource<string>, IDataSource<double>
    {
        IEnumerable<int> IDataSource<int>.Items => new[] { 1, 2, 3, 4, 5 };

        public IEnumerable<string> Items => new[] { "one", "two", "three" };

        IEnumerable<double> IDataSource<double>.Items => new[] { 1.4, 2.7, 3.1 };
    }

    class GenericInterfaceSample
    {
        public static void Main()
        {
            var source = new MultipleDataSource();
            WriteLine<int>(source);
            WriteLine<string>(source);
            WriteLine<double>(source);
        }

        static void WriteLine<T>(IDataSource<T> source)
        {
            Console.WriteLine(string.Join(", ", source.Items));
        }
    }
}
