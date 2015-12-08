using ProjectModels;

namespace GenerateWrapJson
{
    class Program
    {
        /// <summary>
        /// Generate folders and project.json's to wrap csproj's for xproj-csproj interop.
        /// </summary>
        /// <param name="args">
        /// args[0]: path to a target solution
        /// </param>
        static void Main(string[] args)
        {
            var slnPath =
                args.Length >= 1 ? args[0]
                : @"path to a solution which you want to migrate from packages.config to project.json";

            var sln = new Solution(slnPath);
            sln.GenerateWrapJson();
        }
    }
}
