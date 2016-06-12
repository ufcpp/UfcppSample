using System;

namespace Variance
{
    class Contravariance
    {
        delegate Base F();

        Derived M() { return default(Derived); }

        void DelegateVariance()
        {
            F f = M;
        }

        void GenericVariance()
        {
            Func<Derived> f = M;
            Func<Base> g = f;
        }
    }
}
