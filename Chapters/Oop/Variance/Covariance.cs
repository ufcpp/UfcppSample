using System;

namespace Variance
{
    class Covariance
    {
        delegate void F(Derived x);

        void M(Base x) { }

        void DelegateVariance()
        {
            F x = M;
        }

        void GenericVariance()
        {
            Action<Base> f = M;
            Action<Derived> g = f;
        }
    }
}
