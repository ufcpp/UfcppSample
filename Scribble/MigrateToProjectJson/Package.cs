using System.Linq;

namespace MigrateToProjectJson
{
    class Package
    {
        public string Id { get; set; }
        public string Version { get; set; }

        public string AnyVersion => string.Join(".", Version.Split('.').Take(2)) + ".*";

        public override string ToString() => $"{Id}: {Version}";
    }
}
