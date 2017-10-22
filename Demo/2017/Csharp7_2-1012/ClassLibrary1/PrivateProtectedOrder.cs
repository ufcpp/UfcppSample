namespace ClassLibrary1
{
    class PrivateProtectedOrder
    {
        // どちらの順序でも同じ意味
        protected internal int A1;
        internal protected int A2;

        private protected int B1;
        protected private int B2;
    }
}
