namespace ValueTypeGenerics.GenericArithmeticOperators
{
    interface IBinaryOperator<T>
    {
        T Operate(T x, T y);
    }

    struct Add : IBinaryOperator<int>
    {
        public int Operate(int x, int y) => x + y;
    }

    struct Mul : IBinaryOperator<int>
    {
        public int Operate(int x, int y) => x * y;
    }
}
