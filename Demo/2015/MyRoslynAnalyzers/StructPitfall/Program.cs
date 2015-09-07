using System;

namespace StructPitfall
{
    struct Counter
    {
        public int Value { get; private set; }

        public void Count() => Value++;

        public void Reset() => Value = 0;
    }

    class Program
    {
        private static Counter field;

        private readonly static Counter readonlyField;

        private static Counter Property => field;

        static void Main(string[] args)
        {
            // フィールド直利用
            for (int i = 0; i < 3; i++)
                field.Count();

            Console.WriteLine(field.Value); // 3

            // フィールド、いったんリセット
            field.Reset();

            // ローカル変数で受ける
            var local = field;
            for (int i = 0; i < 3; i++)
                local.Count();

            Console.WriteLine(field.Value); // 0 のまま
            Console.WriteLine(local.Value); // 3

            // プロパティごし
            for (int i = 0; i < 3; i++)
                Property.Count(); // プロパティ越しなので、コピーが作られてる

            Console.WriteLine(field.Value); // 0 のまま
            Console.WriteLine(Property.Value); // 0 のまま(の field を改めてコピー)

            // readonly フィールド直利用
            for (int i = 0; i < 3; i++)
                readonlyField.Count(); // 実はここでいったんコピーが作られてる

            Console.WriteLine(readonlyField.Value); // 0 のまま

/*
結果:

3
0
3
0
0
0
*/
        }
    }
}
