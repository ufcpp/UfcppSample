namespace ValueTypeGenerics.GenericArithmeticOperators
{
    interface IBinaryOperator<T>
    {
        T Zero { get; }
        T Operate(T x, T y);
    }

    struct Add : IBinaryOperator<int>
    {
        public int Zero => 0;
        public int Operate(int x, int y) => x + y;
    }

    struct Mul : IBinaryOperator<int>
    {
        public int Zero => 1;
        public int Operate(int x, int y) => x * y;
    }
}
