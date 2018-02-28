using System;
using System.IO.Compression;

class Program
{
    static void Main(string[] args)
    {
        if(args.Length < 2)
        {
            Console.WriteLine(@"Usage: csunzip [sourceArchiveFileName] [destinationDirectoryName]");
            return;
        }

        var src = args[0];
        var dst = args[1];
        ZipFile.ExtractToDirectory(src, dst);
    }
}
