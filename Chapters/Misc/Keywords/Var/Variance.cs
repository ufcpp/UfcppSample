using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keywords.Var
{
    class Variance
    {
        static double Calc(IEnumerable<double> data)
        {
            int count = 0;
            double sum = 0;
            double sqSum = 0;

            foreach (double x in data)
            {
                ++count;
                sum += x;
                sqSum += x * x;
            }

            // 分散(variance)。ローカル変数だし略して var って名前つける人はいる
            double var = (sum * sum - sqSum) / count;
            return var;
        }
    }
}
