namespace Patterns.PropertyPatternField
{
    using System;

    class X
    {
        // (外から見て) get-only なプロパティ
        public int GetOnly { get; private set; }

        // get/set 可能なプロパティ
        public int GetSet { get; set; }

        // フィールド
        public int Field;

        // set-only なプロパティ
        public int SetOnly { set => GetOnly = value; }
    }

    class Program
    {
        public static void Main()
        {
            // オブジェクト初期化子では、set が public なプロパティか readonly ではないフィールドを指定可能
            var x = new X { GetSet = 1, Field = 2, SetOnly = 3 };

            // プロパティ パターンでは、get が public なプロパティかフィールドを指定可能
            Console.WriteLine(x is { GetOnly: 3, GetSet: 1, Field: 2 });
        }
    }
}
