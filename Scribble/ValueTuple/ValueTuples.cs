using System.Collections.Generic;
using static ValueTuples.ValueTuple;

namespace ValueTuples
{
    public static partial class ValueTuple
    {
        public static ValueTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2) => new ValueTuple<T1, T2>(item1, item2);
        public static ValueTuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) => new ValueTuple<T1, T2, T3>(item1, item2, item3);
        public static ValueTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) => new ValueTuple<T1, T2, T3, T4>(item1, item2, item3, item4);
        public static ValueTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) => new ValueTuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
        public static ValueTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) => new ValueTuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(item1, item2, item3, item4, item5, item6, item7, item8);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(item1, item2, item3, item4, item5, item6, item7, item8, item9);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15, item16);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15, item16, item17);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17, T18 item18) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15, item16, item17, item18);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17, T18 item18, T19 item19) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15, item16, item17, item18, item19);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17, T18 item18, T19 item19, T20 item20) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15, item16, item17, item18, item19, item20);
    }

    public struct ValueTuple<T1, T2> : ITuple
    {
        public T1 Item1;
        public T2 Item2;

        public ValueTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;

        public ValueTuple(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5) + Count(ref Item6);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
                { var x = Item6 as ITuple; if (x == null) yield return Item6; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                if (Get(ref Item6, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
                if (Set(ref Item6, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5) + Count(ref Item6) + Count(ref Item7);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
                { var x = Item6 as ITuple; if (x == null) yield return Item6; else foreach (var y in x.Values) yield return y; }
                { var x = Item7 as ITuple; if (x == null) yield return Item7; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                if (Get(ref Item6, ref index, out output)) return output;
                if (Get(ref Item7, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
                if (Set(ref Item6, ref index, value)) return;
                if (Set(ref Item7, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;
        public T8 Item8;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5) + Count(ref Item6) + Count(ref Item7) + Count(ref Item8);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
                { var x = Item6 as ITuple; if (x == null) yield return Item6; else foreach (var y in x.Values) yield return y; }
                { var x = Item7 as ITuple; if (x == null) yield return Item7; else foreach (var y in x.Values) yield return y; }
                { var x = Item8 as ITuple; if (x == null) yield return Item8; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                if (Get(ref Item6, ref index, out output)) return output;
                if (Get(ref Item7, ref index, out output)) return output;
                if (Get(ref Item8, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
                if (Set(ref Item6, ref index, value)) return;
                if (Set(ref Item7, ref index, value)) return;
                if (Set(ref Item8, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;
        public T8 Item8;
        public T9 Item9;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
            Item9 = item9;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5) + Count(ref Item6) + Count(ref Item7) + Count(ref Item8) + Count(ref Item9);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
                { var x = Item6 as ITuple; if (x == null) yield return Item6; else foreach (var y in x.Values) yield return y; }
                { var x = Item7 as ITuple; if (x == null) yield return Item7; else foreach (var y in x.Values) yield return y; }
                { var x = Item8 as ITuple; if (x == null) yield return Item8; else foreach (var y in x.Values) yield return y; }
                { var x = Item9 as ITuple; if (x == null) yield return Item9; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                if (Get(ref Item6, ref index, out output)) return output;
                if (Get(ref Item7, ref index, out output)) return output;
                if (Get(ref Item8, ref index, out output)) return output;
                if (Get(ref Item9, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
                if (Set(ref Item6, ref index, value)) return;
                if (Set(ref Item7, ref index, value)) return;
                if (Set(ref Item8, ref index, value)) return;
                if (Set(ref Item9, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;
        public T8 Item8;
        public T9 Item9;
        public T10 Item10;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
            Item9 = item9;
            Item10 = item10;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5) + Count(ref Item6) + Count(ref Item7) + Count(ref Item8) + Count(ref Item9) + Count(ref Item10);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
                { var x = Item6 as ITuple; if (x == null) yield return Item6; else foreach (var y in x.Values) yield return y; }
                { var x = Item7 as ITuple; if (x == null) yield return Item7; else foreach (var y in x.Values) yield return y; }
                { var x = Item8 as ITuple; if (x == null) yield return Item8; else foreach (var y in x.Values) yield return y; }
                { var x = Item9 as ITuple; if (x == null) yield return Item9; else foreach (var y in x.Values) yield return y; }
                { var x = Item10 as ITuple; if (x == null) yield return Item10; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                if (Get(ref Item6, ref index, out output)) return output;
                if (Get(ref Item7, ref index, out output)) return output;
                if (Get(ref Item8, ref index, out output)) return output;
                if (Get(ref Item9, ref index, out output)) return output;
                if (Get(ref Item10, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
                if (Set(ref Item6, ref index, value)) return;
                if (Set(ref Item7, ref index, value)) return;
                if (Set(ref Item8, ref index, value)) return;
                if (Set(ref Item9, ref index, value)) return;
                if (Set(ref Item10, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;
        public T8 Item8;
        public T9 Item9;
        public T10 Item10;
        public T11 Item11;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
            Item9 = item9;
            Item10 = item10;
            Item11 = item11;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5) + Count(ref Item6) + Count(ref Item7) + Count(ref Item8) + Count(ref Item9) + Count(ref Item10) + Count(ref Item11);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
                { var x = Item6 as ITuple; if (x == null) yield return Item6; else foreach (var y in x.Values) yield return y; }
                { var x = Item7 as ITuple; if (x == null) yield return Item7; else foreach (var y in x.Values) yield return y; }
                { var x = Item8 as ITuple; if (x == null) yield return Item8; else foreach (var y in x.Values) yield return y; }
                { var x = Item9 as ITuple; if (x == null) yield return Item9; else foreach (var y in x.Values) yield return y; }
                { var x = Item10 as ITuple; if (x == null) yield return Item10; else foreach (var y in x.Values) yield return y; }
                { var x = Item11 as ITuple; if (x == null) yield return Item11; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                if (Get(ref Item6, ref index, out output)) return output;
                if (Get(ref Item7, ref index, out output)) return output;
                if (Get(ref Item8, ref index, out output)) return output;
                if (Get(ref Item9, ref index, out output)) return output;
                if (Get(ref Item10, ref index, out output)) return output;
                if (Get(ref Item11, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
                if (Set(ref Item6, ref index, value)) return;
                if (Set(ref Item7, ref index, value)) return;
                if (Set(ref Item8, ref index, value)) return;
                if (Set(ref Item9, ref index, value)) return;
                if (Set(ref Item10, ref index, value)) return;
                if (Set(ref Item11, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;
        public T8 Item8;
        public T9 Item9;
        public T10 Item10;
        public T11 Item11;
        public T12 Item12;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
            Item9 = item9;
            Item10 = item10;
            Item11 = item11;
            Item12 = item12;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5) + Count(ref Item6) + Count(ref Item7) + Count(ref Item8) + Count(ref Item9) + Count(ref Item10) + Count(ref Item11) + Count(ref Item12);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
                { var x = Item6 as ITuple; if (x == null) yield return Item6; else foreach (var y in x.Values) yield return y; }
                { var x = Item7 as ITuple; if (x == null) yield return Item7; else foreach (var y in x.Values) yield return y; }
                { var x = Item8 as ITuple; if (x == null) yield return Item8; else foreach (var y in x.Values) yield return y; }
                { var x = Item9 as ITuple; if (x == null) yield return Item9; else foreach (var y in x.Values) yield return y; }
                { var x = Item10 as ITuple; if (x == null) yield return Item10; else foreach (var y in x.Values) yield return y; }
                { var x = Item11 as ITuple; if (x == null) yield return Item11; else foreach (var y in x.Values) yield return y; }
                { var x = Item12 as ITuple; if (x == null) yield return Item12; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                if (Get(ref Item6, ref index, out output)) return output;
                if (Get(ref Item7, ref index, out output)) return output;
                if (Get(ref Item8, ref index, out output)) return output;
                if (Get(ref Item9, ref index, out output)) return output;
                if (Get(ref Item10, ref index, out output)) return output;
                if (Get(ref Item11, ref index, out output)) return output;
                if (Get(ref Item12, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
                if (Set(ref Item6, ref index, value)) return;
                if (Set(ref Item7, ref index, value)) return;
                if (Set(ref Item8, ref index, value)) return;
                if (Set(ref Item9, ref index, value)) return;
                if (Set(ref Item10, ref index, value)) return;
                if (Set(ref Item11, ref index, value)) return;
                if (Set(ref Item12, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;
        public T8 Item8;
        public T9 Item9;
        public T10 Item10;
        public T11 Item11;
        public T12 Item12;
        public T13 Item13;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
            Item9 = item9;
            Item10 = item10;
            Item11 = item11;
            Item12 = item12;
            Item13 = item13;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5) + Count(ref Item6) + Count(ref Item7) + Count(ref Item8) + Count(ref Item9) + Count(ref Item10) + Count(ref Item11) + Count(ref Item12) + Count(ref Item13);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
                { var x = Item6 as ITuple; if (x == null) yield return Item6; else foreach (var y in x.Values) yield return y; }
                { var x = Item7 as ITuple; if (x == null) yield return Item7; else foreach (var y in x.Values) yield return y; }
                { var x = Item8 as ITuple; if (x == null) yield return Item8; else foreach (var y in x.Values) yield return y; }
                { var x = Item9 as ITuple; if (x == null) yield return Item9; else foreach (var y in x.Values) yield return y; }
                { var x = Item10 as ITuple; if (x == null) yield return Item10; else foreach (var y in x.Values) yield return y; }
                { var x = Item11 as ITuple; if (x == null) yield return Item11; else foreach (var y in x.Values) yield return y; }
                { var x = Item12 as ITuple; if (x == null) yield return Item12; else foreach (var y in x.Values) yield return y; }
                { var x = Item13 as ITuple; if (x == null) yield return Item13; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                if (Get(ref Item6, ref index, out output)) return output;
                if (Get(ref Item7, ref index, out output)) return output;
                if (Get(ref Item8, ref index, out output)) return output;
                if (Get(ref Item9, ref index, out output)) return output;
                if (Get(ref Item10, ref index, out output)) return output;
                if (Get(ref Item11, ref index, out output)) return output;
                if (Get(ref Item12, ref index, out output)) return output;
                if (Get(ref Item13, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
                if (Set(ref Item6, ref index, value)) return;
                if (Set(ref Item7, ref index, value)) return;
                if (Set(ref Item8, ref index, value)) return;
                if (Set(ref Item9, ref index, value)) return;
                if (Set(ref Item10, ref index, value)) return;
                if (Set(ref Item11, ref index, value)) return;
                if (Set(ref Item12, ref index, value)) return;
                if (Set(ref Item13, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;
        public T8 Item8;
        public T9 Item9;
        public T10 Item10;
        public T11 Item11;
        public T12 Item12;
        public T13 Item13;
        public T14 Item14;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
            Item9 = item9;
            Item10 = item10;
            Item11 = item11;
            Item12 = item12;
            Item13 = item13;
            Item14 = item14;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5) + Count(ref Item6) + Count(ref Item7) + Count(ref Item8) + Count(ref Item9) + Count(ref Item10) + Count(ref Item11) + Count(ref Item12) + Count(ref Item13) + Count(ref Item14);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
                { var x = Item6 as ITuple; if (x == null) yield return Item6; else foreach (var y in x.Values) yield return y; }
                { var x = Item7 as ITuple; if (x == null) yield return Item7; else foreach (var y in x.Values) yield return y; }
                { var x = Item8 as ITuple; if (x == null) yield return Item8; else foreach (var y in x.Values) yield return y; }
                { var x = Item9 as ITuple; if (x == null) yield return Item9; else foreach (var y in x.Values) yield return y; }
                { var x = Item10 as ITuple; if (x == null) yield return Item10; else foreach (var y in x.Values) yield return y; }
                { var x = Item11 as ITuple; if (x == null) yield return Item11; else foreach (var y in x.Values) yield return y; }
                { var x = Item12 as ITuple; if (x == null) yield return Item12; else foreach (var y in x.Values) yield return y; }
                { var x = Item13 as ITuple; if (x == null) yield return Item13; else foreach (var y in x.Values) yield return y; }
                { var x = Item14 as ITuple; if (x == null) yield return Item14; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                if (Get(ref Item6, ref index, out output)) return output;
                if (Get(ref Item7, ref index, out output)) return output;
                if (Get(ref Item8, ref index, out output)) return output;
                if (Get(ref Item9, ref index, out output)) return output;
                if (Get(ref Item10, ref index, out output)) return output;
                if (Get(ref Item11, ref index, out output)) return output;
                if (Get(ref Item12, ref index, out output)) return output;
                if (Get(ref Item13, ref index, out output)) return output;
                if (Get(ref Item14, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
                if (Set(ref Item6, ref index, value)) return;
                if (Set(ref Item7, ref index, value)) return;
                if (Set(ref Item8, ref index, value)) return;
                if (Set(ref Item9, ref index, value)) return;
                if (Set(ref Item10, ref index, value)) return;
                if (Set(ref Item11, ref index, value)) return;
                if (Set(ref Item12, ref index, value)) return;
                if (Set(ref Item13, ref index, value)) return;
                if (Set(ref Item14, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;
        public T8 Item8;
        public T9 Item9;
        public T10 Item10;
        public T11 Item11;
        public T12 Item12;
        public T13 Item13;
        public T14 Item14;
        public T15 Item15;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
            Item9 = item9;
            Item10 = item10;
            Item11 = item11;
            Item12 = item12;
            Item13 = item13;
            Item14 = item14;
            Item15 = item15;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5) + Count(ref Item6) + Count(ref Item7) + Count(ref Item8) + Count(ref Item9) + Count(ref Item10) + Count(ref Item11) + Count(ref Item12) + Count(ref Item13) + Count(ref Item14) + Count(ref Item15);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
                { var x = Item6 as ITuple; if (x == null) yield return Item6; else foreach (var y in x.Values) yield return y; }
                { var x = Item7 as ITuple; if (x == null) yield return Item7; else foreach (var y in x.Values) yield return y; }
                { var x = Item8 as ITuple; if (x == null) yield return Item8; else foreach (var y in x.Values) yield return y; }
                { var x = Item9 as ITuple; if (x == null) yield return Item9; else foreach (var y in x.Values) yield return y; }
                { var x = Item10 as ITuple; if (x == null) yield return Item10; else foreach (var y in x.Values) yield return y; }
                { var x = Item11 as ITuple; if (x == null) yield return Item11; else foreach (var y in x.Values) yield return y; }
                { var x = Item12 as ITuple; if (x == null) yield return Item12; else foreach (var y in x.Values) yield return y; }
                { var x = Item13 as ITuple; if (x == null) yield return Item13; else foreach (var y in x.Values) yield return y; }
                { var x = Item14 as ITuple; if (x == null) yield return Item14; else foreach (var y in x.Values) yield return y; }
                { var x = Item15 as ITuple; if (x == null) yield return Item15; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                if (Get(ref Item6, ref index, out output)) return output;
                if (Get(ref Item7, ref index, out output)) return output;
                if (Get(ref Item8, ref index, out output)) return output;
                if (Get(ref Item9, ref index, out output)) return output;
                if (Get(ref Item10, ref index, out output)) return output;
                if (Get(ref Item11, ref index, out output)) return output;
                if (Get(ref Item12, ref index, out output)) return output;
                if (Get(ref Item13, ref index, out output)) return output;
                if (Get(ref Item14, ref index, out output)) return output;
                if (Get(ref Item15, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
                if (Set(ref Item6, ref index, value)) return;
                if (Set(ref Item7, ref index, value)) return;
                if (Set(ref Item8, ref index, value)) return;
                if (Set(ref Item9, ref index, value)) return;
                if (Set(ref Item10, ref index, value)) return;
                if (Set(ref Item11, ref index, value)) return;
                if (Set(ref Item12, ref index, value)) return;
                if (Set(ref Item13, ref index, value)) return;
                if (Set(ref Item14, ref index, value)) return;
                if (Set(ref Item15, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;
        public T8 Item8;
        public T9 Item9;
        public T10 Item10;
        public T11 Item11;
        public T12 Item12;
        public T13 Item13;
        public T14 Item14;
        public T15 Item15;
        public T16 Item16;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
            Item9 = item9;
            Item10 = item10;
            Item11 = item11;
            Item12 = item12;
            Item13 = item13;
            Item14 = item14;
            Item15 = item15;
            Item16 = item16;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5) + Count(ref Item6) + Count(ref Item7) + Count(ref Item8) + Count(ref Item9) + Count(ref Item10) + Count(ref Item11) + Count(ref Item12) + Count(ref Item13) + Count(ref Item14) + Count(ref Item15) + Count(ref Item16);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
                { var x = Item6 as ITuple; if (x == null) yield return Item6; else foreach (var y in x.Values) yield return y; }
                { var x = Item7 as ITuple; if (x == null) yield return Item7; else foreach (var y in x.Values) yield return y; }
                { var x = Item8 as ITuple; if (x == null) yield return Item8; else foreach (var y in x.Values) yield return y; }
                { var x = Item9 as ITuple; if (x == null) yield return Item9; else foreach (var y in x.Values) yield return y; }
                { var x = Item10 as ITuple; if (x == null) yield return Item10; else foreach (var y in x.Values) yield return y; }
                { var x = Item11 as ITuple; if (x == null) yield return Item11; else foreach (var y in x.Values) yield return y; }
                { var x = Item12 as ITuple; if (x == null) yield return Item12; else foreach (var y in x.Values) yield return y; }
                { var x = Item13 as ITuple; if (x == null) yield return Item13; else foreach (var y in x.Values) yield return y; }
                { var x = Item14 as ITuple; if (x == null) yield return Item14; else foreach (var y in x.Values) yield return y; }
                { var x = Item15 as ITuple; if (x == null) yield return Item15; else foreach (var y in x.Values) yield return y; }
                { var x = Item16 as ITuple; if (x == null) yield return Item16; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                if (Get(ref Item6, ref index, out output)) return output;
                if (Get(ref Item7, ref index, out output)) return output;
                if (Get(ref Item8, ref index, out output)) return output;
                if (Get(ref Item9, ref index, out output)) return output;
                if (Get(ref Item10, ref index, out output)) return output;
                if (Get(ref Item11, ref index, out output)) return output;
                if (Get(ref Item12, ref index, out output)) return output;
                if (Get(ref Item13, ref index, out output)) return output;
                if (Get(ref Item14, ref index, out output)) return output;
                if (Get(ref Item15, ref index, out output)) return output;
                if (Get(ref Item16, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
                if (Set(ref Item6, ref index, value)) return;
                if (Set(ref Item7, ref index, value)) return;
                if (Set(ref Item8, ref index, value)) return;
                if (Set(ref Item9, ref index, value)) return;
                if (Set(ref Item10, ref index, value)) return;
                if (Set(ref Item11, ref index, value)) return;
                if (Set(ref Item12, ref index, value)) return;
                if (Set(ref Item13, ref index, value)) return;
                if (Set(ref Item14, ref index, value)) return;
                if (Set(ref Item15, ref index, value)) return;
                if (Set(ref Item16, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;
        public T8 Item8;
        public T9 Item9;
        public T10 Item10;
        public T11 Item11;
        public T12 Item12;
        public T13 Item13;
        public T14 Item14;
        public T15 Item15;
        public T16 Item16;
        public T17 Item17;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
            Item9 = item9;
            Item10 = item10;
            Item11 = item11;
            Item12 = item12;
            Item13 = item13;
            Item14 = item14;
            Item15 = item15;
            Item16 = item16;
            Item17 = item17;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5) + Count(ref Item6) + Count(ref Item7) + Count(ref Item8) + Count(ref Item9) + Count(ref Item10) + Count(ref Item11) + Count(ref Item12) + Count(ref Item13) + Count(ref Item14) + Count(ref Item15) + Count(ref Item16) + Count(ref Item17);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
                { var x = Item6 as ITuple; if (x == null) yield return Item6; else foreach (var y in x.Values) yield return y; }
                { var x = Item7 as ITuple; if (x == null) yield return Item7; else foreach (var y in x.Values) yield return y; }
                { var x = Item8 as ITuple; if (x == null) yield return Item8; else foreach (var y in x.Values) yield return y; }
                { var x = Item9 as ITuple; if (x == null) yield return Item9; else foreach (var y in x.Values) yield return y; }
                { var x = Item10 as ITuple; if (x == null) yield return Item10; else foreach (var y in x.Values) yield return y; }
                { var x = Item11 as ITuple; if (x == null) yield return Item11; else foreach (var y in x.Values) yield return y; }
                { var x = Item12 as ITuple; if (x == null) yield return Item12; else foreach (var y in x.Values) yield return y; }
                { var x = Item13 as ITuple; if (x == null) yield return Item13; else foreach (var y in x.Values) yield return y; }
                { var x = Item14 as ITuple; if (x == null) yield return Item14; else foreach (var y in x.Values) yield return y; }
                { var x = Item15 as ITuple; if (x == null) yield return Item15; else foreach (var y in x.Values) yield return y; }
                { var x = Item16 as ITuple; if (x == null) yield return Item16; else foreach (var y in x.Values) yield return y; }
                { var x = Item17 as ITuple; if (x == null) yield return Item17; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                if (Get(ref Item6, ref index, out output)) return output;
                if (Get(ref Item7, ref index, out output)) return output;
                if (Get(ref Item8, ref index, out output)) return output;
                if (Get(ref Item9, ref index, out output)) return output;
                if (Get(ref Item10, ref index, out output)) return output;
                if (Get(ref Item11, ref index, out output)) return output;
                if (Get(ref Item12, ref index, out output)) return output;
                if (Get(ref Item13, ref index, out output)) return output;
                if (Get(ref Item14, ref index, out output)) return output;
                if (Get(ref Item15, ref index, out output)) return output;
                if (Get(ref Item16, ref index, out output)) return output;
                if (Get(ref Item17, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
                if (Set(ref Item6, ref index, value)) return;
                if (Set(ref Item7, ref index, value)) return;
                if (Set(ref Item8, ref index, value)) return;
                if (Set(ref Item9, ref index, value)) return;
                if (Set(ref Item10, ref index, value)) return;
                if (Set(ref Item11, ref index, value)) return;
                if (Set(ref Item12, ref index, value)) return;
                if (Set(ref Item13, ref index, value)) return;
                if (Set(ref Item14, ref index, value)) return;
                if (Set(ref Item15, ref index, value)) return;
                if (Set(ref Item16, ref index, value)) return;
                if (Set(ref Item17, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;
        public T8 Item8;
        public T9 Item9;
        public T10 Item10;
        public T11 Item11;
        public T12 Item12;
        public T13 Item13;
        public T14 Item14;
        public T15 Item15;
        public T16 Item16;
        public T17 Item17;
        public T18 Item18;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17, T18 item18)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
            Item9 = item9;
            Item10 = item10;
            Item11 = item11;
            Item12 = item12;
            Item13 = item13;
            Item14 = item14;
            Item15 = item15;
            Item16 = item16;
            Item17 = item17;
            Item18 = item18;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5) + Count(ref Item6) + Count(ref Item7) + Count(ref Item8) + Count(ref Item9) + Count(ref Item10) + Count(ref Item11) + Count(ref Item12) + Count(ref Item13) + Count(ref Item14) + Count(ref Item15) + Count(ref Item16) + Count(ref Item17) + Count(ref Item18);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
                { var x = Item6 as ITuple; if (x == null) yield return Item6; else foreach (var y in x.Values) yield return y; }
                { var x = Item7 as ITuple; if (x == null) yield return Item7; else foreach (var y in x.Values) yield return y; }
                { var x = Item8 as ITuple; if (x == null) yield return Item8; else foreach (var y in x.Values) yield return y; }
                { var x = Item9 as ITuple; if (x == null) yield return Item9; else foreach (var y in x.Values) yield return y; }
                { var x = Item10 as ITuple; if (x == null) yield return Item10; else foreach (var y in x.Values) yield return y; }
                { var x = Item11 as ITuple; if (x == null) yield return Item11; else foreach (var y in x.Values) yield return y; }
                { var x = Item12 as ITuple; if (x == null) yield return Item12; else foreach (var y in x.Values) yield return y; }
                { var x = Item13 as ITuple; if (x == null) yield return Item13; else foreach (var y in x.Values) yield return y; }
                { var x = Item14 as ITuple; if (x == null) yield return Item14; else foreach (var y in x.Values) yield return y; }
                { var x = Item15 as ITuple; if (x == null) yield return Item15; else foreach (var y in x.Values) yield return y; }
                { var x = Item16 as ITuple; if (x == null) yield return Item16; else foreach (var y in x.Values) yield return y; }
                { var x = Item17 as ITuple; if (x == null) yield return Item17; else foreach (var y in x.Values) yield return y; }
                { var x = Item18 as ITuple; if (x == null) yield return Item18; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                if (Get(ref Item6, ref index, out output)) return output;
                if (Get(ref Item7, ref index, out output)) return output;
                if (Get(ref Item8, ref index, out output)) return output;
                if (Get(ref Item9, ref index, out output)) return output;
                if (Get(ref Item10, ref index, out output)) return output;
                if (Get(ref Item11, ref index, out output)) return output;
                if (Get(ref Item12, ref index, out output)) return output;
                if (Get(ref Item13, ref index, out output)) return output;
                if (Get(ref Item14, ref index, out output)) return output;
                if (Get(ref Item15, ref index, out output)) return output;
                if (Get(ref Item16, ref index, out output)) return output;
                if (Get(ref Item17, ref index, out output)) return output;
                if (Get(ref Item18, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
                if (Set(ref Item6, ref index, value)) return;
                if (Set(ref Item7, ref index, value)) return;
                if (Set(ref Item8, ref index, value)) return;
                if (Set(ref Item9, ref index, value)) return;
                if (Set(ref Item10, ref index, value)) return;
                if (Set(ref Item11, ref index, value)) return;
                if (Set(ref Item12, ref index, value)) return;
                if (Set(ref Item13, ref index, value)) return;
                if (Set(ref Item14, ref index, value)) return;
                if (Set(ref Item15, ref index, value)) return;
                if (Set(ref Item16, ref index, value)) return;
                if (Set(ref Item17, ref index, value)) return;
                if (Set(ref Item18, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;
        public T8 Item8;
        public T9 Item9;
        public T10 Item10;
        public T11 Item11;
        public T12 Item12;
        public T13 Item13;
        public T14 Item14;
        public T15 Item15;
        public T16 Item16;
        public T17 Item17;
        public T18 Item18;
        public T19 Item19;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17, T18 item18, T19 item19)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
            Item9 = item9;
            Item10 = item10;
            Item11 = item11;
            Item12 = item12;
            Item13 = item13;
            Item14 = item14;
            Item15 = item15;
            Item16 = item16;
            Item17 = item17;
            Item18 = item18;
            Item19 = item19;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5) + Count(ref Item6) + Count(ref Item7) + Count(ref Item8) + Count(ref Item9) + Count(ref Item10) + Count(ref Item11) + Count(ref Item12) + Count(ref Item13) + Count(ref Item14) + Count(ref Item15) + Count(ref Item16) + Count(ref Item17) + Count(ref Item18) + Count(ref Item19);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
                { var x = Item6 as ITuple; if (x == null) yield return Item6; else foreach (var y in x.Values) yield return y; }
                { var x = Item7 as ITuple; if (x == null) yield return Item7; else foreach (var y in x.Values) yield return y; }
                { var x = Item8 as ITuple; if (x == null) yield return Item8; else foreach (var y in x.Values) yield return y; }
                { var x = Item9 as ITuple; if (x == null) yield return Item9; else foreach (var y in x.Values) yield return y; }
                { var x = Item10 as ITuple; if (x == null) yield return Item10; else foreach (var y in x.Values) yield return y; }
                { var x = Item11 as ITuple; if (x == null) yield return Item11; else foreach (var y in x.Values) yield return y; }
                { var x = Item12 as ITuple; if (x == null) yield return Item12; else foreach (var y in x.Values) yield return y; }
                { var x = Item13 as ITuple; if (x == null) yield return Item13; else foreach (var y in x.Values) yield return y; }
                { var x = Item14 as ITuple; if (x == null) yield return Item14; else foreach (var y in x.Values) yield return y; }
                { var x = Item15 as ITuple; if (x == null) yield return Item15; else foreach (var y in x.Values) yield return y; }
                { var x = Item16 as ITuple; if (x == null) yield return Item16; else foreach (var y in x.Values) yield return y; }
                { var x = Item17 as ITuple; if (x == null) yield return Item17; else foreach (var y in x.Values) yield return y; }
                { var x = Item18 as ITuple; if (x == null) yield return Item18; else foreach (var y in x.Values) yield return y; }
                { var x = Item19 as ITuple; if (x == null) yield return Item19; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                if (Get(ref Item6, ref index, out output)) return output;
                if (Get(ref Item7, ref index, out output)) return output;
                if (Get(ref Item8, ref index, out output)) return output;
                if (Get(ref Item9, ref index, out output)) return output;
                if (Get(ref Item10, ref index, out output)) return output;
                if (Get(ref Item11, ref index, out output)) return output;
                if (Get(ref Item12, ref index, out output)) return output;
                if (Get(ref Item13, ref index, out output)) return output;
                if (Get(ref Item14, ref index, out output)) return output;
                if (Get(ref Item15, ref index, out output)) return output;
                if (Get(ref Item16, ref index, out output)) return output;
                if (Get(ref Item17, ref index, out output)) return output;
                if (Get(ref Item18, ref index, out output)) return output;
                if (Get(ref Item19, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
                if (Set(ref Item6, ref index, value)) return;
                if (Set(ref Item7, ref index, value)) return;
                if (Set(ref Item8, ref index, value)) return;
                if (Set(ref Item9, ref index, value)) return;
                if (Set(ref Item10, ref index, value)) return;
                if (Set(ref Item11, ref index, value)) return;
                if (Set(ref Item12, ref index, value)) return;
                if (Set(ref Item13, ref index, value)) return;
                if (Set(ref Item14, ref index, value)) return;
                if (Set(ref Item15, ref index, value)) return;
                if (Set(ref Item16, ref index, value)) return;
                if (Set(ref Item17, ref index, value)) return;
                if (Set(ref Item18, ref index, value)) return;
                if (Set(ref Item19, ref index, value)) return;
            }
        }
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> : ITuple
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;
        public T8 Item8;
        public T9 Item9;
        public T10 Item10;
        public T11 Item11;
        public T12 Item12;
        public T13 Item13;
        public T14 Item14;
        public T15 Item15;
        public T16 Item16;
        public T17 Item17;
        public T18 Item18;
        public T19 Item19;
        public T20 Item20;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17, T18 item18, T19 item19, T20 item20)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
            Item9 = item9;
            Item10 = item10;
            Item11 = item11;
            Item12 = item12;
            Item13 = item13;
            Item14 = item14;
            Item15 = item15;
            Item16 = item16;
            Item17 = item17;
            Item18 = item18;
            Item19 = item19;
            Item20 = item20;
        }

        int ITuple.Count => Count(ref Item1) + Count(ref Item2) + Count(ref Item3) + Count(ref Item4) + Count(ref Item5) + Count(ref Item6) + Count(ref Item7) + Count(ref Item8) + Count(ref Item9) + Count(ref Item10) + Count(ref Item11) + Count(ref Item12) + Count(ref Item13) + Count(ref Item14) + Count(ref Item15) + Count(ref Item16) + Count(ref Item17) + Count(ref Item18) + Count(ref Item19) + Count(ref Item20);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
                { var x = Item2 as ITuple; if (x == null) yield return Item2; else foreach (var y in x.Values) yield return y; }
                { var x = Item3 as ITuple; if (x == null) yield return Item3; else foreach (var y in x.Values) yield return y; }
                { var x = Item4 as ITuple; if (x == null) yield return Item4; else foreach (var y in x.Values) yield return y; }
                { var x = Item5 as ITuple; if (x == null) yield return Item5; else foreach (var y in x.Values) yield return y; }
                { var x = Item6 as ITuple; if (x == null) yield return Item6; else foreach (var y in x.Values) yield return y; }
                { var x = Item7 as ITuple; if (x == null) yield return Item7; else foreach (var y in x.Values) yield return y; }
                { var x = Item8 as ITuple; if (x == null) yield return Item8; else foreach (var y in x.Values) yield return y; }
                { var x = Item9 as ITuple; if (x == null) yield return Item9; else foreach (var y in x.Values) yield return y; }
                { var x = Item10 as ITuple; if (x == null) yield return Item10; else foreach (var y in x.Values) yield return y; }
                { var x = Item11 as ITuple; if (x == null) yield return Item11; else foreach (var y in x.Values) yield return y; }
                { var x = Item12 as ITuple; if (x == null) yield return Item12; else foreach (var y in x.Values) yield return y; }
                { var x = Item13 as ITuple; if (x == null) yield return Item13; else foreach (var y in x.Values) yield return y; }
                { var x = Item14 as ITuple; if (x == null) yield return Item14; else foreach (var y in x.Values) yield return y; }
                { var x = Item15 as ITuple; if (x == null) yield return Item15; else foreach (var y in x.Values) yield return y; }
                { var x = Item16 as ITuple; if (x == null) yield return Item16; else foreach (var y in x.Values) yield return y; }
                { var x = Item17 as ITuple; if (x == null) yield return Item17; else foreach (var y in x.Values) yield return y; }
                { var x = Item18 as ITuple; if (x == null) yield return Item18; else foreach (var y in x.Values) yield return y; }
                { var x = Item19 as ITuple; if (x == null) yield return Item19; else foreach (var y in x.Values) yield return y; }
                { var x = Item20 as ITuple; if (x == null) yield return Item20; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                if (Get(ref Item2, ref index, out output)) return output;
                if (Get(ref Item3, ref index, out output)) return output;
                if (Get(ref Item4, ref index, out output)) return output;
                if (Get(ref Item5, ref index, out output)) return output;
                if (Get(ref Item6, ref index, out output)) return output;
                if (Get(ref Item7, ref index, out output)) return output;
                if (Get(ref Item8, ref index, out output)) return output;
                if (Get(ref Item9, ref index, out output)) return output;
                if (Get(ref Item10, ref index, out output)) return output;
                if (Get(ref Item11, ref index, out output)) return output;
                if (Get(ref Item12, ref index, out output)) return output;
                if (Get(ref Item13, ref index, out output)) return output;
                if (Get(ref Item14, ref index, out output)) return output;
                if (Get(ref Item15, ref index, out output)) return output;
                if (Get(ref Item16, ref index, out output)) return output;
                if (Get(ref Item17, ref index, out output)) return output;
                if (Get(ref Item18, ref index, out output)) return output;
                if (Get(ref Item19, ref index, out output)) return output;
                if (Get(ref Item20, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
                if (Set(ref Item2, ref index, value)) return;
                if (Set(ref Item3, ref index, value)) return;
                if (Set(ref Item4, ref index, value)) return;
                if (Set(ref Item5, ref index, value)) return;
                if (Set(ref Item6, ref index, value)) return;
                if (Set(ref Item7, ref index, value)) return;
                if (Set(ref Item8, ref index, value)) return;
                if (Set(ref Item9, ref index, value)) return;
                if (Set(ref Item10, ref index, value)) return;
                if (Set(ref Item11, ref index, value)) return;
                if (Set(ref Item12, ref index, value)) return;
                if (Set(ref Item13, ref index, value)) return;
                if (Set(ref Item14, ref index, value)) return;
                if (Set(ref Item15, ref index, value)) return;
                if (Set(ref Item16, ref index, value)) return;
                if (Set(ref Item17, ref index, value)) return;
                if (Set(ref Item18, ref index, value)) return;
                if (Set(ref Item19, ref index, value)) return;
                if (Set(ref Item20, ref index, value)) return;
            }
        }
    }
}
