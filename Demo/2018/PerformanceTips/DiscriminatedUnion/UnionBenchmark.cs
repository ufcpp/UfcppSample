using BenchmarkDotNet.Attributes;
using System;
using System.Linq;

namespace DiscriminatedUnion
{
    /// <summary>
    /// <see cref="IsMatching.StringOrCharArray"/>
    /// <see cref="DiscriminatorField.StringOrCharArray"/>
    /// <see cref="DiscriminatorFieldUnsafe.StringOrCharArray"/>
    /// の3つでまったく同じコードを呼んで、それぞれの string or char[] の取り出しの負担を比べる。
    /// </summary>
    public class UnionBenchmark
    {
        private IsMatching.StringOrCharArray[] _data1;
        private DiscriminatorField.StringOrCharArray[] _data2;
        private DiscriminatorFieldUnsafe.StringOrCharArray[] _data3;

        // 最適化で消えないようにだけしたいので、Length の和を取ってる。
        // 処理内容に意味はなくて、単に繰り返し x.Span を呼びたい。
        // (x.Span の中で型チェックが起きてる。)

        [Benchmark]
        public int IsMatching()
        {
            var sum = 0;
            foreach (var x in _data1) sum += x.Span.Length;
            return sum;
        }

        [Benchmark]
        public int DiscriminatorField()
        {
            var sum = 0;
            foreach (var x in _data2) sum += x.Span.Length;
            return sum;
        }

        [Benchmark]
        public int DiscriminatorFieldUnsafe()
        {
            var sum = 0;
            foreach (var x in _data3) sum += x.Span.Length;
            return sum;
        }

        [GlobalSetup]
        public void Setup()
        {
            const int N = 100;
            var r = new Random(1);

            string getRandomString()
            {
                var len = r.Next(0, 100);
                var buf = new char[len];
                foreach (ref var c in buf.AsSpan())
                    c = (char)r.Next(1, 0xd7ff);
                return new string(buf);
            }

            var strings = new[] { "", "abc", "waesrxdtfygujctvygbji", "ｓれｄｔｆｙｇんｖｔｙｂｇｐｍこ", "TRFYGIJDRFTGYIMJ" }
                .Concat(Enumerable.Range(0, 50).Select(_ => getRandomString()))
                .ToArray();

            _data1 = new IsMatching.StringOrCharArray[N];
            _data2 = new DiscriminatorField.StringOrCharArray[N];
            _data3 = new DiscriminatorFieldUnsafe.StringOrCharArray[N];

            for (int i = 0; i < 100; i++)
            {
                var s = strings[r.Next(0, strings.Length)];

                if (r.NextDouble() < 0.5)
                {
                    _data1[i] = new IsMatching.StringOrCharArray(s);
                    _data2[i] = new DiscriminatorField.StringOrCharArray(s);
                    _data3[i] = new DiscriminatorFieldUnsafe.StringOrCharArray(s);
                }
                else
                {
                    char[] array = s.ToArray();
                    _data1[i] = new IsMatching.StringOrCharArray(array);
                    _data2[i] = new DiscriminatorField.StringOrCharArray(array);
                    _data3[i] = new DiscriminatorFieldUnsafe.StringOrCharArray(array);
                }
            }
        }
    }
}
