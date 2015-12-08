using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using D = System.Collections.Generic.Dictionary<string, object>;

namespace ProjectModels
{
    /// <summary>
    /// Manipulate a project.json file.
    /// </summary>
    public class ProjectJson
    {
        internal const string ProjectJsonName = "project.json";

        /// <summary>
        /// project.json path.
        /// </summary>
        public string Path { get; }

        public ProjectJson(string confitPath)
        {
            Path = confitPath;
        }

        /// <summary>
        /// Package infomations from a packages.config file.
        /// </summary>
        /// <param name="packagesPath"></param>
        /// <returns></returns>
        public IEnumerable<Package> Packages => _packages ?? (_packages = GetPackages());
        private IEnumerable<Package> _packages;

        private IEnumerable<Package> GetPackages()
        {
            var json = File.ReadAllText(Path);
            var obj = JsonConvert.DeserializeObject<JObject>(json);
            return GetPackagesRecursive(obj);
        }

        private IEnumerable<Package> GetPackagesRecursive(JObject obj)
        {
            foreach (var item in obj)
            {
                if (item.Key == "dependencies")
                {
                    var dependencies = (JObject)item.Value;
                    foreach (var d in dependencies)
                    {
                        yield return new Package(d.Key, (string)d.Value);
                    }
                }
                else
                {
                    var child = item.Value as JObject;
                    if(child != null)
                    {
                        foreach (var c in GetPackagesRecursive(child))
                        {
                            yield return c;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Generate a project.json file for csproj + NuGet 3.*.
        /// </summary>
        /// <param name="targetVersion"></param>
        /// <param name="csprojFolder"></param>
        /// <param name="dependencies"></param>
        public static void GeneratePackageJson(string targetVersion, string csprojFolder, IEnumerable<Package> dependencies)
        {
            var json = GetPackageJson(targetVersion, dependencies);
            var jsonPath = System.IO.Path.Combine(csprojFolder, ProjectJsonName);
            File.WriteAllText(jsonPath, json);
        }

        private static string GetPackageJson(string targetVersion, IEnumerable<Package> dependencies)
        {
            return JsonConvert.SerializeObject(new D
            {
                ["frameworks"] = new D { [targetVersion] = _empty },
                ["runtimes"] = new D
                {
                    ["win"] = _empty,
                    ["win-anycpu"] = _empty,
                },
                ["dependencies"] = ToDictionary(dependencies),
            }, Formatting.Indented);
        }

        /// <summary>
        /// Generate a project.json file for csproj + NuGet 3.*.
        /// </summary>
        /// <param name="targetVersion"></param>
        /// <param name="csprojFolder"></param>
        /// <param name="packages"></param>
        public static void GenerateWrapJson(string targetVersion, string wrapFolder, string projectName, IEnumerable<Package> packages)
        {
            var folder = System.IO.Path.Combine(wrapFolder, projectName);
            Directory.CreateDirectory(folder);

            var json = GetWrapJson(targetVersion, projectName, packages);
            var jsonPath = System.IO.Path.Combine(folder, ProjectJsonName);
            File.WriteAllText(jsonPath, json);
        }

        private static string GetWrapJson(string targetVersion, string projectName, IEnumerable<Package> packages)
        {
            return JsonConvert.SerializeObject(new D
            {
                ["version"] = "1.0.0-*",
                ["frameworks"] = new D
                {
                    [targetVersion] = new D
                    {
                        ["wrappedProject"] = $"../../{projectName}/{projectName}.csproj",
                        ["bin"] = new D
                        {
                            ["assembly"] = $"../../{projectName}/obj/{{configuration}}/{projectName}.dll",
                            ["pdb"] = $"../../{projectName}/obj/{{configuration}}/{projectName}.pdb",
                        },
                    },
                },
                ["dependencies"] = ToDictionary(packages),
            }, Formatting.Indented);
        }

        private static readonly object _empty = new object();

        private static D ToDictionary(IEnumerable<Package> packages)
        {
            var d = new D();

            foreach (var p in packages)
            {
                d[p.Id] = p.Version;
            }

            return d;
        }
    }
}
