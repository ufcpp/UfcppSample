using System;
using System.Collections.Generic;

namespace ValueTuples.Reflection
{
    public static class TypeRepository
    {
        private static Dictionary<Type, RecordTypeInfo> _table = new Dictionary<Type, RecordTypeInfo>();

        static TypeRepository()
        {
            _table[typeof(Byte)] = Byte;
            _table[typeof(SByte)] = SByte;
            _table[typeof(Int16)] = Int16;
            _table[typeof(UInt16)] = UInt16;
            _table[typeof(Int32)] = Int32;
            _table[typeof(UInt32)] = UInt32;
            _table[typeof(Int64)] = Int64;
            _table[typeof(UInt64)] = UInt64;
            _table[typeof(Boolean)] = Boolean;
            _table[typeof(DateTime)] = DateTime;
            _table[typeof(Single)] = Single;
            _table[typeof(Double)] = Double;
            _table[typeof(Decimal)] = Decimal;
            _table[typeof(String)] = String;
        }

        public static readonly RecordTypeInfo Byte = new ByteInfo();
        public static readonly RecordTypeInfo SByte = new SByteInfo();
        public static readonly RecordTypeInfo Int16 = new Int16Info();
        public static readonly RecordTypeInfo UInt16 = new UInt16Info();
        public static readonly RecordTypeInfo Int32 = new Int32Info();
        public static readonly RecordTypeInfo UInt32 = new UInt32Info();
        public static readonly RecordTypeInfo Int64 = new Int64Info();
        public static readonly RecordTypeInfo UInt64 = new UInt64Info();
        public static readonly RecordTypeInfo Boolean = new BooleanInfo();
        public static readonly RecordTypeInfo DateTime = new DateTimeInfo();
        public static readonly RecordTypeInfo Single = new SingleInfo();
        public static readonly RecordTypeInfo Double = new DoubleInfo();
        public static readonly RecordTypeInfo Decimal = new DecimalInfo();
        public static readonly RecordTypeInfo String = new StringInfo();

        public static void Register(Type t, RecordTypeInfo info) => _table[t] = info;

        public static RecordTypeInfo Get(Type t)
        {
            if(t.IsArray)
            {
                var elem = Get(t.GetElementType());
                return new ArrayTypeInfo(elem);
            }
            return _table[t];
        }
    }
}
