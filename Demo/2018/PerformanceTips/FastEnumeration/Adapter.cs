using System;
using System.Collections;
using System.Collections.Generic;

namespace FastEnumeration
{
    struct Adapter<T> : IEnumerator<T>
    {
        private IFastEnumerator<T> _enumerator;

        public Adapter(IFastEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
            Current = default;
        }

        public T Current { get; private set; }

        public bool MoveNext()
        {
            Current = _enumerator.TryMoveNext(out var success);
            return success;
        }

        object IEnumerator.Current => Current;
        public void Dispose() { }
        public void Reset() => throw new NotImplementedException();
    }
}
