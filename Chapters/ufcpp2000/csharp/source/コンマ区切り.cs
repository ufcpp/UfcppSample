using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace コンマ区切り
{
    class Program
    {
        static void Main(string[] args)
        {
            var samples = new[]
            {
                new SampleData { Name = "ALice", Age = 15, Sex = Sex.Female },
                new SampleData { Name = "Bob", Age = 20, Sex = Sex.Male },
                new SampleData { Name = "Carol", Age = 12, Sex = Sex.Female },
            };

            File.WriteAllLines("sample.csv", samples.ToCommaSeparatedLines());
        }
    }

    /// <summary>
    /// サンプル用。
    /// 性別。
    /// </summary>
    public enum Sex
    {
        NotKnown,
        Male,
        Female,
        NotApplicable = 9,
    }

    /// <summary>
    /// サンプル用のデータ。
    /// とりあえず、よくある名簿用の人クラス。
    /// </summary>
    class SampleData
    {
        [Display(Order = 0)]
        public string Name { get; set; }

        [Display(Order = 1)]
        public int Age { get; set; }

        [Display(Order = 2)]
        public Sex Sex { get; set; }
    }

    public static partial class Extensions
    {
        /// <summary>
        /// 任意の型を与えてコンマ区切りの行を作る。
        /// 1列1プロパティ。
        /// 列は、Display 属性の Order 順に並べる。
        /// </summary>
        /// <typeparam name="T">コンマ区切り行化したい型。</typeparam>
        /// <param name="obj">変換対象。</param>
        /// <returns>コンマ区切り列。</returns>
        public static string ToCommaSeparatedLine<T>(this T obj)
        {
            var props =
                from p in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                let orderAttribute = p.GetCustomAttributes(typeof(DisplayAttribute), false).OfType<DisplayAttribute>().FirstOrDefault()
                let order = orderAttribute != null ? orderAttribute.Order : int.MaxValue
                orderby order
                select p;

            return string.Join(",", props.Select(p => p.GetValue(obj, null)));
        }

        /// <summary>
        /// 複数のオブジェクトをコンマ区切り化。
        /// </summary>
        /// <typeparam name="T">コンマ区切り行化したい型。</typeparam>
        /// <param name="array">変換対象</param>
        /// <returns>コンマ区切り列（複数行）。</returns>
        public static IEnumerable<string> ToCommaSeparatedLines<T>(this IEnumerable<T> array)
        {
            foreach (var x in array)
            {
                yield return x.ToCommaSeparatedLine();
            }
        }
    }
}
