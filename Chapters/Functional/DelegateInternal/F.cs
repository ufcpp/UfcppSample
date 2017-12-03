namespace DelegateInternal
{
    delegate int F(int x);

#if PsudoCode
    class F : Delegate
    {
        object Target;
        IntPtr FunctionPointer;
        // 実際には Delegate クラスのメンバー
        // あと、object がもう1個と、IntPtr がもう1個ある

        public F(object target, IntPtr fp) => (Target, FunctionPointer) = (target, fp);

        public virtual int Invoke(int x)
        {
            // return FunctionPointer(Target); 的な処理
        }
    }
#endif
}
