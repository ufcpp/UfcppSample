using ProjectModels;

namespace MigrateToMsbuild15
{
    /// <summary>
    /// Convert packages.config and project.json in all projects in a solution to the new csproj format (PackageReference tags) in msbuild 15 (VS 2017).
    /// </summary>
    /// <param name="args">
    /// args[0]: path to a target solution
    /// </param>
    class Program
    {
        static void Main(string[] args)
        {
            var slnPath =
                args.Length >= 1 ? args[0]
                : @"path to a solution which you want to migrate from packages.config and project.json to PackageReference tags";

            var sln = new Solution(slnPath);
            sln.MigrateToMsbuild15();
        }
    }
}
