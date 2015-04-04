using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CodeAnalysisForCSharp6
{
    /// <summary>
    /// ソリューションと、そのソリューション内の全 .cs ファイルのコンパイル結果を持っておくクラス。
    /// </summary>
    class SolutionLoader
    {
        /// <summary>
        /// 読み込んだソリューション。
        /// </summary>
        public Solution Solution { get; }

        /// <summary>
        /// <see cref="Solution"/> 内の全 .cs ファイルの構文解析結果。
        /// </summary>
        public IEnumerable<SyntaxTree> SyntaxTrees { get; }

        /// <summary>
        /// <see cref="SyntaxTrees"/> を全部コンパイルした結果。
        /// </summary>
        public CSharpCompilation Compilation { get; }

        private SolutionLoader(Solution solution)
        {
            Solution = solution;
            SyntaxTrees = GetSyntaxTrees(solution);
            Compilation =  CSharpCompilation.Create("temp", SyntaxTrees);
        }

        /// <summary>
        /// ソリューション読み込み。
        /// </summary>
        /// <param name="path">ソリューションのパス。</param>
        /// <returns>読み込み結果。</returns>
        public static async Task<SolutionLoader> LoadAsync(string path)
        {
            var w = Microsoft.CodeAnalysis.MSBuild.MSBuildWorkspace.Create();
            var solution = await w.OpenSolutionAsync(path);

            var solutionName = Path.GetFileName(solution.FilePath);

            return new SolutionLoader(solution);
        }

        private IEnumerable<SyntaxTree> GetSyntaxTrees(Solution solution)
        {
            foreach (var project in solution.Projects)
            {
                foreach (var doc in project.Documents)
                {
                    var ext = Path.GetExtension(doc.FilePath);
                    if (ext == ".cs")
                    {
                        var text = File.ReadAllText(doc.FilePath);
                        yield return CSharpSyntaxTree.ParseText(SourceText.From(text));
                    }
                }
            }
        }
    }
}
