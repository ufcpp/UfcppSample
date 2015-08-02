using System;
using System.Runtime.InteropServices;

namespace MiniMessagePack
{
    public enum NumberType : byte
    {
        _bool,
        _byte,
        _sbyte,
        _short,
        _ushort,
        _int,
        _uint,
        _long,
        _ulong,
        _float,
        _double,
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Number
    {
        [FieldOffset(0)] bool _bool;
        public Number(bool b) : this() { _bool = b; _type = NumberType._bool; }
        public static explicit operator Number (bool n) => new Number(n);
        [FieldOffset(0)] byte _byte;
        public Number(byte b) : this() { _byte = b; _type = NumberType._byte; }
        public static explicit operator Number (byte n) => new Number(n);
        [FieldOffset(0)] sbyte _sbyte;
        public Number(sbyte b) : this() { _sbyte = b; _type = NumberType._sbyte; }
        public static explicit operator Number (sbyte n) => new Number(n);
        [FieldOffset(0)] short _short;
        public Number(short b) : this() { _short = b; _type = NumberType._short; }
        public static explicit operator Number (short n) => new Number(n);
        [FieldOffset(0)] ushort _ushort;
        public Number(ushort b) : this() { _ushort = b; _type = NumberType._ushort; }
        public static explicit operator Number (ushort n) => new Number(n);
        [FieldOffset(0)] int _int;
        public Number(int b) : this() { _int = b; _type = NumberType._int; }
        public static explicit operator Number (int n) => new Number(n);
        [FieldOffset(0)] uint _uint;
        public Number(uint b) : this() { _uint = b; _type = NumberType._uint; }
        public static explicit operator Number (uint n) => new Number(n);
        [FieldOffset(0)] long _long;
        public Number(long b) : this() { _long = b; _type = NumberType._long; }
        public static explicit operator Number (long n) => new Number(n);
        [FieldOffset(0)] ulong _ulong;
        public Number(ulong b) : this() { _ulong = b; _type = NumberType._ulong; }
        public static explicit operator Number (ulong n) => new Number(n);
        [FieldOffset(0)] float _float;
        public Number(float b) : this() { _float = b; _type = NumberType._float; }
        public static explicit operator Number (float n) => new Number(n);
        [FieldOffset(0)] double _double;
        public Number(double b) : this() { _double = b; _type = NumberType._double; }
        public static explicit operator Number (double n) => new Number(n);
        public static explicit operator byte (Number n)
        {
            switch (n._type)
            {
            case NumberType._byte: return unchecked((byte)n._byte);
            case NumberType._sbyte: return unchecked((byte)n._sbyte);
            case NumberType._short: return unchecked((byte)n._short);
            case NumberType._ushort: return unchecked((byte)n._ushort);
            case NumberType._int: return unchecked((byte)n._int);
            case NumberType._uint: return unchecked((byte)n._uint);
            case NumberType._long: return unchecked((byte)n._long);
            case NumberType._ulong: return unchecked((byte)n._ulong);
            }
            throw new InvalidCastException();
        }

        public static explicit operator sbyte (Number n)
        {
            switch (n._type)
            {
            case NumberType._byte: return unchecked((sbyte)n._byte);
            case NumberType._sbyte: return unchecked((sbyte)n._sbyte);
            case NumberType._short: return unchecked((sbyte)n._short);
            case NumberType._ushort: return unchecked((sbyte)n._ushort);
            case NumberType._int: return unchecked((sbyte)n._int);
            case NumberType._uint: return unchecked((sbyte)n._uint);
            case NumberType._long: return unchecked((sbyte)n._long);
            case NumberType._ulong: return unchecked((sbyte)n._ulong);
            }
            throw new InvalidCastException();
        }

        public static explicit operator short (Number n)
        {
            switch (n._type)
            {
            case NumberType._byte: return unchecked((short)n._byte);
            case NumberType._sbyte: return unchecked((short)n._sbyte);
            case NumberType._short: return unchecked((short)n._short);
            case NumberType._ushort: return unchecked((short)n._ushort);
            case NumberType._int: return unchecked((short)n._int);
            case NumberType._uint: return unchecked((short)n._uint);
            case NumberType._long: return unchecked((short)n._long);
            case NumberType._ulong: return unchecked((short)n._ulong);
            }
            throw new InvalidCastException();
        }

        public static explicit operator ushort (Number n)
        {
            switch (n._type)
            {
            case NumberType._byte: return unchecked((ushort)n._byte);
            case NumberType._sbyte: return unchecked((ushort)n._sbyte);
            case NumberType._short: return unchecked((ushort)n._short);
            case NumberType._ushort: return unchecked((ushort)n._ushort);
            case NumberType._int: return unchecked((ushort)n._int);
            case NumberType._uint: return unchecked((ushort)n._uint);
            case NumberType._long: return unchecked((ushort)n._long);
            case NumberType._ulong: return unchecked((ushort)n._ulong);
            }
            throw new InvalidCastException();
        }

        public static explicit operator int (Number n)
        {
            switch (n._type)
            {
            case NumberType._byte: return unchecked((int)n._byte);
            case NumberType._sbyte: return unchecked((int)n._sbyte);
            case NumberType._short: return unchecked((int)n._short);
            case NumberType._ushort: return unchecked((int)n._ushort);
            case NumberType._int: return unchecked((int)n._int);
            case NumberType._uint: return unchecked((int)n._uint);
            case NumberType._long: return unchecked((int)n._long);
            case NumberType._ulong: return unchecked((int)n._ulong);
            }
            throw new InvalidCastException();
        }

        public static explicit operator uint (Number n)
        {
            switch (n._type)
            {
            case NumberType._byte: return unchecked((uint)n._byte);
            case NumberType._sbyte: return unchecked((uint)n._sbyte);
            case NumberType._short: return unchecked((uint)n._short);
            case NumberType._ushort: return unchecked((uint)n._ushort);
            case NumberType._int: return unchecked((uint)n._int);
            case NumberType._uint: return unchecked((uint)n._uint);
            case NumberType._long: return unchecked((uint)n._long);
            case NumberType._ulong: return unchecked((uint)n._ulong);
            }
            throw new InvalidCastException();
        }

        public static explicit operator long (Number n)
        {
            switch (n._type)
            {
            case NumberType._byte: return unchecked((long)n._byte);
            case NumberType._sbyte: return unchecked((long)n._sbyte);
            case NumberType._short: return unchecked((long)n._short);
            case NumberType._ushort: return unchecked((long)n._ushort);
            case NumberType._int: return unchecked((long)n._int);
            case NumberType._uint: return unchecked((long)n._uint);
            case NumberType._long: return unchecked((long)n._long);
            case NumberType._ulong: return unchecked((long)n._ulong);
            }
            throw new InvalidCastException();
        }

        public static explicit operator ulong (Number n)
        {
            switch (n._type)
            {
            case NumberType._byte: return unchecked((ulong)n._byte);
            case NumberType._sbyte: return unchecked((ulong)n._sbyte);
            case NumberType._short: return unchecked((ulong)n._short);
            case NumberType._ushort: return unchecked((ulong)n._ushort);
            case NumberType._int: return unchecked((ulong)n._int);
            case NumberType._uint: return unchecked((ulong)n._uint);
            case NumberType._long: return unchecked((ulong)n._long);
            case NumberType._ulong: return unchecked((ulong)n._ulong);
            }
            throw new InvalidCastException();
        }

        public static explicit operator float (Number n)
        {
            switch (n._type)
            {
            case NumberType._float: return unchecked((float)n._float);
            case NumberType._double: return unchecked((float)n._double);
            }
            throw new InvalidCastException();
        }

        public static explicit operator double (Number n)
        {
            switch (n._type)
            {
            case NumberType._float: return unchecked((double)n._float);
            case NumberType._double: return unchecked((double)n._double);
            }
            throw new InvalidCastException();
        }


        public static explicit operator bool (Number n)
        {
            if (n._type != NumberType._byte)
                throw new InvalidCastException();
            return n._bool;
        }

        public override string ToString()
        {
            switch (_type)
            {
            default:
            case NumberType._bool: return _bool.ToString();
            case NumberType._byte: return _byte.ToString();
            case NumberType._sbyte: return _sbyte.ToString();
            case NumberType._short: return _short.ToString();
            case NumberType._ushort: return _ushort.ToString();
            case NumberType._int: return _int.ToString();
            case NumberType._uint: return _uint.ToString();
            case NumberType._long: return _long.ToString();
            case NumberType._ulong: return _ulong.ToString();
            case NumberType._float: return _float.ToString();
            case NumberType._double: return _double.ToString();
            }
        }

        [FieldOffset(8)]
        NumberType _type;

        public NumberType Type => _type;
    }
}
