using System;
using System.Runtime.CompilerServices;

namespace PathMap
{
    class Program
    {
        static void Main()
        {
            // CallerFilePath で、このファイルのパスがコンソール出力される。
            // 通常ならフルパス。
            // 今回は csproj に $(MSBuildProjectDirectory)=. と PathMap 設定を入れているので、
            // .\Program.cs
            // と表示される。
            M();

            // スタックトレースにも影響。
            try
            {
                Throw();
            }
            catch (Exception ex)
            {
                // これも、フルパスだったものが短いスタックトレースに変わる:
                //.\Program.cs
                //   at PathMap.Program.Throw() in .\Program.cs:line 33
                //   at PathMap.Program.Main(String[] args) in .\Program.cs:line 20
                Console.WriteLine(ex.StackTrace);
            }
        }

        static void M([CallerFilePath] string path = null) => Console.WriteLine(path);
        static void Throw() => throw new Exception();
    }
}
