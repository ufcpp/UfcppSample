using System;
using System.IO.Compression;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        if(args.Length < 2)
        {
            Console.WriteLine(@"Usage: cszip [sourceDirectoryName] [destinationArchiveFileName]");
            return;
        }

        var src = args[0];
        var dst = args[1];
        ZipFile.CreateFromDirectory(src, dst, CompressionLevel.Optimal, true, Encoding.UTF8);
    }
}
