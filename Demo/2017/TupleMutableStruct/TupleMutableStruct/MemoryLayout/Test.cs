using System;
using Xunit;

namespace TupleMutableStruct.MemoryLayout
{
    public class VectorTest
    {
        [Fact]
        public void XAndYShouldBeIdentical()
        {
            var r = new Random();

            for (int i = 0; i < 1000; i++)
            {
                var a = r.RandomByte();
                var x = r.RandomVector();
                var y = r.RandomVector();

                var a1 = (SafeAdd.Vector)x + y;
                var a2 = (PointerAdd.Vector)x + y;
                if (!a1.ToTuple().Equals(a2.ToTuple())) throw new InvalidOperationException();

                var m1 = a * (SafeAdd.Vector)x;
                var m2 = a * (PointerAdd.Vector)x;
                if (!m1.ToTuple().Equals(m2.ToTuple())) throw new InvalidOperationException();
            }
        }
    }
}
