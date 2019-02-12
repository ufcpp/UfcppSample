using System;
using System.Collections.Generic;
using System.Text;

namespace Patterns
{
    class TupleSwitch
    {
        int Compare(int? a, int? b)
        {
            switch (a, b)
            {
                case (a: null, null): return 0;
                case (int _, null): return -1;
                case (null, int _): return -1;
                case (int a1, int b1): return a1.CompareTo(b1);
            }
        }
    }
}
