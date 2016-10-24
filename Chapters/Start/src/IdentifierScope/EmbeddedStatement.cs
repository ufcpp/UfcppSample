namespace IdentifierScope.EmbeddedStatement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    class Program
    {
        static void DeclarationInEmbeddedStatement()
        {
            if (true)
                int x = 10; // コンパイル エラー

            if (true)
            {
                int x = 10; // これなら OK
            }

            foreach (var n in new[] { 1 })
                int x = 10; // コンパイル エラー

            foreach (var n in new[] { 1 })
            {
                int x = 10; // これなら OK
            }
        }

        static void DeclarationExpressions(object obj)
        {
            if (obj is string s)
            {

            }

            if (true)
            {
                int x = 10; // これなら OK
            }

            foreach (var n in new[] { 1 })
                int x = 10; // コンパイル エラー

            foreach (var n in new[] { 1 })
            {
                int x = 10; // これなら OK
            }
        }
    }
}
