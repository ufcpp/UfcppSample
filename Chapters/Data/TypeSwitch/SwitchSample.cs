using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeSwitch
{
    class SwitchSample
    {
        static void F(object obj)
        {
            switch (obj)
            {
                case string s:
                    Console.WriteLine("string #" + s.Length);
                    break;
                case 7:
                    Console.WriteLine("7の時だけここに来る");
                    break;
                case int n when n > 0:
                    Console.WriteLine("正の数の時にここに来る " + n);
                    // ただし、上から順に判定するので、7 の時には来なくなる
                    break;
                case int n:
                    Console.WriteLine("整数の時にここに来る" + n);
                    // 同上、0 以下の時にしか来ない
                    break;
                default:
                    Console.WriteLine("その他");
                    break;
            }
        }

        static void TypeSwitch(object obj)
        {
            switch (obj)
            {
                case int n:
                    Console.WriteLine("整数 " + n);
                    break;
                case string s:
                    Console.WriteLine("文字列 " + s);
                    break;
                default:
                    Console.WriteLine("その他");
                    break;
            }
        }

        static string 値だけでswitch(int n)
        {
            switch(n)
            {
                case 0: return "zero";
                case 1: return "one";
                case 2: return "two";
                case 3: return "three";
                case 4: return "four";
                case 5: return "five";
                case 6: return "six";
                case 7: return "seven";
                case 8: return "eight";
                case 9: return "nine";
                default: return "other";
            }
        }

        static string ジャンプテーブル化(int n)
        {
            var map = new Dictionary<int, string>
            {
                { 0, "zero" },
                { 1, "one" },
                { 2, "two" },
                { 3, "three" },
                { 4, "four" },
                { 5, "five" },
                { 6, "six" },
                { 7, "seven" },
                { 8, "eight" },
                { 9, "nine" },
            };

            string s;
            if (map.TryGetValue(n, out s)) return s;
            else return "other";
        }
    }
}
