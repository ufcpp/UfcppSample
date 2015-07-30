using System;
using System.Collections.Generic;
using System.Linq;

namespace ValueTuples.Reflection
{
    class SimpleTypeInfo<T> : RecordTypeInfo
    {
        public override Type Type => typeof(T);
        public override bool IsSimple => true;
        public override IEnumerable<RecordFieldInfo> Fields => Enumerable.Empty<RecordFieldInfo>();
        public override object GetInstance() => null;
        public override Array GetArray(int length) => new T[length];
        public override IRecordAccessor GetAccessor(object instance) => null;
    }

    class ByteInfo : SimpleTypeInfo<byte> { }
    class SByteInfo : SimpleTypeInfo<sbyte> { }
    class Int16Info : SimpleTypeInfo<short> { }
    class UInt16Info : SimpleTypeInfo<ushort> { }
    class Int32Info : SimpleTypeInfo<int> { }
    class UInt32Info : SimpleTypeInfo<uint> { }
    class Int64Info : SimpleTypeInfo<long> { }
    class UInt64Info : SimpleTypeInfo<ulong> { }
    class BooleanInfo : SimpleTypeInfo<bool> { }
    class DateTimeInfo : SimpleTypeInfo<DateTime> { }
    class SingleInfo : SimpleTypeInfo<float> { }
    class DoubleInfo : SimpleTypeInfo<double> { }
    class DecimalInfo : SimpleTypeInfo<decimal> { }
    class StringInfo : SimpleTypeInfo<string> { }
}
