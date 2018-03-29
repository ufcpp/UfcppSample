using System;
using System.Linq;
using static System.Reflection.BindingFlags;

namespace ConsoleApp1.BackingFieldAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    class MyAttribute : Attribute { }

    class MyClass
    {
        /// <summary>
        /// プロパティに対して field 指定で属性を付けると、それがバック フィールドに反映されるようになった。
        /// 今までは、書けたけど反映されず、無視されてた(警告だけは出る)。
        ///
        /// C# 7.2 以前、イベント構文の場合はこれが有効だったのに、なぜかプロパティでは反映されないという謎仕様。
        ///
        /// たぶん、<see cref="NonSerializedAttribute"/> とかはそれなりに使い道があるはず。
        /// この属性を付けたいがために自動プロパティにできない(↓みたいに書かないといけない)ことがちらほらあった。
        /// <code><![CDATA[
        /// [NonSerialized]
        /// private int _x;
        /// public int X { get => _x; set => _x = value; }
        /// ]]></code>
        ///
        /// 一応、破壊的変更ではある。
        /// 前述の通り今までは単に無視されてて、その場合、「AttributeTargets が Field ではない属性を付けてもエラーにならない」という状態だった。
        /// それが、正しく適用されるようになったことで、ちゃんと AttributeTargets の判定が入るようになった。
        /// そういう変なコードを書いてしまっていた人がいた場合、破壊的変更ではある。
        /// 心配するだけばかばかしいレベルだとは思うものの、一応。
        /// (ちなみに、この属性の field 指定、「使われることがない C# 機能」ランキングを作ったら5本の指に入るレベルのレア機能。)
        /// </summary>
        [field:My]
        public int X { get; set; }
    }

    class Program
    {
        static void Main()
        {
            var t = typeof(MyClass);
            var x = t.GetProperty("X");

            // 何も表示されないはず。
            Console.WriteLine("プロパティに付けた属性");
            foreach (var a in x.CustomAttributes)
            {
                Console.WriteLine(a.AttributeType.Name);
            }

            // こっちに My 属性が付いてるはず。
            // (その他、CompilerGenerated とかが勝手につくけど、これは自動プロパティの仕様)
            var backingField = t.GetFields(Instance | NonPublic).First();
            Console.WriteLine("そのプロパティのバック フィールドに付けた属性");
            foreach (var a in backingField.CustomAttributes)
            {
                Console.WriteLine(a.AttributeType.Name);
            }
        }
    }
}
