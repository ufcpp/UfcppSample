using System.Linq;

namespace ProjectModels
{
    public class Package
    {
        public string Id { get; }
        public string Version { get; }

        /// <summary></summary>
        /// <param name="id"><see cref="Id"/></param>
        /// <param name="version"><see cref="Version"/></param>
        public Package(string id, string version)
        {
            Id = id;
            Version = version;
        }

        public string AnyVersion => string.Join(".", Version.Split('.').Take(2)) + ".*";

        public override string ToString() => $"{Id}: {Version}";
    }
}
