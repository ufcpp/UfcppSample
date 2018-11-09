using System.Linq;
using Xunit;

namespace Enumeration
{
    public class EnumerationTest
    {
        private static ListLike<int> _data;
        private static readonly int _expected;

        static EnumerationTest()
        {
            var array = Enumerable.Range(0, 1000).ToArray();
            _data = new ListLike<int> { Array = array, Length = 500 };

            _expected = array.Take(500).Sum();
        }

        [Fact] public void SumArray() => Assert.Equal(_expected, Sum.SumArray(_data));
        [Fact] public void SumSpan() => Assert.Equal(_expected, Sum.SumSpan(_data));
        [Fact] public void SumEnumerable() => Assert.Equal(_expected, Sum.SumEnumerable(_data));
        [Fact] public void SumFastEnumerable() => Assert.Equal(_expected, Sum.SumFastEnumerable(_data));
        [Fact] public void SumEnumeratorInterface() => Assert.Equal(_expected, Sum.SumEnumeratorInterface(_data.GetEnumerator()));
        [Fact] public void SumFastEnumeratorInterface() => Assert.Equal(_expected, Sum.SumFastEnumeratorInterface(_data.GetFastEnumerator()));
    }
}
