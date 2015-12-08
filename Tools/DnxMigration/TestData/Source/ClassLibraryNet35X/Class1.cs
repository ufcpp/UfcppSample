using System.Threading.Tasks;

namespace ClassLibraryNet35X
{
    public class Class1 : Common.IInterface1
    {
        public string Name => "net35/project.json";

        public async Task<int> GetAsync()
        {
            await Task.Delay(1);
            return 351;
        }
    }
}
