namespace Tuples.AnonymousTypes
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// 性別
    /// </summary>
    enum Sex
    {
        Male,
        Female,
    }

    /// <summary>
    /// 個人情報
    /// </summary>
    class Person
    {
        public int Id { get; }
        public string Name { get; }
        public string Kana { get; }
        public Sex Sex { get; }
        public string PhoneNumber { get; }
        public DateTime BirthDay { get; }

        public Person(int id, string name, string kana, Sex sex, string phoneNumber, DateTime birthDay)
        {
            Id = id;
            Name = name;
            Kana = kana;
            Sex = sex;
            PhoneNumber = phoneNumber;
            BirthDay = birthDay;
        }
    }

    class Program
    {
        static void Main()
        {
            var persons = ReadAll("personal_infomation.csv").ToArray();

            // 性別・年代(10年区切り)ごとに何人いるかを集計
            var histgram = persons
                .GroupBy(p => new { p.Sex, BirthDecade = p.BirthDay.Year / 10 })
                .Select(g => new { g.Key.Sex, g.Key.BirthDecade, Count = g.Count() })
                .OrderBy(x => x.BirthDecade)
                .ThenBy(x => x.Sex);

            foreach (var item in histgram)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// CSV から <see cref="Person"/> を読み込み。
        /// </summary>
        /// <param name="csvFilename"></param>
        /// <returns></returns>
        static IEnumerable<Person> ReadAll(string csvFilename)
        {
            foreach (var line in File.ReadLines(csvFilename))
            {
                var items = line.Split(',');
                yield return new Person(
                    id: int.Parse(items[0]),
                    name: items[1],
                    kana: items[2],
                    sex: items[3] == "男" ? Sex.Male : Sex.Female,
                    phoneNumber: items[4],
                    birthDay: DateTime.Parse(items[5])
                    );
            }
        }
    }
}
