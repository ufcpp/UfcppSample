using System;

namespace NullPointer.ManualNullCheck
{
    class CallHierarchy
    {
        public void Run() => A();

        void A() => B(null);
        void B(string s) => C(s);
        void C(string s) => D(s);
        void D(string s) => E(s);
        void E(string s) => F(s);
        void F(string s) => Console.WriteLine(s.Length);
    }
}
