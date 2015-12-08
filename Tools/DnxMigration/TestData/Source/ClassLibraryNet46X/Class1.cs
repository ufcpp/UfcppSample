using System.Threading.Tasks;

namespace ClassLibraryNet46X
{
    public class Class1 : Common.IInterface1
    {
        public string Name => "net46/project.json";

        public async Task<int> GetAsync()
        {
            await Task.Delay(1);
            return 461;
        }
    }
}
