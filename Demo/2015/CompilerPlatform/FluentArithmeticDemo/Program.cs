using System;
using FluentArithmetic;

namespace FluentArithmeticDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = 1
                .Add(2)
                .Mul(3)
                .Sub(1)
                .Div(4);

            Console.WriteLine(x);
        }
    }
}
