using System;
using System.Linq.Expressions;

namespace GenericOperator
{
    using Binary = Func<ParameterExpression, ParameterExpression, BinaryExpression>;
    using Unary = Func<ParameterExpression, UnaryExpression>;

    /// <summary>
    /// 動的にジェネリック型 T の加減乗除関数を作る。
    /// </summary>
    /// <typeparam name="T">対象となる型。</typeparam>
    public static class Operator<T>
    {
        static readonly ParameterExpression x = Expression.Parameter(typeof(T), "x");
        static readonly ParameterExpression y = Expression.Parameter(typeof(T), "y");
        static readonly ParameterExpression z = Expression.Parameter(typeof(T), "z");
        static readonly ParameterExpression w = Expression.Parameter(typeof(T), "w");

        public static readonly Func<T, T, T> Add = Lambda(Expression.Add);
        public static readonly Func<T, T, T> Subtract = Lambda(Expression.Subtract);
        public static readonly Func<T, T, T> Multiply = Lambda(Expression.Multiply);
        public static readonly Func<T, T, T> Divide = Lambda(Expression.Divide);
        public static readonly Func<T, T> Plus = Lambda(Expression.UnaryPlus);
        public static readonly Func<T, T> Negate = Lambda(Expression.Negate);

        public static Func<T, T, T> Lambda(Binary op)
        {
            return Expression.Lambda<Func<T, T, T>>(op(x, y), x, y).Compile();
        }

        public static Func<T, T> Lambda(Unary op)
        {
            return Expression.Lambda<Func<T, T>>(op(x), x).Compile();
        }

        public static readonly Func<T, T, T, T, T> ProductSum =
            Expression.Lambda<Func<T, T, T, T, T>>(
                Expression.Add(
                    Expression.Multiply(x, y),
                    Expression.Multiply(z, w)),
                x, y, z, w).Compile();

        public static readonly Func<T, T, T, T, T> ProductDifference =
            Expression.Lambda<Func<T, T, T, T, T>>(
                Expression.Subtract(
                    Expression.Multiply(x, y),
                    Expression.Multiply(z, w)),
                x, y, z, w).Compile();
    }
}
