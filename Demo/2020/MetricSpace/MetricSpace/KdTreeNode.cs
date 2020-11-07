using System;
using System.Text;

namespace MetricSpace
{
    public partial class Point<T, TArithmetic>
    {
        public partial class Dimension<TArray, TArrayAccessor>
        {
            public partial class Metric<TMetric>
            {
                public partial class KdTree<TValue>
                {
                    [Serializable]
                    public class Node
                    {
                        public Node(TArray point, TValue value)
                        {
                            Point = point;
                            Value = value;
                        }

                        public TArray Point;
                        public TValue Value;

                        internal Node? LeftChild = null;
                        internal Node? RightChild = null;

                        internal ref Node? this[int compare]
                        {
                            get
                            {
                                if (compare <= 0)
                                    return ref LeftChild;
                                else
                                    return ref RightChild;
                            }
                        }

                        public bool IsLeaf
                        {
                            get
                            {
                                return (LeftChild == null) && (RightChild == null);
                            }
                        }

                        public override string ToString()
                        {
                            var sb = new StringBuilder();

                            var accessor = default(TArrayAccessor);
                            var dim = accessor.Length;
                            var p = Point;
                            for (var i = 0; i < dim; i++)
                            {
                                sb.Append(accessor.At(ref p, i).ToString() + "\t");
                            }

                            sb.Append(Value?.ToString() ?? "null");

                            return sb.ToString();
                        }
                    }
                }
            }
        }
    }
}
