using System.Linq;

namespace FixedPacker.Samples.DefinitionData
{
    class Program
    {
        static void Query(IQueryable<Sample> dataSource)
        {
            var q1 = dataSource.ToArray();
            var q2 = dataSource.Select(x => new { x.A, x.B, x.C });
            var q3 = dataSource.Select(x => new { x.Id, x.B, x.D });
        }
    }
}
