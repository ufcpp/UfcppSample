#pragma warning disable CS0168
#pragma warning disable CS8321

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalFunctions
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    class Sample
    {
        public Sample()
        {
            int f(int n) => n * n;
        }

        public int Property
        {
            get
            {
                int f(int n) => n * n;
                return f(10);
            }
        }

        public static Sample operator+(Sample x)
        {
            int f(int n) => n * n;
            return null;
        }
    }
}
