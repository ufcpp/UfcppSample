using ProjectModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var testDataPath = @"..\..\..\TestData";
            var sourcePath = Path.Combine(testDataPath, "Source");
            var tempPath = Path.Combine(testDataPath, "Temp");

            Prepare(sourcePath, tempPath);

            var slnPath = Path.Combine(tempPath, "XprojInterop.sln");
            var sln = new Solution(slnPath);

            sln.MigrateToProjectJson();
            sln.GenerateWrapJson();

            //Dump(sln);
        }

        private static void Dump(Solution sln)
        {
            foreach (var proj in sln.CsharpProjcts)
            {
                Console.WriteLine(proj.Path);

                if (proj.HasPackagesConfig)
                {
                    Console.WriteLine("---- packages.config");
                    foreach (var p in proj.PackagesConfig.Packages)
                    {
                        Console.WriteLine(p);
                    }
                }

                if (proj.HasProjectJson)
                {
                    Console.WriteLine("---- project.json");
                    foreach (var p in proj.ProjectJson.Packages)
                    {
                        Console.WriteLine(p);
                    }
                }

                if (proj.Packages.Any())
                {
                    Console.WriteLine("---- msbuild 15");
                    foreach (var p in proj.Packages)
                    {
                        Console.WriteLine(p);
                    }
                }
            }
        }

        private static void Prepare(string sourcePath, string tempPath)
        {
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
                Directory.CreateDirectory(tempPath);
            }
            Xcopy(sourcePath, tempPath);

            Delete(tempPath, "wrap");
            Delete(tempPath, "packages");
            Delete(tempPath, "artifacts");
        }

        private static void Delete(string tempPath, string folderName)
        {
            var path = Path.Combine(tempPath, folderName);
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        /// <summary>
        /// Method to Perform Xcopy to copy files/folders from Source machine to Target Machine
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        private static void Xcopy(string sourcePath, string destinationPath)
        {
            // Use ProcessStartInfo class
            var startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "xcopy";
            startInfo.Arguments = "\"" + sourcePath + "\"" + " " + "\"" + destinationPath + "\"" + @" /e /y /I";

            using (var exeProcess = Process.Start(startInfo))
            {
                exeProcess.WaitForExit();
            }
        }
    }
}
