using ClassLibrary1;
using ConsoleApp1;

var t = typeof(Sample);
m<T4Generator>(t);
m<InterpolationGenerator>(t);

static void m<T>(Type type)
    where T : IGenerator
    => Console.WriteLine(T.Create(type).TransformText());
