using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variance
{
    class Program
    {
        static void Main(string[] args)
        {
            ArrayCovariance.F();
        }
    }

    public interface IObserver<in T>
    {
        void OnCompleted();
        void OnError(Exception error);
        void OnNext(T value);
    }

    public interface IObservable<out T>
    {
        IDisposable Subscribe(IObserver<T> observer);
    }
}
