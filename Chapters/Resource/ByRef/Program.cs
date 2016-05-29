using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByRef
{
    class Program
    {
        static void Main(string[] args)
        {
            VariableToVariable();
        }

        private static void VariableToVariable()
        {
            var x = 1;
            var y = x;
        }

        static void VariableToParameter()
        {
            var x = 1;
            F(x); // 変数 x から、F の引数 x に値を渡す
        }

        static void F(int x)
        {
        }

        static void ReturnToVariable()
        {
            var x = F(); // F の戻り値から変数 x に値を渡す
        }

        static int F() => 1;
    }
}
