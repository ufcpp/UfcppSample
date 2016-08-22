using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuples
{
    class Sample
    {
        private (int x, int y) value;
        public (int x, int y) GetValue() => value;

        public Sample() { }
        public Sample(int x, int y)
        {
            value = (x, y);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MemberAccess();
            Ref();
            DifferentNames();
            TypeParameters();
        }

        private static void LocalDenotation()
        {
            var s = new Sample();
            (int x, int y) t = s.GetValue();
        }

        private static void TypeInference()
        {
            var s = new Sample();
            var t = s.GetValue();
        }

        static void New()
        {
            // var t = new T(1, 2); みたいなのと同じノリ
            var t1 = new (int x, int y) (1, 2);

            // var t = new T { x = 1, y = 2}; みたいなのと同じノリ
            var t2 = new (int x, int y) { x = 1, y = 2 };
        }

        static void Deconstruct()
        {
            var t = (x: 1, y: 2);

            // 分解宣言1
            (int x1, int y1) = t; // x1, y1 を宣言しつつ、ｔ を分解
            // 分解宣言2
            var (x2, y2) = t; // 分解宣言の簡易記法

            // 分解代入
            int x, y;
            (x, y) = t; // 分解結果を既存の変数に代入
        }

        static void Literal()
        {
            // メソッド呼び出し時の F(1, 2); みたいなノリ
            (int x, int y) t1 = (1, 2);

            // メソッド呼び出し時の F(x: 1, y: 2); みたいなノリ
            var t2 = (x: 1, y: 2);
        }

        private static void MemberTypeInference()
        {
            // これは左辺から型推論が聞くので、null も書ける
            (string s, int i) t1 = (null, 1);

#if false
            // これはダメ。null の型が決まらない。
            var t2 = (null, 1);
#endif
        }

        static void MemberAccess()
        {
            var t = (x: 1, y: 2);
            Console.WriteLine(t.x); // 1
            Console.WriteLine(t.y); // 2

            // メンバーごとに書き換え可能
            t.x = 10;
            t.y = 20;
            Console.WriteLine(t.x); // 10
            Console.WriteLine(t.y); // 20

            // タプル自身も書き換え可能
            t = (100, 200);
            Console.WriteLine(t.x); // 100
            Console.WriteLine(t.y); // 200
        }

        static void Ref()
        {
            var t = (x: 1, y: 2);
            Swap(ref t.x, ref t.y);
            Console.WriteLine(t.x); // 2
            Console.WriteLine(t.y); // 1
        }

        static void Swap<T>(ref T x, ref T y)
        {
            var t = x;
            x = y;
            y = t;
        }

        static void DifferentNames()
        {
            (int s, int t) t1 = (x: 1, y: 2);
            Console.WriteLine(t1.s); // 1
            Console.WriteLine(t1.t); // 2

            (int y, int x) t2 = (x: 1, y: 2);
            Console.WriteLine(t2.x); // 2
            Console.WriteLine(t2.y); // 1
        }

        static void DifferentTypes()
        {
            {
                object x = "abc"; // string → object は OK
                long y = 1; // int → long は OK
                int? z = 2; // int → int? は OK
                // ↓
                (object x, long y, int? z) t = ("abc", 1, 2); // OK
            }
#if false
            {
                string x = 1; // int → string は NG
                int y = 1L; // long → int は NG
                int z = default(int?); // int? → int は NG
                // ↓
                (string x, int y, int z) t = (1, 1L, default(int?)); // NG
            }
#endif
        }

        static void NestedTuples()
        {
            // タプルの入れ子
            (string a, (int x, int y) b) t1 = ("abc", (1, 2));
            Console.WriteLine(t1.a);   // abc
            Console.WriteLine(t1.b.x); // 1
            Console.WriteLine(t1.b.y); // 2

            // 型推論も可能
            var t2 = (a: "abc", b: (x: 1, y: 2));
        }

        static void TypeParameters()
        {
            var dic = new Dictionary<(string s, string t), (int x, int y)>
            {
                { ("a", "b"), (1, 2) },
                { ("x", "y"), (4, 8) },
            };

            Console.WriteLine(dic[("a", "b")]); // (1, 2)
        }

        static void AnonymousMember()
        {
            var t1 = (1, 2);
            Console.WriteLine(t1.Item1); // 1
            Console.WriteLine(t1.Item2); // 2

            //var t2 = (x: 1, 2); // 次のプレビューにはこれできるようになるはず
        }
    }
}
