using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Tuples.X
{
    [StructLayout(LayoutKind.Auto)]
    public struct ValueTuple<T1, T2>
        //: IEquatable<ValueTuple<T1, T2>>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<ValueTuple<T1, T2>>
    {
        public T1 Item1;
        public T2 Item2;

        public ValueTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        // 後略、インターフェイスのメンバー定義
    }
}
