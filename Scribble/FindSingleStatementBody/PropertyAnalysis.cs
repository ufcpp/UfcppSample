using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalysisForCSharp6
{
    /// <summary>
    /// プロパティ関連の解析。
    /// </summary>
    static class PropertyAnalysis
    {
        public static IEnumerable<PropertySyntaxInfo> GetProperties(this SolutionLoader s)
        {
            foreach (var tree in s.SyntaxTrees)
            {
                var root = tree.GetRoot();

                foreach (var cd in root.DescendantNodes().Where(n => n.Kind() == SyntaxKind.ClassDeclaration).Cast<ClassDeclarationSyntax>())
                {
                    foreach (var p in cd.ChildNodes().Where(n => n.Kind() == SyntaxKind.PropertyDeclaration).Cast<PropertyDeclarationSyntax>())
                    {
                        yield return new PropertySyntaxInfo(tree, p);
                    }
                }
            }
        }
    }

    /// <summary>
    /// プロパティの構文解析結果。
    /// </summary>
    class PropertySyntaxInfo
    {
        public SyntaxTree Tree { get; }
        public PropertyDeclarationSyntax Property { get; }

        public PropertySyntaxInfo(SyntaxTree tree, PropertyDeclarationSyntax property)
        {
            Tree = tree;
            Property = property;
            HasBody = property.Modifiers.All(x => x.Text != "abstract");

            var hasOnlyOneAccessor = property.AccessorList.Accessors.Count == 1;
            var getter = property.AccessorList.Accessors.SingleOrDefault(a => a.Keyword.Text == "get");
            var setter = property.AccessorList.Accessors.SingleOrDefault(a => a.Keyword.Text == "set");

            IsAuto = (getter != null && getter.Body == null) && (setter != null && setter.Body == null);
            HasPrivateSet = setter != null && setter.Modifiers.Any(x => x.Text == "private");

            IsGetOnly = hasOnlyOneAccessor && getter != null;
            HasSingleStatement = IsGetOnly && getter != null && getter.Body != null && getter.Body.Statements.Count == 1;

            if (HasSingleStatement)
            {
                var statement = getter.Body.Statements[0];

                var ret = statement as ReturnStatementSyntax;
                if (ret != null)
                {
                    var id = ret.Expression as IdentifierNameSyntax;

                    if (id != null)
                    {
                        BackingField = tree.GetRoot().DescendantNodes()
                            .Select(x => x as FieldDeclarationSyntax)
                            .Where(x => x != null)
                            .Select(x => x.Declaration.Variables[0].Identifier.Text)
                            .FirstOrDefault(name => name == id.Identifier.Text);
                    }
                }
            }
        }

        /// <summary>
        /// ボディを持つ(= abstract ではない)。
        /// </summary>
        public bool HasBody { get; }

        /// <summary>
        /// getter 1つだけを持つ。
        /// </summary>
        public bool IsGetOnly { get; }

        /// <summary>
        /// <see cref="IsGetOnly"/> かつ getter の中身が式1つだけ。
        /// </summary>
        public bool HasSingleStatement { get; }

        /// <summary>
        /// <see cref="HasSingleStatement"/> かつ その1文がバック フィールド参照のとき、その識別子名。
        /// 条件を満たさないときは null。
        /// </summary>
        public string BackingField { get; }

        /// <summary>
        /// 自動プロパティ。
        /// </summary>
        public bool IsAuto { get; }

        /// <summary>
        /// setter が private。
        /// </summary>
        public bool HasPrivateSet { get; }
    }
}
