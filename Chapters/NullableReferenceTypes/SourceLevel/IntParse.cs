using System;
using System.Collections.Generic;
using System.Text;

namespace SourceLevel
{
    class IntParse
    {
        public static void M()
        {
#nullable enable
            int.Parse(null);

#nullable disable
            int.Parse(null);
        }
    }
}
