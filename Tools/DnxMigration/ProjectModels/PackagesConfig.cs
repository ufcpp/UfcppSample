using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ProjectModels
{
    /// <summary>
    /// Manipulate a packages.config file.
    /// </summary>
    public class PackagesConfig
    {
        /// <summary>
        /// packages.config path.
        /// </summary>
        public string Path { get; }

        public PackagesConfig(string confitPath)
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
            var doc = XDocument.Load(Path);
            var items = doc.Root.Elements("package");
            var packages = items.Select(x => new Package((string)x.Attribute("id"), (string)x.Attribute("version")));
            return packages;
        }
    }
}
