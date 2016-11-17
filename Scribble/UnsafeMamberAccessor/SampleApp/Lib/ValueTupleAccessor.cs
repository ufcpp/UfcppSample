using System;

namespace SampleApp.Lib
{
    public static class ValueTupleAccessor
    {
        public static TypedPointer GetPointer<T1>(ref ValueTuple<T1> v, int index)
        {
            switch (index)
            {
                case 0: return TypedPointer.Create(ref v.Item1);
                default: throw new IndexOutOfRangeException();
            }
        }

        public static TypedPointer GetPointer<T1, T2>(ref ValueTuple<T1, T2> v, int index)
        {
            switch (index)
            {
                case 0: return TypedPointer.Create(ref v.Item1);
                case 1: return TypedPointer.Create(ref v.Item2);
                default: throw new IndexOutOfRangeException();
            }
        }
        public static TypedPointer GetPointer<T1, T2, T3>(ref ValueTuple<T1, T2, T3> v, int index)
        {
            switch (index)
            {
                case 0: return TypedPointer.Create(ref v.Item1);
                case 1: return TypedPointer.Create(ref v.Item2);
                case 2: return TypedPointer.Create(ref v.Item3);
                default: throw new IndexOutOfRangeException();
            }
        }
        public static TypedPointer GetPointer<T1, T2, T3, T4>(ref ValueTuple<T1, T2, T3, T4> v, int index)
        {
            switch (index)
            {
                case 0: return TypedPointer.Create(ref v.Item1);
                case 1: return TypedPointer.Create(ref v.Item2);
                case 2: return TypedPointer.Create(ref v.Item3);
                case 3: return TypedPointer.Create(ref v.Item4);
                default: throw new IndexOutOfRangeException();
            }
        }
        public static TypedPointer GetPointer<T1, T2, T3, T4, T5>(ref ValueTuple<T1, T2, T3, T4, T5> v, int index)
        {
            switch (index)
            {
                case 0: return TypedPointer.Create(ref v.Item1);
                case 1: return TypedPointer.Create(ref v.Item2);
                case 2: return TypedPointer.Create(ref v.Item3);
                case 3: return TypedPointer.Create(ref v.Item4);
                case 4: return TypedPointer.Create(ref v.Item5);
                default: throw new IndexOutOfRangeException();
            }
        }
        public static TypedPointer GetPointer<T1, T2, T3, T4, T5, T6>(ref ValueTuple<T1, T2, T3, T4, T5, T6> v, int index)
        {
            switch (index)
            {
                case 0: return TypedPointer.Create(ref v.Item1);
                case 1: return TypedPointer.Create(ref v.Item2);
                case 2: return TypedPointer.Create(ref v.Item3);
                case 3: return TypedPointer.Create(ref v.Item4);
                case 4: return TypedPointer.Create(ref v.Item5);
                case 5: return TypedPointer.Create(ref v.Item6);
                default: throw new IndexOutOfRangeException();
            }
        }
        public static TypedPointer GetPointer<T1, T2, T3, T4, T5, T6, T7>(ref ValueTuple<T1, T2, T3, T4, T5, T6, T7> v, int index)
        {
            switch (index)
            {
                case 0: return TypedPointer.Create(ref v.Item1);
                case 1: return TypedPointer.Create(ref v.Item2);
                case 2: return TypedPointer.Create(ref v.Item3);
                case 3: return TypedPointer.Create(ref v.Item4);
                case 4: return TypedPointer.Create(ref v.Item5);
                case 5: return TypedPointer.Create(ref v.Item6);
                case 6: return TypedPointer.Create(ref v.Item7);
                default: throw new IndexOutOfRangeException();
            }
        }

        //todo: 今、TRest の再帰がうまく扱えてない。
        //{
        //    switch (index)
        //    {
        //        case 0: return TypedPointer.Create(ref v.Item1);
        //        case 1: return TypedPointer.Create(ref v.Item2);
        //        case 2: return TypedPointer.Create(ref v.Item3);
        //        case 3: return TypedPointer.Create(ref v.Item4);
        //        case 4: return TypedPointer.Create(ref v.Item5);
        //        case 5: return TypedPointer.Create(ref v.Item6);
        //        case 6: return TypedPointer.Create(ref v.Item7);
        //        default:
        //            if(_value.Rest is IMemberAccessor) return 
        //            throw new IndexOutOfRangeException();
        //    }
        //}
    }
}
