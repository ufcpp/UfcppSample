using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentifierScope.Member
{
    public class Sample
    {
        int x = 20;

        public void M()
        {
            int x = 10;

            Console.WriteLine(x);      // ローカル変数の方の x = 10
            Console.WriteLine(this.x); // フィールドの方の x = 20
        }
    }
}
