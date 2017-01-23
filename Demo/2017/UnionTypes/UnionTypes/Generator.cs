using System;

namespace UnionTypes
{
    partial class Generator
    {
        public static Union1<int> Random_int_1(Random r, double arrayRate) => RandomUnion(r, arrayRate, Random_int, Union1.New, Union1.New);
        public static Union1<Vector> Random_Vector_1(Random r, double arrayRate) => RandomUnion(r, arrayRate, Random_Vector, Union1.New, Union1.New);
        public static Union1<string> Random_string_1(Random r, double arrayRate) => RandomUnion(r, arrayRate, Random_string, Union1.New, Union1.New);
        public static Union2<int> Random_int_2(Random r, double arrayRate) => RandomUnion(r, arrayRate, Random_int, Union2.New, Union2.New);
        public static Union2<Vector> Random_Vector_2(Random r, double arrayRate) => RandomUnion(r, arrayRate, Random_Vector, Union2.New, Union2.New);
        public static Union2<string> Random_string_2(Random r, double arrayRate) => RandomUnion(r, arrayRate, Random_string, Union2.New, Union2.New);
        public static Union3<int> Random_int_3(Random r, double arrayRate) => RandomUnion(r, arrayRate, Random_int, Union3.New, Union3.New);
        public static Union3<Vector> Random_Vector_3(Random r, double arrayRate) => RandomUnion(r, arrayRate, Random_Vector, Union3.New, Union3.New);
        public static Union3<string> Random_string_3(Random r, double arrayRate) => RandomUnion(r, arrayRate, Random_string, Union3.New, Union3.New);
        public static Union4<int> Random_int_4(Random r, double arrayRate) => RandomUnion(r, arrayRate, Random_int, Union4.New, Union4.New);
        public static Union4<Vector> Random_Vector_4(Random r, double arrayRate) => RandomUnion(r, arrayRate, Random_Vector, Union4.New, Union4.New);
        public static Union4<string> Random_string_4(Random r, double arrayRate) => RandomUnion(r, arrayRate, Random_string, Union4.New, Union4.New);
        public static Union5<int> Random_int_5(Random r, double arrayRate) => RandomUnion(r, arrayRate, Random_int, Union5.New, Union5.New);
        public static Union5<Vector> Random_Vector_5(Random r, double arrayRate) => RandomUnion(r, arrayRate, Random_Vector, Union5.New, Union5.New);
        public static Union5<string> Random_string_5(Random r, double arrayRate) => RandomUnion(r, arrayRate, Random_string, Union5.New, Union5.New);

        public static Union1<int> Fix_int_1(Random r, double arrayRate) => FixUnion(r, arrayRate, value_int, array_int, Union1.New, Union1.New);
        public static Union1<Vector> Fix_Vector_1(Random r, double arrayRate) => FixUnion(r, arrayRate, value_Vector, array_Vector, Union1.New, Union1.New);
        public static Union1<string> Fix_string_1(Random r, double arrayRate) => FixUnion(r, arrayRate, value_string, array_string, Union1.New, Union1.New);
        public static Union2<int> Fix_int_2(Random r, double arrayRate) => FixUnion(r, arrayRate, value_int, array_int, Union2.New, Union2.New);
        public static Union2<Vector> Fix_Vector_2(Random r, double arrayRate) => FixUnion(r, arrayRate, value_Vector, array_Vector, Union2.New, Union2.New);
        public static Union2<string> Fix_string_2(Random r, double arrayRate) => FixUnion(r, arrayRate, value_string, array_string, Union2.New, Union2.New);
        public static Union3<int> Fix_int_3(Random r, double arrayRate) => FixUnion(r, arrayRate, value_int, array_int, Union3.New, Union3.New);
        public static Union3<Vector> Fix_Vector_3(Random r, double arrayRate) => FixUnion(r, arrayRate, value_Vector, array_Vector, Union3.New, Union3.New);
        public static Union3<string> Fix_string_3(Random r, double arrayRate) => FixUnion(r, arrayRate, value_string, array_string, Union3.New, Union3.New);
        public static Union4<int> Fix_int_4(Random r, double arrayRate) => FixUnion(r, arrayRate, value_int, array_int, Union4.New, Union4.New);
        public static Union4<Vector> Fix_Vector_4(Random r, double arrayRate) => FixUnion(r, arrayRate, value_Vector, array_Vector, Union4.New, Union4.New);
        public static Union4<string> Fix_string_4(Random r, double arrayRate) => FixUnion(r, arrayRate, value_string, array_string, Union4.New, Union4.New);
        public static Union5<int> Fix_int_5(Random r, double arrayRate) => FixUnion(r, arrayRate, value_int, array_int, Union5.New, Union5.New);
        public static Union5<Vector> Fix_Vector_5(Random r, double arrayRate) => FixUnion(r, arrayRate, value_Vector, array_Vector, Union5.New, Union5.New);
        public static Union5<string> Fix_string_5(Random r, double arrayRate) => FixUnion(r, arrayRate, value_string, array_string, Union5.New, Union5.New);
    }
}
