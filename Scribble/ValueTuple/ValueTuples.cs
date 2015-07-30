using System.Collections.Generic;
using static ValueTuples.ValueTuple;

namespace ValueTuples
{
    public static partial class ValueTuple
    {
        public static ValueTuple<T1> Create<T1>(T1 item1) => new ValueTuple<T1>(item1);
        public static ValueTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2) => new ValueTuple<T1, T2>(item1, item2);
        public static ValueTuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) => new ValueTuple<T1, T2, T3>(item1, item2, item3);
        public static ValueTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) => new ValueTuple<T1, T2, T3, T4>(item1, item2, item3, item4);
        public static ValueTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) => new ValueTuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
        public static ValueTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) => new ValueTuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(item1, item2, item3, item4, item5, item6, item7, item8);
    }

    public struct ValueTuple<T1> : ITuple, IDeepCloneable<ValueTuple<T1>>
    {
        public T1 Item1;

        public ValueTuple(T1 item1)
        {
            Item1 = item1;
        }

        int ITuple.Count => Count(ref Item1);

        IEnumerable<object> ITuple.Values
        {
            get
            {
                { var x = Item1 as ITuple; if (x == null) yield return Item1; else foreach (var y in x.Values) yield return y; }
            }
        }

        object ITuple.this[int index]
        {
            get
            {
                object output;
                if (Get(ref Item1, ref index, out output)) return output;
                return output;
            }
            set
            {
                if (Set(ref Item1, ref index, value)) return;
            }
        }

        ValueTuple<T1> IDeepCloneable<ValueTuple<T1>>.Clone() => Create(Item1.DeepClone());
    }
    public struct ValueTuple<T1, T2> : ITuple, IDeepCloneable<ValueTuple<T1, T2>>
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

        ValueTuple<T1, T2> IDeepCloneable<ValueTuple<T1, T2>>.Clone() => Create(Item1.DeepClone(), Item2.DeepClone());
    }
    public struct ValueTuple<T1, T2, T3> : ITuple, IDeepCloneable<ValueTuple<T1, T2, T3>>
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

        ValueTuple<T1, T2, T3> IDeepCloneable<ValueTuple<T1, T2, T3>>.Clone() => Create(Item1.DeepClone(), Item2.DeepClone(), Item3.DeepClone());
    }
    public struct ValueTuple<T1, T2, T3, T4> : ITuple, IDeepCloneable<ValueTuple<T1, T2, T3, T4>>
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

        ValueTuple<T1, T2, T3, T4> IDeepCloneable<ValueTuple<T1, T2, T3, T4>>.Clone() => Create(Item1.DeepClone(), Item2.DeepClone(), Item3.DeepClone(), Item4.DeepClone());
    }
    public struct ValueTuple<T1, T2, T3, T4, T5> : ITuple, IDeepCloneable<ValueTuple<T1, T2, T3, T4, T5>>
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

        ValueTuple<T1, T2, T3, T4, T5> IDeepCloneable<ValueTuple<T1, T2, T3, T4, T5>>.Clone() => Create(Item1.DeepClone(), Item2.DeepClone(), Item3.DeepClone(), Item4.DeepClone(), Item5.DeepClone());
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6> : ITuple, IDeepCloneable<ValueTuple<T1, T2, T3, T4, T5, T6>>
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

        ValueTuple<T1, T2, T3, T4, T5, T6> IDeepCloneable<ValueTuple<T1, T2, T3, T4, T5, T6>>.Clone() => Create(Item1.DeepClone(), Item2.DeepClone(), Item3.DeepClone(), Item4.DeepClone(), Item5.DeepClone(), Item6.DeepClone());
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7> : ITuple, IDeepCloneable<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>
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

        ValueTuple<T1, T2, T3, T4, T5, T6, T7> IDeepCloneable<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>.Clone() => Create(Item1.DeepClone(), Item2.DeepClone(), Item3.DeepClone(), Item4.DeepClone(), Item5.DeepClone(), Item6.DeepClone(), Item7.DeepClone());
    }
    public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8> : ITuple, IDeepCloneable<ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>>
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

        ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8> IDeepCloneable<ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>>.Clone() => Create(Item1.DeepClone(), Item2.DeepClone(), Item3.DeepClone(), Item4.DeepClone(), Item5.DeepClone(), Item6.DeepClone(), Item7.DeepClone(), Item8.DeepClone());
    }
}
