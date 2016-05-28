using System;

interface ICalculator<T>
{
    string Name { get; }
    T[] GetSeries(Random r, int count);
    T SeriesSum(T[] series);
}
