using System.Threading;

namespace ThreadPost
{
    public interface IUpdatable
    {
        void Initialize();
        void Upadte(CancellationToken cancellationToken);
    }
}
