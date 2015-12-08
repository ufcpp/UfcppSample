using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectModels
{
    /// <summary>
    /// Manipulate a *.sln file.
    /// </summary>
    public class Solution
    {
        /// <summary>
        /// *.sln path.
        /// </summary>
        public string Path { get; }

        public Solution(string slnPath)
        {
            Path = slnPath;
        }

        /// <summary>
        /// A folder which contains this *.sln file.
        /// </summary>
        public string Folder => System.IO.Path.GetDirectoryName(Path);

        public IEnumerable<Csproj> CsharpProjcts => _csprojs ?? (_csprojs = GetProjectPaths(Path).ToArray());
        private IEnumerable<Csproj> _csprojs;

        private static readonly Regex regProject = new Regex(@" = "".*?"", ""(?<csproj>.*?\.csproj)""");

        /// <summary>
        /// Enumerate all projects in a solution.
        /// </summary>
        /// <param name="slnPath"></param>
        /// <returns></returns>
        private static IEnumerable<Csproj> GetProjectPaths(string slnPath)
        {
            var baseFolder = System.IO.Path.GetDirectoryName(slnPath);
            var slnLines = File.ReadAllLines(slnPath);

            foreach (var line in slnLines)
            {
                var m = regProject.Match(line);
                if (!m.Success) continue;

                var relative = m.Groups["csproj"].Value;
                var csprojPath = System.IO.Path.Combine(baseFolder, relative);
                if (!File.Exists(csprojPath)) continue;

                yield return new Csproj(baseFolder, relative);
            }
        }

        public void MigrateToProjectJson()
        {
            foreach (var csproj in CsharpProjcts.Where(p => p.HasPackagesConfig))
            {
                if (csproj.MigrateToProjectJson())
                    csproj.Save();
            }
        }

        private string WrapFolder => System.IO.Path.Combine(Folder, "wrap");

        public void GenerateWrapJson()
        {
            var wrapFolder = WrapFolder;
            Directory.CreateDirectory(wrapFolder);

            foreach (var csproj in CsharpProjcts.Where(p => p.OutputType == CsprojOutputType.Library))
            {
                csproj.GenerateWrapJson(wrapFolder);
            }
        }
    }
}
