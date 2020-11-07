using System;

namespace MetricSpace
{
    public class DuplicateNodeError : Exception
    {
        public DuplicateNodeError()
            : base("Cannot Add Node With Duplicate Coordinates")
        {
        }
    }
}
