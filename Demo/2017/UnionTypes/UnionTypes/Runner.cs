using System;

namespace UnionTypes
{
    partial class Runner
    {

        public static void Run()
        {
            var r = new Random();

            var t1 = TimeSpan.Zero;
            var t2 = TimeSpan.Zero;
            var t3 = TimeSpan.Zero;
            var t4 = TimeSpan.Zero;
            var t5 = TimeSpan.Zero;

            for (int i = 0; i < 3; i++)
            {
                foreach (var rate in new[] { 0.1, 0.3, 0.5, 0.7, 0.9 })
                {
                    //Console.WriteLine(rate);

                    t1 += Run<int, Union1<int>>("rnd 1 int", rate, r, Generator.Random_int_1);
                    t1 += Run<int, Union1<int>>("fix 1 int", rate, r, Generator.Fix_int_1);
                    t1 += Run<Vector, Union1<Vector>>("rnd 1 Vec", rate, r, Generator.Random_Vector_1);
                    t1 += Run<Vector, Union1<Vector>>("fix 1 Vec", rate, r, Generator.Fix_Vector_1);
                    t1 += Run<string, Union1<string>>("rnd 1 str", rate, r, Generator.Random_string_1);
                    t1 += Run<string, Union1<string>>("fix 1 str", rate, r, Generator.Fix_string_1);
                    t2 += Run<int, Union2<int>>("rnd 2 int", rate, r, Generator.Random_int_2);
                    t2 += Run<int, Union2<int>>("fix 2 int", rate, r, Generator.Fix_int_2);
                    t2 += Run<Vector, Union2<Vector>>("rnd 2 Vec", rate, r, Generator.Random_Vector_2);
                    t2 += Run<Vector, Union2<Vector>>("fix 2 Vec", rate, r, Generator.Fix_Vector_2);
                    t2 += Run<string, Union2<string>>("rnd 2 str", rate, r, Generator.Random_string_2);
                    t2 += Run<string, Union2<string>>("fix 2 str", rate, r, Generator.Fix_string_2);
                    t3 += Run<int, Union3<int>>("rnd 3 int", rate, r, Generator.Random_int_3);
                    t3 += Run<int, Union3<int>>("fix 3 int", rate, r, Generator.Fix_int_3);
                    t3 += Run<Vector, Union3<Vector>>("rnd 3 Vec", rate, r, Generator.Random_Vector_3);
                    t3 += Run<Vector, Union3<Vector>>("fix 3 Vec", rate, r, Generator.Fix_Vector_3);
                    t3 += Run<string, Union3<string>>("rnd 3 str", rate, r, Generator.Random_string_3);
                    t3 += Run<string, Union3<string>>("fix 3 str", rate, r, Generator.Fix_string_3);
                    t4 += Run<int, Union4<int>>("rnd 4 int", rate, r, Generator.Random_int_4);
                    t4 += Run<int, Union4<int>>("fix 4 int", rate, r, Generator.Fix_int_4);
                    t4 += Run<Vector, Union4<Vector>>("rnd 4 Vec", rate, r, Generator.Random_Vector_4);
                    t4 += Run<Vector, Union4<Vector>>("fix 4 Vec", rate, r, Generator.Fix_Vector_4);
                    t4 += Run<string, Union4<string>>("rnd 4 str", rate, r, Generator.Random_string_4);
                    t4 += Run<string, Union4<string>>("fix 4 str", rate, r, Generator.Fix_string_4);
                    t5 += Run<int, Union5<int>>("rnd 5 int", rate, r, Generator.Random_int_5);
                    t5 += Run<int, Union5<int>>("fix 5 int", rate, r, Generator.Fix_int_5);
                    t5 += Run<Vector, Union5<Vector>>("rnd 5 Vec", rate, r, Generator.Random_Vector_5);
                    t5 += Run<Vector, Union5<Vector>>("fix 5 Vec", rate, r, Generator.Fix_Vector_5);
                    t5 += Run<string, Union5<string>>("rnd 5 str", rate, r, Generator.Random_string_5);
                    t5 += Run<string, Union5<string>>("fix 5 str", rate, r, Generator.Fix_string_5);
                }
            }

            Console.WriteLine("Union1: " + t1);
            Console.WriteLine("Union2: " + t2);
            Console.WriteLine("Union3: " + t3);
            Console.WriteLine("Union4: " + t4);
            Console.WriteLine("Union5: " + t5);
        }
    }
}
