using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.TypeAlias
{
#if false // 書きたいコード
    typedef StockId = int;
#endif

    public struct StockId
    {
        int _value;
        public StockId(int value) { _value = value; }
        public static explicit operator int(StockId id) => id._value;
    }
}
