using BenchmarkDotNet.Running;

namespace Inlining
{
    class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<WithLoop>();
            //BenchmarkRunner.Run<SimpleAdd>();
            BenchmarkRunner.Run<CommonExecutionPath>();
        }
    }
}
