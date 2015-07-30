using System;
using System.Collections.Generic;
using System.Linq;

namespace ValueTuples.Reflection
{
    public class ArrayTypeInfo : RecordTypeInfo
    {
        public RecordTypeInfo ElementType { get; }

        internal ArrayTypeInfo(RecordTypeInfo elementType)
        {
            ElementType = elementType;
        }

        public override bool IsArray => true;

        public override IEnumerable<RecordFieldInfo> Fields => Enumerable.Empty<RecordFieldInfo>();

        public override Type Type => ElementType.Type.MakeArrayType();

        public override IRecordAccessor GetAccessor(object instance)
        {
            throw new NotImplementedException();
        }

        public override Array GetArray(int length) => ElementType.GetArray(length);

        public override object GetInstance() => ElementType.GetInstance();
    }
}
