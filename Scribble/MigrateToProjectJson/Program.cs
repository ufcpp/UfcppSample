using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using D = System.Collections.Generic.Dictionary<string, object>;

namespace MigrateToProjectJson
{
    class Program
    {
        const string packagesConf = "packages.config";
        const string projectJson = "project.json";
        static readonly Regex regProject = new Regex(@" = "".*?"", ""(?<csproj>.*?\.csproj)""");

        static void Main(string[] args)
        {
            var slnPath =
                args.Length >= 1 ? args[0]
                : @"path to a solution which you want to migrate from packages.config to project.json";

            var baseFolder = Path.GetDirectoryName(slnPath);
            var slnLines = File.ReadAllLines(slnPath);

            foreach (var line in slnLines)
            {
                var m = regProject.Match(line);
                if (!m.Success) continue;

                var csprojPath = Path.Combine(baseFolder, m.Groups["csproj"].Value);
                if (!File.Exists(csprojPath)) continue;

                string targetVersion = ReadTargetFrameworkVersion(csprojPath);

                var csprojFolder = Path.GetDirectoryName(csprojPath);
                var packagesPath = Path.Combine(csprojFolder, packagesConf);

                if (!File.Exists(packagesPath)) continue;

                var packages = ReadPackages(packagesPath);
                WriteJson(targetVersion, csprojFolder, packages);
                RewriteCsproj(csprojPath, packages);
                File.Delete(packagesPath);
            }
        }

        private static void RewriteCsproj(string csprojPath, IEnumerable<Package> packages)
        {
            var doc = XDocument.Load(csprojPath);
            var ns = (XNamespace)"http://schemas.microsoft.com/developer/msbuild/2003";
            var itemGroups = doc.Root.Elements(ns + "ItemGroup");
            var changed = false;

            foreach (var item in itemGroups)
            {
                var references = item.Elements(ns + "Reference").ToArray();

                foreach (var r in references)
                {
                    if (packages.Any(x => r.Attribute("Include").Value.StartsWith(x.Id + ",")))
                    {
                        r.Remove();
                        changed = true;
                    }
                }

                var none = item.Elements(ns + "None").ToArray();

                foreach (var n in none)
                {
                    var include = n.Attribute("Include");
                    if (include.Value == packagesConf)
                    {
                        n.ReplaceAll(new XAttribute("Include", projectJson));
                        changed = true;
                    }
                }
            }

            if (changed)
                doc.Save(csprojPath);
        }

        private static void WriteJson(string targetVersion, string csprojFolder, IEnumerable<Package> packages)
        {
            var json = GetJson(targetVersion, packages);
            var jsonPath = Path.Combine(csprojFolder, projectJson);
            File.WriteAllText(jsonPath, json);
        }

        private static string GetJson(string targetVersion, IEnumerable<Package> packages)
        {
            return JsonConvert.SerializeObject(new D
            {
                ["frameworks"] = new D {[targetVersion] = _empty },
                ["runtimes"] = new D
                {
                    ["win"] = _empty,
                    ["win-anycpu"] = _empty,
                },
                ["dependencies"] = ToDictionary(packages),
            }, Formatting.Indented);
        }

        private static string ReadTargetFrameworkVersion(string csprojPath)
        {
            var targetVersionLine = File.ReadAllLines(csprojPath).First(x => x.Contains("TargetFrameworkVersion"));

            var targetVersion =
                targetVersionLine.Contains("v2.0") ? "net20" :
                targetVersionLine.Contains("v3.0") ? "net30" :
                targetVersionLine.Contains("v3.5") ? "net35" :
                targetVersionLine.Contains("v4.5") ? "net45" :
                targetVersionLine.Contains("v4.6") ? "net46" :
                "dotnet";
            return targetVersion;
        }

        private static readonly object _empty = new object();

        private static IEnumerable<Package> ReadPackages(string packagesPath)
        {
            var doc = XDocument.Load(packagesPath);
            var items = doc.Root.Elements("package");
            var packages = items.Select(x => new Package { Id = (string)x.Attribute("id"), Version = (string)x.Attribute("version") });
            return packages;
        }

        private static D ToDictionary(IEnumerable<Package> packages)
        {
            var d = new D();

            foreach (var p in packages)
            {
                d[p.Id] = p.AnyVersion;
            }

            return d;
        }
    }
}
