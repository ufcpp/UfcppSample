using System;
using System.Runtime.CompilerServices;

namespace BitOperations
{
    public static class Bits
    {
        public static Bits<byte, ByteBitOperator> Create(ref byte x) => new Bits<byte, ByteBitOperator>(ref x);
        public static Bits<ushort, ShortBitOperator> Create(ref ushort x) => new Bits<ushort, ShortBitOperator>(ref x);
        public static Bits<uint, IntBitOperator> Create(ref uint x) => new Bits<uint, IntBitOperator>(ref x);
        public static Bits<ulong, LongBitOperator> Create(ref ulong x) => new Bits<ulong, LongBitOperator>(ref x);
        public static Bits<Bytes16, Bytes16BitOperator> Create(ref Bytes16 x) => new Bits<Bytes16, Bytes16BitOperator>(ref x);

        // box 化起きちゃうし、非常に unsafe 操作だし、決まったサイズしか対応してないけど、任意の型を受け付ける版
        // 将来的に blittable の仕様が入ったときに、それをインターフェイスを介して戻り値に返せるかは不明
        public static IBits Create<T>(ref T x)
            where T : struct
        {
            switch (Unsafe.SizeOf<T>())
            {
                case 1:
                    ref var b = ref Unsafe.As<T, byte>(ref x);
                    return Create(ref b);
                case 2:
                    ref var s = ref Unsafe.As<T, ushort>(ref x);
                    return Create(ref s);
                case 4:
                    ref var i = ref Unsafe.As<T, uint>(ref x);
                    return Create(ref i);
                case 8:
                    ref var l = ref Unsafe.As<T, ulong>(ref x);
                    return Create(ref l);
                case 16:
                    ref var b16 = ref Unsafe.As<T, Bytes16>(ref x);
                    return Create(ref b16);
                default:
                    throw new NotSupportedException("今のところ、1, 2, 4, 8, 16バイトの構造体しか対応していない");
            }
        }
    }
}
