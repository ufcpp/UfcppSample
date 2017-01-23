using System;

namespace UnionTypes
{
    partial class Generator
    {
        static int value_int = 0;
        static int[] array_int = new[] { 1, 2, 3, 4, 5 };
        static int Random_int(Random r) => r.Next();

        static Vector value_Vector = default(Vector);
        static Vector[] array_Vector = new[] { default(Vector), default(Vector), default(Vector), default(Vector), default(Vector) };
        static Vector Random_Vector(Random r) => new Vector(r.Next(), r.Next(), r.Next());

        static string value_string = "zero";
        static string[] array_string = new[] { "one", "two", "three", "four", "five" };
        static string Random_string(Random r)
        {
            switch (r.Next(0, 5))
            {
                default: return "zero";
                case 1: return "one";
                case 2: return "two";
                case 3: return "thre";
                case 4: return "four";
            }
        }

        static Union FixUnion<T, Union>(Random r, double arrayRate, T v, T[] a, Func<T, Union> value, Func<T[], Union> array)
            where Union : IUnion<T>
        {
            if (r.NextDouble() < arrayRate) return array(a);
            else return value(v);
        }

        static Union RandomUnion<T, Union>(Random r, double arrayRate, Func<Random, T> generator, Func<T, Union> value, Func<T[], Union> array)
            where Union : IUnion<T>
        {
            if (r.NextDouble() < arrayRate) return array(RandomArray(r, generator));
            else return value(generator(r));
        }

        static T[] RandomArray<T>(Random r, Func<Random, T> generator)
        {
            var array = new T[r.Next(1, 5)];
            for (int i = 0; i < array.Length; i++) array[i] = generator(r);
            return array;
        }
    }
}
