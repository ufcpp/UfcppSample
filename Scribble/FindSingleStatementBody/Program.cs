using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAnalysisForCSharp6
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
            Console.WriteLine("Build started");

            var s = await SolutionLoader.LoadAsync(path);

            FindGetOnlyPropertyWhichHasSingleStatement(s);
            FindMethodWhichHasSingleStatement(s);
        }

        private static void FindGetOnlyPropertyWhichHasSingleStatement(SolutionLoader solution)
        {
            var items = (
                from p in solution.GetProperties()
                where p.HasBody
                select p
                ).ToArray();

            var total = 0;
            var getOnly = 0;
            var singleStatement = 0;
            var fieldAccess = 0;
            var auto = 0;
            var privateSet = 0;

            foreach (var x in items)
            {
                total++;
                if (x.IsGetOnly) getOnly++;
                if (x.HasSingleStatement) singleStatement++;
                if (x.BackingField != null) fieldAccess++;
                if (x.IsAuto) auto++;
                if (x.IsAuto && x.HasPrivateSet) privateSet++;
            }

            Console.WriteLine($"total: {total}, get-only: {getOnly}, single stetement: {singleStatement}, field access: {fieldAccess}, auto: {auto}, private set: {privateSet}");
            Console.WriteLine($"プロパティ全体の {singleStatement}個({(double)singleStatement / total * 100: 0.00}%) がステートメント1個だけ");
            Console.WriteLine($"プロパティ全体の {(double)singleStatement / total * 100: 0.00}%");

            Console.WriteLine($"get だけのプロパティのうち {(double)singleStatement / getOnly * 100: 0.00}% が式1個だけ");
            Console.WriteLine($"式1つだけプロパティのうち {(double)fieldAccess / singleStatement * 100: 0.00}% が単なるフィールド アクセス");

            Console.WriteLine($"プロパティ全体の {auto}個({(double)auto / total * 100: 0.00}%) が自動実装");
            Console.WriteLine($"自動プロパティのうち {privateSet}個({(double)privateSet / auto * 100: 0.00}%) が private set");
        }

        private static bool HasNoBody(PropertyDeclarationSyntax p)
        {
            return p.Modifiers.Any(x => x.Text == "abstract");
        }

        private static void FindMethodWhichHasSingleStatement(SolutionLoader solution)
        {
            var groups = (
                from m in solution.GetMethods()
                where m.HasBody
                group m by m.StatementCount into g
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
