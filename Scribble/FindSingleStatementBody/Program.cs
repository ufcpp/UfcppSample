using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FindSingleStatementBody
{
    class Programs
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("引数にソリューション名を指定してください");
                return;
            }

            ReadCsharpSourceCodes(args[0]).Wait();
        }

        private static async Task ReadCsharpSourceCodes(string path)
        {
            var w = Microsoft.CodeAnalysis.MSBuild.MSBuildWorkspace.Create();
            var solution = await w.OpenSolutionAsync(path);

            var solutionName = Path.GetFileName(solution.FilePath);
            var c = CSharpCompilation.Create(solutionName + ".dll");

            FindGetOnlyPropertyWhichHasSingleStatement(solution);
            FindMethodWhichHasSingleStatement(solution);
        }

        private static void FindGetOnlyPropertyWhichHasSingleStatement(Solution solution)
        {
            var items = (
                from p in GetProperties(solution)
                where !HasNoBody(p)
                let getter = p.AccessorList.Accessors.Count != 1 ? null : p.AccessorList.Accessors.SingleOrDefault(a => a.Keyword.Text == "get")
                let IsGetOnly = getter != null
                let HasSingleStatement = getter != null && getter.Body != null && getter.Body.Statements.Count == 1
                select new { IsGetOnly, HasSingleStatement }
                ).ToArray();

            var total = 0;
            var getOnly = 0;
            var singleStatement = 0;

            foreach (var x in items)
            {
                total++;
                if (x.IsGetOnly) getOnly++;
                if (x.HasSingleStatement) singleStatement++;
            }

            Console.WriteLine($"total: {total}, get-only: {getOnly}, single stetement: {singleStatement}");
            Console.WriteLine($"プロパティのうち {singleStatement}個({(double)singleStatement / total * 100: 0.00}%) がステートメント1個だけ");
            Console.WriteLine($"プロパティ全体の {(double)singleStatement / total * 100: 0.00}%");
            Console.WriteLine($"get だけのプロパティのうち {(double)singleStatement / getOnly * 100: 0.00}%");
        }

        private static bool HasNoBody(PropertyDeclarationSyntax p)
        {
            return p.Modifiers.Any(x => x.Text == "abstract");
        }

        private static void FindMethodWhichHasSingleStatement(Solution solution)
        {
            var groups = (
                from m in GetMethods(solution)
                where !HasNoBody(m)
                group m by m.Body.Statements.Count into g
                orderby g.Key
                select new { g.Key, Count = g.Count() }
                ).ToArray();

            var sum = 0;
            foreach (var x in groups)
            {
                //Console.WriteLine($"ステートメント数が {x.Key} 個のメソッドは {x.Count} 個ある");
                sum += x.Count;
            }

            Console.WriteLine($"合計: {sum}");
            var one = groups.First(x => x.Key == 1);
            Console.WriteLine($"メソッドのうち {one.Count}個({(double)one.Count / sum * 100: 0.00}%) がステートメント1個だけ");
        }

        private static bool HasNoBody(MethodDeclarationSyntax m)
        {
            return m.Modifiers.Any(x => x.Text == "abstract" || x.Text == "partial");
        }

        private static IEnumerable<MethodDeclarationSyntax> GetMethods(Solution solution)
        {
            foreach (var project in solution.Projects)
            {
                foreach (var doc in project.Documents)
                {
                    var text = File.ReadAllText(doc.FilePath);
                    var tree = CSharpSyntaxTree.ParseText(SourceText.From(text));
                    var root = tree.GetRoot();

                    foreach (var cd in root.DescendantNodes().Where(n => n.Kind() == SyntaxKind.ClassDeclaration).Cast<ClassDeclarationSyntax>())
                    {
                        foreach (var m in cd.ChildNodes().Where(n => n.Kind() == SyntaxKind.MethodDeclaration).Cast<MethodDeclarationSyntax>())
                        {
                            yield return m;
                        }
                    }
                }
            }
        }

        private static IEnumerable<PropertyDeclarationSyntax> GetProperties(Solution solution)
        {
            foreach (var project in solution.Projects)
            {
                foreach (var doc in project.Documents)
                {
                    var text = File.ReadAllText(doc.FilePath);
                    var tree = CSharpSyntaxTree.ParseText(SourceText.From(text));
                    var root = tree.GetRoot();

                    foreach (var cd in root.DescendantNodes().Where(n => n.Kind() == SyntaxKind.ClassDeclaration).Cast<ClassDeclarationSyntax>())
                    {
                        foreach (var p in cd.ChildNodes().Where(n => n.Kind() == SyntaxKind.PropertyDeclaration).Cast<PropertyDeclarationSyntax>())
                        {
                            yield return p;
                        }
                    }
                }
            }
        }
    }
}
