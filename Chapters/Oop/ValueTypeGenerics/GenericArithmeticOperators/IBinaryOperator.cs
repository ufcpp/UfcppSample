namespace ValueTypeGenerics.GenericArithmeticOperators
{
    interface IBinaryOperator<T>
    {
        T Operate(T x, T y);
    }

    struct AddOperation : IBinaryOperator<int>
    {
        public int Operate(int x, int y) => x + y;
    }

    struct MulOperation : IBinaryOperator<int>
    {
        public int Operate(int x, int y) => x * y;
    }
}
