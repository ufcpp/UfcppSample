using System;

namespace LibModReq
{
    public class Class1
    {
        public void UnamanagedConstraint<T>() where T : unmanaged { }
        public virtual void InParameter(in int x) { }
    }
}
