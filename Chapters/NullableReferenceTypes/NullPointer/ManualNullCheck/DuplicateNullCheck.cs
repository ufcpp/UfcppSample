using System;

namespace NullPointer.ManualNullCheck
{
    class DuplicateNullCheck
    {
        void A(string s)
        {
            if (s != null) B(s);
        }

        void B(string s)
        {
            if(s != null)
                Console.WriteLine(s.Length);
        }
    }
}
