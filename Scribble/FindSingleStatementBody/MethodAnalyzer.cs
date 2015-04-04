using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysisForCSharp6
{
    /// <summary>
    /// メソッド関連の解析。
    /// </summary>
    static class MethodAnalyzer
    {
        public static IEnumerable<MethodSyntaxInfo> GetMethods(this SolutionLoader s)
        {
            foreach (var tree in s.SyntaxTrees)
            {
                var root = tree.GetRoot();

                foreach (var cd in root.DescendantNodes().Where(n => n.Kind() == SyntaxKind.ClassDeclaration).Cast<ClassDeclarationSyntax>())
                {
                    foreach (var m in cd.ChildNodes().Where(n => n.Kind() == SyntaxKind.MethodDeclaration).Cast<MethodDeclarationSyntax>())
                    {
                        yield return new MethodSyntaxInfo(tree, m);
                    }
                }
            }
        }
    }

    /// <summary>
    /// メソッドの構文解析結果。
    /// </summary>
    class MethodSyntaxInfo
    {
        public SyntaxTree Tree { get; }
        public MethodDeclarationSyntax Method { get; }

        public MethodSyntaxInfo(SyntaxTree tree, MethodDeclarationSyntax method)
        {
            Tree = tree;
            Method = method;

            HasBody = method.Modifiers.All(x => x.Text != "abstract" && x.Text != "partial");
            StatementCount = method.Body.Statements.Count;
        }

        /// <summary>
        /// ボディを持つ(= abstract, partial ではない)。
        /// </summary>
        public bool HasBody { get; }

        /// <summary>
        /// ステートメント数。
        /// </summary>
        public int StatementCount { get; }
    }
}
