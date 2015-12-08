using System.Threading.Tasks;

namespace Common
{
    public interface IInterface1
    {
        string Name { get; }

        Task<int> GetAsync();
    }
}
